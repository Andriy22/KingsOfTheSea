using Application.Common.DTOs.BattleShip;
using Application.Stats.Commands.CreateStatsCommand;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;

namespace API.Hubs;

public struct WaitingPlayer
{
    public string ConnectionId;
    public string PlayerId;
    public List<ShipPosition> Ships;
}

[EnableCors("signalr")]
[Authorize]
public class BattleshipsHub : Hub
{
    private static readonly List<WaitingPlayer> _waitingPlayers = new();
    private static readonly List<GameSession> _activeGames = new();
    private readonly IMediator _mediator;
    private readonly UserManager<Player> _userManager;

    public BattleshipsHub(UserManager<Player> userManager, IMediator mediator)
    {
        _userManager = userManager;
        _mediator = mediator;
    }

    public async Task FindGame(string shipsJson)
    {
        var ships = JsonConvert.DeserializeObject<ShipPosition[]>(shipsJson);

        var player = RemovePlayerFromWaitingListIfPresent(Context?.User?.Identity?.Name);

        AddPlayerToWaitingList(Context.ConnectionId, Context?.User?.Identity?.Name, ships.ToList());

        var game = TryCreateGameSessionFromWaitingPlayers();
        if (game != null)
        {
            _activeGames.Add(game);
            await StartGameForPlayers(game);
        }
    }

    private WaitingPlayer RemovePlayerFromWaitingListIfPresent(string playerId)
    {
        var player = _waitingPlayers.FirstOrDefault(x => x.PlayerId == playerId);
        if (!player.Equals(default(WaitingPlayer)))
        {
            _waitingPlayers.Remove(player);
        }
        return player;
    }

    private void AddPlayerToWaitingList(string connectionId, string playerId, List<ShipPosition> ships)
    {
        _waitingPlayers.Add(new WaitingPlayer
        {
            ConnectionId = connectionId,
            Ships = ships,
            PlayerId = playerId
        });
    }

    private GameSession TryCreateGameSessionFromWaitingPlayers()
    {
        if (_waitingPlayers.Count >= 2)
        {
            var player1 = _waitingPlayers[0];
            var player2 = _waitingPlayers[1];
            _waitingPlayers.RemoveRange(0, 2);

            var player1Board = CreateBoardFromShips(player1.Ships);
            var player2Board = CreateBoardFromShips(player2.Ships);

            var game = new GameSession
            {
                Player1ConnectionId = player1.ConnectionId,
                Player2ConnectionId = player2.ConnectionId,
                Player1Ships = player1.Ships,
                Player2Ships = player2.Ships,
                Player1Turn = new Random().Next(0, 1) == 1,
                Player1Board = player1Board,
                Player2Board = player2Board,
                Player1Id = player1.PlayerId,
                Player2Id = player2.PlayerId
            };

            return game;
        }

        return null;
    }

    private List<List<int>> CreateBoardFromShips(List<ShipPosition> ships)
    {
        var board = new List<List<int>>();
        for (var i = 0; i < 10; i++)
        {
            board.Add(new List<int>(Enumerable.Repeat(0, 10)));
        }

        foreach (var ship in ships)
        {
            for (var i = 0; i < ship.Length; i++)
            {
                if (ship.IsHorizontal)
                {
                    board[ship.X][ship.Y + i] = 1;
                }
                else
                {
                    board[ship.X + i][ship.Y] = 1;
                }
            }
        }

        return board;
    }

    private async Task StartGameForPlayers(GameSession game)
    {
        // Get user objects for each player
        var userA = await _userManager.FindByIdAsync(game?.Player1Id);
        var userB = await _userManager.FindByIdAsync(game?.Player2Id);

        // Calculate new Elo ratings based on game outcome
        var ratingsForAWinner = EloSystem.CalculateNewRatings(userA.EloRating, userB.EloRating, 1, 0);
        var ratingsForBWinner = EloSystem.CalculateNewRatings(userA.EloRating, userB.EloRating, 0, 1);

        // Build game info object with player details and Elo ratings
        var gameInfo = BuildGameInfo(game, userA, userB, ratingsForAWinner, ratingsForBWinner);

        // Send start game message to each player with their own board and opponent's board without ships
        await SendStartGameMessage(game.Player1ConnectionId, game.Player1Board, game.Player2Board, gameInfo);
        await SendStartGameMessage(game.Player2ConnectionId, game.Player2Board, game.Player1Board, gameInfo);

        // Send turn message to each player with opponent's connection ID
        await SendTurnMessage(game.Player1ConnectionId, game.Player2ConnectionId, game.Player1Turn);
        await SendTurnMessage(game.Player2ConnectionId, game.Player1ConnectionId, !game.Player1Turn);
    }

    // Build game info object with player details and Elo ratings
    private GameInfoVM BuildGameInfo(GameSession game, Player userA, Player userB, (int, int)ratingsForAWinner, (int, int) ratingsForBWinner)
    {
        return new GameInfoVM
        {
            Player1ConnectionId = game.Player1ConnectionId,
            Player2ConnectionId = game.Player2ConnectionId,
            Player1Name = userA?.Nickname,
            Player2Name = userB?.Nickname,
            Player1Elo = userA.EloRating,
            Player2Elo = userB.EloRating,
            Player1EloWinDelta = ratingsForAWinner.Item1 - userA.EloRating,
            Player2EloWinDelta = ratingsForBWinner.Item2 - userB.EloRating,
            Player1EloLoseDelta = ratingsForBWinner.Item1 - userA.EloRating,
            Player2EloLoseDelta = ratingsForAWinner.Item2 - userB.EloRating
        };
    }

    // Send start game message to player with their own board and opponent's board without ships
    private async Task SendStartGameMessage(string connectionId, List<List<int>> playerBoard, List<List<int>> opponentBoard, GameInfoVM gameInfo)
    {
        await Clients.Client(connectionId).SendAsync("startGame", playerBoard, GameSession.GetBoardWithoutShips(opponentBoard), gameInfo);
    }

    // Send turn message to player with opponent's connection ID
    private async Task SendTurnMessage(string connectionId, string opponentConnectionId, bool isPlayerTurn)
    {
        await Clients.Client(connectionId).SendAsync("turn", isPlayerTurn ? connectionId : opponentConnectionId);
    }


    public void CancelFindGame()
    {
        var waitingPlayer = _waitingPlayers.FirstOrDefault(p => p.ConnectionId == Context.ConnectionId);
        if (waitingPlayer.ConnectionId != null) _waitingPlayers.Remove(waitingPlayer);
    }

    public async Task FireShot(int x, int y)
    {
        // Find the game session for the current player
        var game = _activeGames.FirstOrDefault(g =>
            g.Player1ConnectionId == Context.ConnectionId || g.Player2ConnectionId == Context.ConnectionId);
        if (game == null)
            // Game session not found
            return;

        // Check whose turn it is
        if ((game.Player1Turn && game.Player1ConnectionId != Context.ConnectionId) ||
            (!game.Player1Turn && game.Player2ConnectionId != Context.ConnectionId))
            // Not the current player's turn
            return;

        // Process the shot and notify the players
        bool isHit;
        var isGameOver = game.ProcessShot(x, y, out isHit);

        Clients.Client(game.Player1ConnectionId).SendAsync("boardsUpdated", game.Player1Board,
            GameSession.GetBoardWithoutShips(game.Player2Board));
        Clients.Client(game.Player2ConnectionId).SendAsync("boardsUpdated", game.Player2Board,
            GameSession.GetBoardWithoutShips(game.Player1Board));

        if (isGameOver)
        {
            game.IsGameOver(out var winner);

            Clients.Client(game.Player1ConnectionId).SendAsync("gameOver", winner);
            Clients.Client(game.Player2ConnectionId).SendAsync("gameOver", winner);

            var userA = await _userManager.FindByIdAsync(game?.Player1Id);
            var userB = await _userManager.FindByIdAsync(game?.Player2Id);

            var ratings = EloSystem.CalculateNewRatings(userA.EloRating, userB.EloRating, winner == game.Player1ConnectionId ? 1 : 0, winner == game.Player1ConnectionId ? 0 : 1);

            await UpdateUserRating(userA, ratings.Item1);
            await UpdateUserRating(userB, ratings.Item2);

            await CreatePlayerStats(userA, ratings.Item1, winner == game.Player1ConnectionId);
            await CreatePlayerStats(userB, ratings.Item2, winner != game.Player1ConnectionId);

            _activeGames.Remove(game);
        }

        if (!isGameOver)
        {
            if (!isHit) game.Player1Turn = !game.Player1Turn;

            Clients.Client(game.Player1ConnectionId).SendAsync("turn",
                game.Player1Turn ? game.Player1ConnectionId : game.Player2ConnectionId);
            Clients.Client(game.Player2ConnectionId).SendAsync("turn",
                game.Player1Turn ? game.Player1ConnectionId : game.Player2ConnectionId);
        }
    }

    public override async Task<Task> OnDisconnectedAsync(Exception exception)
    {
        // Remove the player from the waiting list and any active games
        var waitingPlayer = _waitingPlayers.FirstOrDefault(p => p.ConnectionId == Context.ConnectionId);
        if (waitingPlayer.ConnectionId != null) _waitingPlayers.Remove(waitingPlayer);

        var game = _activeGames.FirstOrDefault(g =>
            g.Player1ConnectionId == Context.ConnectionId || g.Player2ConnectionId == Context.ConnectionId);
        if (game != null)
        {
            _activeGames.Remove(game);

            // Notify the other player that their opponent disconnected
            Clients.Client(game.Player1ConnectionId == Context.ConnectionId
                ? game.Player2ConnectionId
                : game.Player1ConnectionId).SendAsync("OpponentDisconnected");

            var userA = await _userManager.FindByIdAsync(game?.Player1Id);
            var userB = await _userManager.FindByIdAsync(game?.Player2Id);

            var ratingsForAWinner = EloSystem.CalculateNewRatings(userA.EloRating, userB.EloRating, 1, 0);
            var ratingsForBWinner = EloSystem.CalculateNewRatings(userA.EloRating, userB.EloRating, 0, 1);

            if (Context.ConnectionId != game.Player1ConnectionId)
            {
                await _mediator.Send(new UpdateRatingCommand
                    { UserId = userA.Id, NewEloRating = ratingsForAWinner.Item1 });
                await _mediator.Send(new UpdateRatingCommand
                    { UserId = userB.Id, NewEloRating = ratingsForAWinner.Item2 });

                await _mediator.Send(new CreateStatsCommand
                {
                    Date = DateTime.Now,
                    EloDelta = ratingsForAWinner.Item1 - userA.EloRating,
                    PlayerId = userA.Id,
                    IsWin = true
                });

                await _mediator.Send(new CreateStatsCommand
                {
                    Date = DateTime.Now,
                    EloDelta = ratingsForAWinner.Item2 - userB.EloRating,
                    PlayerId = userB.Id,
                    IsWin = false
                });
            }
            else
            {
                await _mediator.Send(new UpdateRatingCommand
                    { UserId = userA.Id, NewEloRating = ratingsForBWinner.Item1 });
                await _mediator.Send(new UpdateRatingCommand
                    { UserId = userB.Id, NewEloRating = ratingsForBWinner.Item2 });


                await _mediator.Send(new CreateStatsCommand
                {
                    Date = DateTime.Now,
                    EloDelta = ratingsForBWinner.Item1 - userA.EloRating,
                    PlayerId = userA.Id,
                    IsWin = false
                });

                await _mediator.Send(new CreateStatsCommand
                {
                    Date = DateTime.Now,
                    EloDelta = ratingsForBWinner.Item2 - userB.EloRating,
                    PlayerId = userB.Id,
                    IsWin = true
                });
            }
        }

        return base.OnDisconnectedAsync(exception);
    }

    private async Task UpdateUserRating(Player user, int newEloRating)
    {
        await _mediator.Send(new UpdateRatingCommand { UserId = user.Id, NewEloRating = newEloRating });
    }

    private async Task CreatePlayerStats(Player user, int newEloRating, bool isWin)
    {
        await _mediator.Send(new CreateStatsCommand
        {
            Date = DateTime.Now,
            EloDelta = newEloRating - user.EloRating,
            PlayerId = user.Id,
            IsWin = isWin
        });
    }
}