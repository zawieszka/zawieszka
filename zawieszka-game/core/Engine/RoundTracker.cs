using Dunet;

namespace ZawieszkaCore.Engine;

internal class RoundTracker
{
    public int RoundNumber { get; private set; } = 1;
    public Turn ActiveTurn { get; private set; }
    private List<Player> Players { get; }
    private Queue<Player> PlayerQueue { get; set; }

    public RoundTracker(IEnumerable<Player> players)
    {
        Players = players.ToList();
        if (Players.Count == 0)
        {
            throw new ArgumentException("At least one player is required.");
        }
        
        PlayerQueue = new Queue<Player>(Players.Skip(1));
        ActiveTurn = new Turn.PlayerTurn(Players.First()){RoundNumber = RoundNumber};
    }
    
    public Turn NextTurn()
    {
        if (PlayerQueue.Count != 0)
        {
            return ActiveTurn = new Turn.PlayerTurn(PlayerQueue.Dequeue())
            {
                RoundNumber = RoundNumber
            };
        }

        RoundNumber++;
        PlayerQueue = new Queue<Player>(Players);
        return ActiveTurn = new Turn.SystemTurn
        {
            RoundNumber = RoundNumber
        };
    }
}

[Union]
internal partial record Turn
{
    partial record PlayerTurn(Player Player);
    partial record SystemTurn;
    public required int RoundNumber { get; init; }
}