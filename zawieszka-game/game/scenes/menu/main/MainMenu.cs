using Godot;
using System;
using Zawieszka.Connection;

public partial class MainMenu : Node
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void _on_client_scene_button_up()
	{
		var clientRpc = new ClientRpcConnection{Name = "RpcConnection"};
		GetTree().GetRoot().AddChild(clientRpc);
		GetTree().ChangeSceneToFile("res://scenes/menu/client/client_menu.tscn");
	}

	public void _on_server_scene_button_up()
	{
		var serverRpc = new ServerRpcConnection{Name = "RpcConnection"};
		GetTree().GetRoot().AddChild(serverRpc);
		GetTree().ChangeSceneToFile("res://scenes/menu/server/server_menu.tscn");
	}
}
