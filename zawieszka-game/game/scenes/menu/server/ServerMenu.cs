using Godot;
using Zawieszka.Connection;

namespace Zawieszka.scenes.menu.server;

public partial class ServerMenu : Node
{
    [Export] TextEdit Log { get; set; }
    private ServerRpcConnection Connection { get; set; }
    public override void _Ready()
    {
        Connection = GetNode<ServerRpcConnection>("/root/RpcConnection");
        Connection.SetUsername += OnSetUsername;
    }

    public void _on_turn_on_button_up()
    {
        Connection.StartServer();
    }

    public void OnSetUsername(int id, string username)
    {
        Log.Text += $"{username}: {id}\n";
        Connection.Client_DisplayMessage($"User {id} is {username}");
    }
    
}