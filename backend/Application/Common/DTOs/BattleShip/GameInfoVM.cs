namespace Application.Common.DTOs.BattleShip;

public class GameInfoVM
{
    public string Player1ConnectionId { get; set; }
    public string Player2ConnectionId { get; set; }

    public string Player1Name { get; set; }
    public string Player2Name { get; set; }

    public int Player1Elo { get; set; }
    public int Player2Elo { get; set; }

    public int Player1EloWinDelta { get; set; }
    public int Player2EloWinDelta { get; set; }

    public int Player1EloLoseDelta { get; set; }
    public int Player2EloLoseDelta { get; set; }
}