namespace Zawieszka.Connection;

using Godot;

public partial class ClientRpcConnection : Node, IRpcConnection
{
    private const int ServerId = 1;
    public bool ConnectedToServer { get; private set; } = false;

    [Signal]
    public delegate void ServerDisconnectedEventHandler();

    [Signal]
    public delegate void CustomMessageEventHandler(int peerId, string message);

    [Signal]
    public delegate void DisplayNotificationEventHandler(string message);

    [Signal]
    public delegate void DisplayMessageEventHandler(string message);

    [Signal]
    public delegate void NextTurnEventHandler(string username);

    public override void _Ready()
    {
        Multiplayer.ConnectedToServer += OnConnectOk;
        Multiplayer.ConnectionFailed += OnConnectionFail;
        Multiplayer.ServerDisconnected += OnServerDisconnected;
    }

    public override void _Process(double delta) { }

    private void OnConnectOk()
    {
        ConnectedToServer = true;
    }

    private void OnConnectionFail()
    {
        ConnectedToServer = false;
        Multiplayer.MultiplayerPeer = null;
    }

    private void OnServerDisconnected()
    {
        Multiplayer.MultiplayerPeer = null;
        ConnectedToServer = false;
        EmitSignal(SignalName.ServerDisconnected);
    }

    public void TryConnectToServer()
    {
        if (ConnectedToServer)
        {
            GD.PrintErr("Client has already connected to a server");
            return;
        }
        
        var peer = new ENetMultiplayerPeer();
        var error = peer.CreateClient(Globals.DefaultServerIp, Globals.Port);

        if (error != Error.Ok)
        {
            return;
        }

        ConnectedToServer = true;
        Multiplayer.MultiplayerPeer = peer;
    }

    [Rpc(MultiplayerApi.RpcMode.Disabled)]
    public void Server_SetUsername(string username)
    {
        RpcId(ServerId, MethodName.Server_SetUsername, username);
    }

    [Rpc(MultiplayerApi.RpcMode.Disabled)]
    public void Server_EndTurn()
    {
        RpcId(ServerId, MethodName.Server_EndTurn);
    }

    [Rpc]
    public void Client_DisplayNotification(string message)
    {
        EmitSignal(SignalName.DisplayNotification, message);
    }

    [Rpc]
    public void Client_DisplayMessage(string message)
    {
        EmitSignal(SignalName.DisplayMessage, message);
    }

    [Rpc]
    public void Client_NextTurn(string username)
    {
        EmitSignal(SignalName.NextTurn, username);
    }
}