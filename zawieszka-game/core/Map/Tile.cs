namespace ZawieszkaCore.Map;

public class Tile
{
    public required int Id { get; init; }
    public required string Name { get; init; }
    public required LinkedList<Road> Roads { get; init; } = [];
}