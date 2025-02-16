using System.Text.Json;
using Godot;
using Zawieszka.Connection;
using Zawieszka.Server;

namespace Zawieszka.scenes.menu.server;

public partial class ServerMenu : Node
{
    [Export] private TextEdit Log { get; set; } = null!;
    private ServerRpcConnection Connection { get; set; } = null!;

    private Lobby Lobby { get; } = new();

    public override void _Ready()
    {
        Connection = GetNode<ServerRpcConnection>("/root/RpcConnection");

        Connection.PeerConnected +=
            peerId => Connection.Client_UpdateLobby(peerId, JsonSerializer.Serialize(Lobby.Users));
        Connection.PeerDisconnected += OnPeerDisconnected;
        Connection.ConnectionRegistered += (id, username) => Log.Text += $"Peer{id}:{username} registered\n";
        Connection.TakeSeatRequested += OnTakeSeatRequested;
        Connection.EndTurnRequested += _ => throw new NotImplementedException();
        Connection.StartGameRequested += OnStartGameRequested;
    }

    private void _on_turn_on_button_up()
    {
        Connection.StartServer();
    }

    private void _on_display_lobby_button_up()
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

    private void OnPeerDisconnected(int peerId)
    {
        if (!Lobby.EmptySeat(peerId)) return;

        Log.Text += $"user {peerId} removed from lobby\n";
        Connection.Client_UpdateLobby(JsonSerializer.Serialize(Lobby.Users));
    }

    private void OnTakeSeatRequested(int seat, int peerId, string username)
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
    
    private void OnStartGameRequested(int peerId)
    {
        Connection.Client_GameStarted();
    }
}