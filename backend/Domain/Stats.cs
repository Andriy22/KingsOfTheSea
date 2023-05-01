namespace Domain;

public class Stats
{
    public int Id { get; set; }
    public Player Player { get; set; }
    public bool IsWin { get; set; }
    public int EloDelta { get; set; }
    public DateTime Date { get; set; }
}