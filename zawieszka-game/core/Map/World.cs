namespace ZawieszkaCore.Map;

public class World
{
    public List<Tile> Tiles { get; set; } = [];

    public World(List<Tile> tiles)
    {
        for (var i = 0; i < tiles.Count; i++)
        {
            if (Tiles[i].Id != i)
            {
                throw new ArgumentException($"Invalid tile list - pos({i}) != Id({Tiles[i].Id})");
            }
        }

        if (tiles.SelectMany(x => x.Roads).Select(road => road.DestinationId).Any(InvalidId))
        {
            throw new ArgumentException($"One or more roads are invalid");
        }
        
        Tiles = tiles;

        return;

        bool InvalidId(int id) => id < 0 && id >= tiles.Count;
    }

    public IEnumerable<int> AllowedMoves(int startingTile, int moveStrength)
    {
        List<int> visited = [];
        
        var legal = new LinkedList<int>();
        
        Helper(startingTile, moveStrength);
        return legal;

        void Helper(int act, int remainingStrength)
        {
            if (remainingStrength <= 0)
            {
                legal.AddLast(act);
                return;
            }
            visited.Add(act);
            
            foreach (var road in Tiles[act].Roads)
            {
                if (visited.Contains(road.DestinationId))
                {
                    continue;
                }
                Helper(road.DestinationId, remainingStrength - road.Weight);
                visited.Remove(road.DestinationId);
            }
        }
    }
}