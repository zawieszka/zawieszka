namespace Zawieszka.Connection;

using Godot;

public partial class ServerRpcConnection : Node, IRpcConnection
{
    public bool ServerRunning { get; private set; } = false;
    [Signal]
    public delegate void EndTurnEventHandler(int id);

    [Signal]
    public delegate void SetUsernameEventHandler(int id, string username);
    
    public override void _Ready()
    {
        Multiplayer.PeerConnected += OnPlayerConnected;
        Multiplayer.PeerDisconnected += OnPlayerDisconnected;
    }

    public override void _Process(double delta) { }

    public void StartServer()
    {
        if (ServerRunning)
        {
            GD.PrintErr("Server has already started");
            return;
        }
        var peer = new ENetMultiplayerPeer();
        var error = peer.CreateServer(Globals.Port, Globals.MaxConnections);

        if (error != Error.Ok)
        {
            return;
        }

        ServerRunning = true;
        Multiplayer.MultiplayerPeer = peer;
    }

    private void OnPlayerConnected(long id)
    {
        GD.Print($"Peer {id} connected");
    }

    private void OnPlayerDisconnected(long id)
    {
        GD.Print($"Peer {id} disconnected");
    }

    [Rpc(MultiplayerApi.RpcMode.AnyPeer)]
    public void Server_SetUsername(string username)
    {
        EmitSignal(SignalName.SetUsername, Multiplayer.GetRemoteSenderId(), username);
    }

    [Rpc(MultiplayerApi.RpcMode.AnyPeer)]
    public void Server_EndTurn()
    {
        EmitSignal(SignalName.EndTurn, Multiplayer.GetRemoteSenderId());
    }

    [Rpc(MultiplayerApi.RpcMode.Disabled, CallLocal = false)]
    public void Client_DisplayNotification(string message)
    {
        Rpc(MethodName.Client_DisplayNotification, message);
    }

    [Rpc(MultiplayerApi.RpcMode.Disabled, CallLocal = false)]
    public void Client_DisplayMessage(string message)
    {
        Rpc(MethodName.Client_DisplayMessage, message);
    }

    [Rpc(MultiplayerApi.RpcMode.Disabled, CallLocal = false)]
    public void Client_NextTurn(string username)
    {
        Rpc(MethodName.Client_NextTurn, username);
    }
    
    
}