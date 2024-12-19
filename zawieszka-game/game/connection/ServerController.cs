namespace Zawieszka.Connection;

using Godot;

public partial class ServerController : Node
{
    [Export] private TextEdit Log { get; set; }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        Multiplayer.PeerConnected += OnPlayerConnected;
        Multiplayer.PeerDisconnected += OnPlayerDisconnected;
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
    }

    void _on_start_server_button_down()
    {
        /*if (Multiplayer.MultiplayerPeer is not null)
        {
            Log.Text += "Server already started\n";
            return;
        }*/

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

    [Rpc(MultiplayerApi.RpcMode.AnyPeer, CallLocal = false, TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
    public void KokMessage()
    {
        Log.Text += $"Server Kok\n";
    }

    #endregion
}