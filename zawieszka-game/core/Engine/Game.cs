namespace ZawieszkaCore.Engine;

public class Game(List<Player> players)
{
    private RoundTracker RoundTracker { get; } = new(players);

    public Player NextTurn()
    {
        var nextTurn = RoundTracker.NextTurn();
        return nextTurn.Match(
            playerTurn => playerTurn.Player,
            systemTurn => NextTurn());
    }

    public Player? GetActivePlayer()
    {
        return RoundTracker.ActiveTurn.MatchPlayerTurn<Player?>(
            playerTurn => playerTurn.Player, 
            () => null
        );
    }
}