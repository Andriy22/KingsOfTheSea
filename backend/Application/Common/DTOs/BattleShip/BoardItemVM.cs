namespace Application.Common.DTOs.BattleShip;

public class BoardItemVM
{
    public string Key { get; set; }
    public string Type { get; set; }
    public string Rotation { get; set; }

    public int X { get; set; }
    public int Y { get; set; }

    public List<PartItem> Parts { get; set; }
}

public class PartItem
{
    public string Key { get; set; }
    public string Type { get; set; }
}