using System.Text.Json;
using Zawieszka.Scenes.Settings;
using Zawieszka.Server;

namespace Zawieszka.Connection;

using Godot;

public partial class ClientRpcConnection : Node, IRpcConnection
{
    private const int ServerId = 1;
    public bool ConnectedToServer => State == ConnectionState.Connected;
    private ConnectionState State { get; set; } = ConnectionState.NotConnected;

    [Signal]
    public delegate void ServerConnectedEventHandler(string username, bool isMe);
    [Signal]
    public delegate void ServerDisconnectedEventHandler();

    [Signal]
    public delegate void CustomMessageEventHandler(int peerId, string message);

    [Signal]
    public delegate void LobbyUpdatedEventHandler(string lobby);

    [Signal]
    public delegate void NewNotificationEventHandler(string message);

    [Signal]
    public delegate void NewMessageEventHandler(string message);

    [Signal]
    public delegate void NextTurnEventHandler(string username);

    [Signal]
    public delegate void GameStartedEventHandler();

    public override void _Ready()
    {
        Multiplayer.ConnectedToServer += OnConnected;
        Multiplayer.ConnectionFailed += OnConnectionFailed;
        Multiplayer.ServerDisconnected += OnServerDisconnected;
    }

    public override void _Process(double delta) { }

    private void OnConnected()
    {
        State = ConnectionState.PeerConnected;
        Server_RegisterConnection(SettingsManager.Instance.Settings.Username);
    }

    private void OnConnectionFailed()
    {
        State = ConnectionState.NotConnected;
        Multiplayer.MultiplayerPeer = null;
        EmitSignal(SignalName.ServerDisconnected);
    }

    private void OnServerDisconnected()
    {
        Multiplayer.MultiplayerPeer = null;
        State = ConnectionState.NotConnected;
        EmitSignal(SignalName.ServerDisconnected);
    }

    public void TryConnectToServer()
    {
        switch (State)
        {
            case ConnectionState.NotConnected:
                var peer = new ENetMultiplayerPeer();
                var error = peer.CreateClient(Globals.DefaultServerIp, Globals.Port);

                if (error != Error.Ok)
                {
                    return;
                }

                State = ConnectionState.PeerConnected;
                Multiplayer.MultiplayerPeer = peer;
                return;
            case ConnectionState.PeerConnected:
                Server_RegisterConnection(SettingsManager.Instance.Settings.Username);
                return;
            case ConnectionState.Connected:
                GD.PrintErr("Client is already connected to the server");
                return;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    [Rpc(MultiplayerApi.RpcMode.Disabled)]
    public void Server_RegisterConnection(string username)
    {
        RpcId(ServerId, MethodName.Server_RegisterConnection, username);
    }

    [Rpc(MultiplayerApi.RpcMode.Disabled)]
    public void Server_TakeSeat(int seat)
    {
        RpcId(ServerId, MethodName.Server_TakeSeat, seat);
    }

    [Rpc(MultiplayerApi.RpcMode.Disabled)]
    public void Server_EndTurn()
    {
        RpcId(ServerId, MethodName.Server_EndTurn);
    }

    [Rpc(MultiplayerApi.RpcMode.Disabled)]
    public void Server_StartGame()
    {
        RpcId(ServerId, MethodName.Server_StartGame);
    }

    [Rpc]
    public void Client_RegisteredConnection(int peerId, string username)
    {
        if (peerId == Multiplayer.MultiplayerPeer.GetUniqueId())
        {
            State = ConnectionState.Connected;
            EmitSignal(SignalName.ServerConnected, username, true);
        }
        else
        {
            EmitSignal(SignalName.ServerConnected, username, false);
        }
    }

    [Rpc]
    public void Client_UpdateLobby(string lobby)
    {
        EmitSignal(SignalName.LobbyUpdated, lobby);
    }

    [Rpc]
    public void Client_DisplayNotification(string message)
    {
        EmitSignal(SignalName.NewNotification, message);
    }

    [Rpc]
    public void Client_DisplayMessage(string message)
    {
        EmitSignal(SignalName.NewMessage, message);
    }

    [Rpc]
    public void Client_NextTurn(string username)
    {
        EmitSignal(SignalName.NextTurn, username);
    }

    [Rpc]
    public void Client_GameStarted()
    {
        EmitSignal(SignalName.GameStarted);
    }

    private enum ConnectionState
    {
        NotConnected,
        PeerConnected,
        Connected
    }
}