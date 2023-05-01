using Application.Common.DTOs.BattleShip;

namespace API.Hubs;

public class GameSession
{
    public string Player1ConnectionId { get; set; }
    public string Player2ConnectionId { get; set; }

    public string Player1Id { get; set; }
    public string Player2Id { get; set; }

    public List<ShipPosition> Player1Ships { get; set; }
    public List<ShipPosition> Player2Ships { get; set; }
    public bool Player1Turn { get; set; }
    public List<List<int>> Player1Board { get; set; }
    public List<List<int>> Player2Board { get; set; }


    public bool IsGameOver(out string winner)
    {
        // Check if all of one player's ships have been sunk
        var player1ShipsSunk = Player1Board.Sum(row => row.Count(cell => cell == 1));
        var player2ShipsSunk = Player2Board.Sum(row => row.Count(cell => cell == 1));
        if (player1ShipsSunk == 0)
        {
            winner = Player2ConnectionId;
            return true;
        }

        if (player2ShipsSunk == 0)
        {
            winner = Player1ConnectionId;
            return true;
        }

        winner = null;
        return false;
    }

    public bool ProcessShot(int x, int y, out bool isHit)
    {
        isHit = false;

        var board = Player1Turn ? Player2Board : Player1Board;
        if (x < 0 || x >= board.Count || y < 0 || y >= board[0].Count)
            throw new ArgumentException("Invalid coordinates");

        var cellValue = board[x][y];
        switch (cellValue)
        {
            case 0:
                board[x][y] = 4;
                break;
            case 1:
                board[x][y] = 2;
                isHit = true;
                if (IsShipSunk(board, x, y))
                {
                    UpdateSunkShip(board, x, y);
                    MarkAdjacentCells(board, x, y);
                }

                break;
        }

        return IsGameOver(out var winner);
    }

    private void MarkAdjacentCells(List<List<int>> board, int x, int y)
    {
        int[] dx = { -1, 0, 1, 0, -1, 1, 1, -1 };
        int[] dy = { 0, 1, 0, -1, 1, 1, -1, -1 };

        var visited = new HashSet<(int, int)>();
        var queue = new Queue<(int, int)>();
        queue.Enqueue((x, y));
        visited.Add((x, y));

        while (queue.Count > 0)
        {
            (var currX, var currY) = queue.Dequeue();

            for (var i = 0; i < 8; i++)
            {
                var newX = currX + dx[i];
                var newY = currY + dy[i];

                if (newX >= 0 && newX < board.Count && newY >= 0 && newY < board[0].Count && board[newX][newY] == 0)
                    board[newX][newY] = 4;
            }

            for (var i = 0; i < 4; i++)
            {
                var newX = currX + dx[i];
                var newY = currY + dy[i];

                while (newX >= 0 && newX < board.Count && newY >= 0 && newY < board[0].Count)
                {
                    if (board[newX][newY] == 3 && !visited.Contains((newX, newY)))
                    {
                        queue.Enqueue((newX, newY));
                        visited.Add((newX, newY));
                    }

                    if (board[newX][newY] == 0 || board[newX][newY] == 4) break;
                    newX += dx[i];
                    newY += dy[i];
                }
            }
        }
    }

    private bool IsShipSunk(List<List<int>> board, int x, int y)
    {
        int[] dx = { -1, 1, 0, 0 };
        int[] dy = { 0, 0, -1, 1 };

        for (var i = 0; i < 4; i++)
        {
            var newX = x + dx[i];
            var newY = y + dy[i];

            while (newX >= 0 && newX < board.Count && newY >= 0 && newY < board[0].Count)
            {
                if (board[newX][newY] == 1) return false;
                if (board[newX][newY] == 0 || board[newX][newY] == 4) break;
                newX += dx[i];
                newY += dy[i];
            }
        }

        return true;
    }

    private void UpdateSunkShip(List<List<int>> board, int x, int y)
    {
        int[] dx = { -1, 1, 0, 0 };
        int[] dy = { 0, 0, -1, 1 };

        board[x][y] = 3;

        for (var i = 0; i < 4; i++)
        {
            var newX = x + dx[i];
            var newY = y + dy[i];

            while (newX >= 0 && newX < board.Count && newY >= 0 && newY < board[0].Count)
            {
                if (board[newX][newY] == 2) UpdateSunkShip(board, newX, newY);
                if (board[newX][newY] == 0 || board[newX][newY] == 4) break;
                newX += dx[i];
                newY += dy[i];
            }
        }
    }


    public static List<List<int>> GetBoardWithoutShips(List<List<int>> board)
    {
        var result = new List<List<int>>();

        foreach (var line in board)
        {
            var lineResult = new List<int>();
            foreach (var item in line)
                if (item == 1)
                    lineResult.Add(0);
                else
                    lineResult.Add(item);

            result.Add(lineResult);
        }

        return result;
    }
}