using Godot;
using Zawieszka.Connection;

namespace Zawieszka.scenes.menu.client;

public partial class ClientMenu : Node
{
    [Export] TextEdit Username { get; set; }
    [Export] TextEdit Log { get; set; }
    private ClientRpcConnection Connection { get; set; }
    public override void _Ready()
    {
        Connection = GetNode<ClientRpcConnection>("/root/RpcConnection");
        Connection.DisplayMessage += OnDisplayMessage;
    }

    public void _on_connect_button_up()
    {
        Connection.TryConnectToServer();
    }

    public void _on_set_username_button_up()
    {
        Connection.Server_SetUsername(Username.Text);
    }

    private void OnDisplayMessage(string message)
    {
        Log.Text += $"{message}\n";
    }
}