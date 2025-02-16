namespace ZawieszkaCore.Map;

public class World
{
    public IReadOnlyList<Tile> Tiles { get; init; } = [];

    public World(List<Tile> tiles)
    {
        for (var i = 0; i < tiles.Count; i++)
        {
            if (tiles[i].Id != i)
            {
                throw new ArgumentException($"Invalid tile list - pos({i}) != Id({tiles[i].Id})");
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
        return legal.Distinct();

        void Helper(int act, int remainingStrength)
        {
            if (remainingStrength <= 0)
            {
                legal.AddLast(act);
                return;
            }
            visited.Add(act);
            
            var next = Tiles[act].Roads.Where(road => !visited.Contains(road.DestinationId)).ToList();
            if (next.Count != 0)
            {
                foreach (var road in next)
                {
                    if (visited.Contains(road.DestinationId))
                    {
                        continue;
                    }
                    Helper(road.DestinationId, remainingStrength - road.Weight);
                    visited.Remove(road.DestinationId);
                }
            }
            else
            {
                legal.AddLast(act);
            }
            
        }
    }
}