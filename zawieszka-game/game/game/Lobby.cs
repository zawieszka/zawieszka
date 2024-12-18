namespace ZawieszkaCore;

public class Lobby
{
    public static int MAX_PLAYERS = 6;
    public static int MIN_PLAYERS = 2;
    public List<Player> Players { get; } = new(6);

    public bool AddPlayer(Player player)
    {
        if (Players.Count > MAX_PLAYERS)
        {
            return false;
        }

        if (Players.Contains(player))
        {
            throw new ArgumentException("Player is already in a lobby");
        }
        
        Players.Add(player);
        return true;
    }

    public void RemovePlayer(Player player)
    {
        Players.Remove(player);
    }

    public Game StartGame()
    {
        if (Players.Count < MIN_PLAYERS)
        {
            throw new SystemException("Insufficient player count");
        }
        
        return new Game(Players);
    }
}