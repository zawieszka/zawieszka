using Godot;
using Zawieszka.Connection;
using Zawieszka.Server;

namespace Zawieszka.scenes.menu.server;

public partial class ServerMenu : Node
{
    [Export] TextEdit Log { get; set; }
    private ServerRpcConnection Connection { get; set; }
    
    private Lobby Lobby { get; } = new ();
    
    public override void _Ready()
    {
        Connection = GetNode<ServerRpcConnection>("/root/RpcConnection");
        Connection.SetUsername += OnSetUsername;
        Connection.PlayerDisconnected += OnPlayerDisconnected;
    }

    public void _on_turn_on_button_up()
    {
        Connection.StartServer();
    }

    public void _on_display_lobby_button_up()
    {
        Log.Text += "Lobby:\n";
        foreach (var user in Lobby.Users)
        {
            Log.Text += $"{user.PeerId} - {user.Username}\n";
        }
    }

    public void OnSetUsername(int peerId, string username)
    {
        var success = Lobby.PutUser(peerId, username);

        if (success)
        {
            Log.Text += $"{peerId} is now {username}\n";
            Connection.Client_DisplayMessage($"{username} connected to lobby");
        }
        else
        {
            Connection.Client_DisplayMessage($"{username} failed to connect to lobby");
        }
    }
    
    public void OnPlayerDisconnected(int peerId)
    {
        if (Lobby.RemoveUser(peerId))
        {
            Log.Text += $"user {peerId} removed from lobby\n";
            Connection.Client_DisplayMessage($"{peerId} removed from lobby");
        }
    }
    
}