namespace Zawieszka.Connection;

using Godot;

public partial class ServerRpcConnection : Node, IRpcConnection
{
    public bool ServerRunning { get; private set; } = false;
    private Dictionary<int, string> RegisteredUsers { get; } = new();

    [Signal]
    public delegate void EndTurnEventHandler(int peerId);

    [Signal]
    public delegate void ConnectionRegisteredEventHandler(int peerId, string username);

    [Signal]
    public delegate void PeerConnectedEventHandler(int peerId);

    [Signal]
    public delegate void PlayerDisconnectedEventHandler(int peerId);
    
    [Signal]
    public delegate void TakeSeatEventHandler(int seat, int peerId, string username);

    public override void _Ready()
    {
        Multiplayer.PeerConnected += OnPeerConnected;
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

    private void OnPeerConnected(long id)
    {
        // https://github.com/godotengine/godot/issues/75396
        if (id > int.MaxValue)
        {
            GD.PrintErr("PeerID was trimmed!!!");
        }
        GD.Print($"Peer {(int)id} connected");
        EmitSignal(SignalName.PeerConnected, (int)id);
    }

    private void OnPlayerDisconnected(long id)
    {
        GD.Print($"Peer {(int)id} disconnected");
        RegisteredUsers.Remove((int)id);
        EmitSignal(SignalName.PlayerDisconnected, (int)id);
    }

    [Rpc(MultiplayerApi.RpcMode.AnyPeer)]
    public void Server_RegisterConnection(string username)
    {
        var peerId = Multiplayer.GetRemoteSenderId();
        RegisteredUsers[Multiplayer.GetRemoteSenderId()] = username;
        EmitSignal(SignalName.ConnectionRegistered, peerId, username);
        
        Client_RegisteredConnection(peerId, username);
    }

    [Rpc(MultiplayerApi.RpcMode.AnyPeer)]
    public void Server_TakeSeat(int seat)
    {
        var peerId = Multiplayer.GetRemoteSenderId();
        if (RegisteredUsers.TryGetValue(peerId, out var username))
        {
            EmitSignal(SignalName.TakeSeat, seat, peerId, username);
        }
        else
        {
            RpcId(peerId, MethodName.Client_DisplayNotification, "Connection not registered");
        }
    }

    [Rpc(MultiplayerApi.RpcMode.AnyPeer)]
    public void Server_EndTurn()
    {
        EmitSignal(SignalName.EndTurn, Multiplayer.GetRemoteSenderId());
    }

    [Rpc(MultiplayerApi.RpcMode.Disabled, CallLocal = false)]
    public void Client_RegisteredConnection(int peerId, string username)
    {
        Rpc(MethodName.Client_RegisteredConnection, peerId, username);
    }

    [Rpc(MultiplayerApi.RpcMode.Disabled, CallLocal = false)]
    public void Client_UpdateLobby(string lobby)
    {
        Rpc(MethodName.Client_UpdateLobby, lobby);
    }

    public void Client_UpdateLobby(int peerId, string lobby)
    {
        RpcId(peerId, MethodName.Client_UpdateLobby, lobby);
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