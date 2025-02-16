using ZawieszkaCore.Map;

namespace ZawieszkaCoreTests.Map;

public static class WorldFactory
{
    public static World BiggerTestWorld()
    {
        List<Tile> tiles =
        [
            GetTile(0, [new Road(1, 2), new Road(5)]),
            GetTile(1, [new Road(0), new Road(2)]),
            GetTile(2, [new Road(1), new Road(3)]),
            GetTile(3, [new Road(2), new Road(4)]),
            GetTile(4, [new Road(3), new Road(5), new Road(6), new Road(8, 3)]),
            GetTile(5, [new Road(0), new Road(4)]),
            GetTile(6, [new Road(7), new Road(8)]),
            GetTile(7, [new Road(6)]),
            GetTile(8, [new Road(4), new Road(6)])
        ];
        return new World(tiles);
    }
    
    public static World SimpleTestWorld()
    {
        List<Tile> tiles =
        [
            GetTile(0, [new Road(1), new Road(2)]),
            GetTile(1, [new Road(0), new Road(2)]),
            GetTile(2, [new Road(0), new Road(1)])
        ];
        return new World(tiles);
    }
    
    public static World LineTestWorld()
    {
        List<Tile> tiles =
        [
            GetTile(0, [new Road(1)]),
            GetTile(1, [new Road(0), new Road(2, 2)]),
            GetTile(2, [new Road(1), new Road(3, 2)]),
            GetTile(3, [new Road(2)]),
        ];
        return new World(tiles);
    }

    private static Tile GetTile(int id, IEnumerable<Road> roads)
    {
        return new Tile { Id = id, Name = $"T{id}", Roads = new LinkedList<Road>(roads) };
    }
}