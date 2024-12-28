namespace Zawieszka.Server;

public class Lobby
{
    public static int MAX_PLAYERS = 6;
    public List<User> Users { get; } = new(6);

    public Lobby()
    {
        for (var i = 0; i < MAX_PLAYERS; i++)
        {
            Users.Add(null);
        }
    }

    public bool TakeSeat(int seat, User user)
    {
        var seatedUser = Users[seat];
        if (seatedUser == null)
        {
            EmptySeat(user.PeerId);
            Users[seat] = user;
            return true;
        }

        if (seatedUser.PeerId != user.PeerId)
        {
            return false;
        }
        
        Users[seat] = user;
        return true;
    }
    
    public bool EmptySeat(int peerId)
    {
        var user = Users.FirstOrDefault(u => u is not null && u.PeerId == peerId);
        if (user is null)
        {
            return false;
        }

        Users[Users.IndexOf(user)] = null;
        return true;
    }
}