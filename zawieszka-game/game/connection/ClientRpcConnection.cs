using ZawieszkaCore;

namespace Zawieszka.Connection;

using Godot;

public partial class ClientRpcConnection : Node, IRpcConnection
{
    [Export] private TextEdit Log { get; set; }

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

    public override void _Process(double delta) { }

    public void _on_connect_server_button_down()
    {
        ConnectToServer();
    }

    public void _on_send_message_button_down()
    {
        var id = Multiplayer.GetMultiplayerPeer().GetUniqueId();
        RpcId(1, MethodName.GetCustomMessage, id, Test.TestMessage());
    }

    private void OnPlayerConnected(long id)
    {
        Log.Text += $"Client: player {id} connected\n";
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


    #region Demo

    [Rpc]
    private void GetCustomMessage(int id, string message)
    {
        Log.Text += $"Client: got {message} from {id}\n";
        EmitSignal(SignalName.CustomMessage, id, message);
    }

    [Rpc]
    private void KokMessage()
    {
        Log.Text += $"Client: KOk\n";
    }

    #endregion

    [Rpc(MultiplayerApi.RpcMode.Disabled)]
    public void Server_SetUsername(string username) { }

    [Rpc(MultiplayerApi.RpcMode.Disabled)]
    public void Server_EndTurn() { }

    [Rpc]
    public void Client_DisplayNotification(string message) { }

    [Rpc]
    public void Client_DisplayMessage(string message) { }

    [Rpc]
    public void Client_NextTurn(string username) { }
}