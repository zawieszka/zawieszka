namespace Zawieszka.Server;

public class Lobby
{
    public static int MAX_PLAYERS = 6;
    public static int MIN_PLAYERS = 2;
    public List<User> Users { get; } = new(6);

    public bool PutUser(int peerId, string username)
    {
        if (Users.Any(u => u.Username == username))
        {
            return false;
        }
        
        var user = Users.FirstOrDefault(u => u.PeerId == peerId);
        if (user is not null)
        {
            user.Username = username;
            return true;
        }

        if (Users.Count > MAX_PLAYERS)
        {
            return false;
        }

        Users.Add(new User { PeerId = peerId, Username = username });
        return true;
    }

    public bool RemoveUser(int peerId)
    {
        return Users.Remove(Users.FirstOrDefault(u => u.PeerId == peerId));
    }

    /*public StartGame()
    {
        if (Players.Count < MIN_PLAYERS)
        {
            throw new SystemException("Insufficient player count");
        }

        return new User(Players);
    }*/
}