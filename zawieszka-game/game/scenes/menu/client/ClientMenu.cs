using System.Text.Json;
using Godot;
using Zawieszka.Connection;
using Zawieszka.Scenes.Lobby;
using Zawieszka.Server;

namespace Zawieszka.Scenes.Menu.Client;

public partial class ClientMenu : Node
{
    [Export] private TextEdit Log { get; set; } = null!;
    [Export] private SeatsList Seats { get; set; } = null!;
    [Export] private Control LoadingPanel {get;set;} = null!;
    [Export] private Control LobbyPanel {get;set;} = null!;
    private ClientRpcConnection Connection { get; set; } = null!;
    public override void _Ready()
    {
        LoadingPanel.Show();
        LobbyPanel.Hide();
        
        Connection = GetNode<ClientRpcConnection>("/root/RpcConnection");
        Connection.NewMessage += OnDisplayMessage;
        Connection.NewNotification += OnNotifyMessage;
        Connection.ServerDisconnected += () =>
        {
            LoadingPanel.Show();
            LobbyPanel.Hide();
        };
        Connection.ServerConnected += (username, isMe) =>
        {
            if (!isMe)
            {
                Log.Text = $"{username} connected to lobby \n";
            }
            else
            {
                LobbyPanel.Show();
                LoadingPanel.Hide();
            }
        };
        
        Connection.LobbyUpdated += OnUpdateLobby;

        Seats.RequestTakeSeat += seat => Connection.Server_TakeSeat(seat);
        
        Connection.TryConnectToServer();
    }

    private void _on_connect_to_lobby_button_up()
    {
        Connection.TryConnectToServer();
    }

    private void OnUpdateLobby(string lobby)
    {
        var users = JsonSerializer.Deserialize<List<User?>>(lobby)!;
        for (var i = 0; i < Server.Lobby.MaxPlayers; i++)
        {
            Seats.SetSeat(i, users[i]?.Username);
        }
    }

    private void OnDisplayMessage(string message)
    {
        Log.Text += $"{message}\n";
    }
    
    private void OnNotifyMessage(string message)
    {
        GD.Print(message);
    }
}