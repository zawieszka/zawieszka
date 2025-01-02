namespace ZawieszkaCore.Map;

public readonly struct Road(int destination, int weight = 1)
{
    public int DestinationId { get; init; } = destination;
    public int Weight { get; init; } = weight;
}