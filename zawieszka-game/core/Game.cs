namespace ZawieszkaCore;

public class Game(List<Player> players)
{
    private int _playerTurn = 0;
    private List<Player> Players { get; } = players;

    public void NextTurn()
    {
        _playerTurn = ++_playerTurn % Players.Count;
    }

    public Player GetActivePlayer()
    {
        return Players[_playerTurn];
    }
}