using System.Text.Json;
using Godot;
using Zawieszka.Connection;
using Zawieszka.Server;

namespace Zawieszka.scenes.menu.server;

public partial class ServerMenu : Node
{
    [Export] TextEdit Log { get; set; }
    private ServerRpcConnection Connection { get; set; }

    private Lobby Lobby { get; } = new();

    public override void _Ready()
    {
        Connection = GetNode<ServerRpcConnection>("/root/RpcConnection");
        Connection.ConnectionRegistered += (id, username) => Log.Text = $"Peer{id}:{username} registered";
        Connection.TakeSeat += OnTakeSeat;
        Connection.PlayerDisconnected += OnPlayerDisconnected;
        Connection.PeerConnected +=
            peerId => Connection.Client_UpdateLobby(peerId, JsonSerializer.Serialize(Lobby.Users));
    }

    public void _on_turn_on_button_up()
    {
        Connection.StartServer();
    }

    public void _on_display_lobby_button_up()
    {
        Log.Text += "Lobby:\n";
        foreach (var (i, user) in Lobby.Users.Select((val, n) => (n, val)))
        {
            if (user is not null)
            {
                Log.Text += $"Player {i} - {user.PeerId} - {user.Username}\n";
            }
            else
            {
                Log.Text += $"Player {i} - Empty\n";
            }
        }
    }

    public void OnTakeSeat(int seat, int peerId, string username)
    {
        var success = Lobby.TakeSeat(seat, new User { PeerId = peerId, Username = username });

        if (success)
        {
            Log.Text += $"{peerId} is now {username}\n";
            Connection.Client_UpdateLobby(JsonSerializer.Serialize(Lobby.Users));
        }
        else
        {
            Connection.Client_DisplayMessage($"{username} failed to take a seat");
        }
    }

    public void OnPlayerDisconnected(int peerId)
    {
        if (Lobby.EmptySeat(peerId))
        {
            Log.Text += $"user {peerId} removed from lobby\n";
            Connection.Client_UpdateLobby(JsonSerializer.Serialize(Lobby.Users));
        }
    }
}