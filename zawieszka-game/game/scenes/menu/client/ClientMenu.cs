using System.Text.Json;
using Godot;
using Zawieszka.Connection;
using Zawieszka.Scenes.Lobby;
using Zawieszka.Server;

namespace Zawieszka.scenes.menu.client;

public partial class ClientMenu : Node
{
    [Export] TextEdit Log { get; set; }
    [Export] private SeatsList Seats { get; set; } 
    [Export] private Control LoadingPanel {get;set;}
    [Export] private Control LobbyPanel {get;set;}
    private ClientRpcConnection Connection { get; set; }
    public override void _Ready()
    {
        LoadingPanel.Show();
        LobbyPanel.Hide();
        
        Connection = GetNode<ClientRpcConnection>("/root/RpcConnection");
        Connection.DisplayMessage += OnDisplayMessage;
        Connection.DisplayNotification += OnNotifyMessage;
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
        
        Connection.UpdateLobby += OnUpdateLobby;

        Seats.RequestTakeSeat += seat => Connection.Server_TakeSeat(seat);
        
        Connection.TryConnectToServer();
    }

    private void _on_connect_to_lobby_button_up()
    {
        Connection.TryConnectToServer();
    }

    private void OnUpdateLobby(string lobby)
    {
        var users = JsonSerializer.Deserialize<List<User>>(lobby);
        for (var i = 0; i < 6; i++)
        {
            Seats.SetSeat(i, users[i] != null ? users[i].Username : null);
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