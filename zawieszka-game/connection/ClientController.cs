namespace Zawieszka.Connection;

using Godot;
using System;

public partial class ClientController : Node
{
    [Export] 
    private TextEdit Log { get; set; }
    
    [Signal]
    public delegate void PlayerConnectedEventHandler(long peerId);

    [Signal]
    public delegate void PlayerDisconnectedEventHandler(long peerId);

    [Signal]
    public delegate void ServerDisconnectedEventHandler();
    
    [Signal]
    public delegate void CustomMessageEventHandler(int peerId, string message);

    public override void _Ready()
    {
        Multiplayer.ConnectedToServer += OnConnectOk;
        Multiplayer.ConnectionFailed += OnConnectionFail;
        Multiplayer.ServerDisconnected += OnServerDisconnected;
        Multiplayer.PeerConnected += OnPlayerConnected;
        Multiplayer.PeerDisconnected += OnPlayerDisconnected;
    }

    public override void _Process(double delta)
    {
    }
    
    public void _on_connect_server_button_down()
    {
        /*if (Multiplayer.MultiplayerPeer is not null)
        {
            Log.Text += "Already connected!\n";
            return;
        }*/
        
        ConnectToServer();
    }

    public void _on_send_message_button_down()
    {
        var id = Multiplayer.GetMultiplayerPeer().GetUniqueId();
        Rpc(MethodName.KokMessage, id, "spoget");
    }

    private void OnPlayerConnected(long id)
    {
        Log.Text += $"Client: player {id} connected";
        EmitSignal(SignalName.PlayerConnected, id);
    }

    private void OnPlayerDisconnected(long id)
    {
        Log.Text += $"Client: player {id} connected";
        EmitSignal(SignalName.PlayerDisconnected, id);
    }

    private void OnConnectOk()
    {
        Log.Text += "Client: connected successfully\n";
        var id = Multiplayer.GetUniqueId();
        EmitSignal(SignalName.PlayerConnected, id);
    }

    private void OnConnectionFail()
    {
        Log.Text += "Client: connection failure\n";
        Multiplayer.MultiplayerPeer = null;
    }

    private void OnServerDisconnected()
    {
        Log.Text += "Client: server disconnected\n";
        Multiplayer.MultiplayerPeer = null;
        EmitSignal(SignalName.ServerDisconnected);
    }
    
    private void ConnectToServer()
    {
        var peer = new ENetMultiplayerPeer();
        var error = peer.CreateClient(Globals.DefaultServerIp, Globals.Port);

        if (error != Error.Ok)
        {
            Log.Text += "Connecting to server unsuccessful\n";
            return;
        }
        
        Multiplayer.MultiplayerPeer = peer;
    }
    
    [Rpc(MultiplayerApi.RpcMode.AnyPeer, CallLocal = false, TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
    private void GetCustomMessage(int id, string message)
    {
        Log.Text += $"Client: got {message} from {id}";
        EmitSignal(SignalName.CustomMessage, id, message);
    }
    
    [Rpc(MultiplayerApi.RpcMode.AnyPeer, CallLocal = false, TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
    private void KokMessage(int id, string message)
    {
        Log.Text += $"Client: Kok {message} from {id}";
    }
}