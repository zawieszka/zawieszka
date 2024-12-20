namespace Zawieszka.Connection;

using Godot;

public partial class ServerRpcConnection : Node, IRpcConnection
{
    [Export] private TextEdit Log { get; set; }
    
    public override void _Ready()
    {
        Multiplayer.PeerConnected += OnPlayerConnected;
        Multiplayer.PeerDisconnected += OnPlayerDisconnected;
    }
    
    public override void _Process(double delta) { }

    void _on_start_server_button_down()
    {

        CreateGame();
    }

    private void CreateGame()
    {
        var peer = new ENetMultiplayerPeer();
        var error = peer.CreateServer(Globals.Port, Globals.MaxConnections);

        if (error != Error.Ok)
        {
            Log.Text += "Server creation failure\n";
            return;
        }

        Multiplayer.MultiplayerPeer = peer;

        Log.Text += "Server created successfully\n";
    }

    private void OnPlayerConnected(long id)
    {
        Log.Text += $"Server: player {id} connected\n";
    }

    private void OnPlayerDisconnected(long id)
    {
        Log.Text += $"Server: player {id} connected\n";
    }


    #region Demo

    [Rpc(MultiplayerApi.RpcMode.AnyPeer, CallLocal = false, TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
    private void GetCustomMessage(int id, string message)
    {
        Log.Text += $"Server: got {message} from {id}\n";
        Rpc(MethodName.GetCustomMessage, id, message);
    }

    #endregion

    [Rpc(MultiplayerApi.RpcMode.AnyPeer)]
    public void Server_SetUsername(string username) { }

    [Rpc(MultiplayerApi.RpcMode.AnyPeer)]
    public void Server_EndTurn() { }

    [Rpc(MultiplayerApi.RpcMode.Disabled)]
    public void Client_DisplayNotification(string message) { }

    [Rpc(MultiplayerApi.RpcMode.Disabled)]
    public void Client_DisplayMessage(string message) { }

    [Rpc(MultiplayerApi.RpcMode.Disabled)]
    public void Client_NextTurn(string username) { }
}