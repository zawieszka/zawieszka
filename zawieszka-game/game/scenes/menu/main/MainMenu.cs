using Godot;
using System;
using Zawieszka.Connection;
using Zawieszka.Scenes.Settings;

public partial class MainMenu : Node
{
	[Export] private Label HelloDisplay { get; set; } = null!;
	[Export] private Button ClientButton { get; set; } = null!;
	
	private SettingsManager SettingsManager { get; set; } = SettingsManager.Instance;
	public override void _Ready()
	{
		var serverRpc = new ServerRpcConnection{Name = "RpcConnection"};
		var username = SettingsManager.Settings.Username;
		if (username.Length != 0)
		{
			HelloDisplay.Text = $"Hello {username}";
		}
		else
		{
			HelloDisplay.Text = "Set up your user profile in Settings before adventure!";
			ClientButton.Disabled = true;
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void _on_client_scene_button_up()
	{
		var clientRpc = new ClientRpcConnection{Name = "RpcConnection"};
		GetTree().GetRoot().AddChild(clientRpc);
		GetTree().ChangeSceneToFile("res://scenes/menu/client/client_menu.tscn");
	}

	private void _on_server_scene_button_up()
	{
		var serverRpc = new ServerRpcConnection{Name = "RpcConnection"};
		GetTree().GetRoot().AddChild(serverRpc);
		GetTree().ChangeSceneToFile("res://scenes/menu/server/server_menu.tscn");
	}

	private void _on_settings_scene_button_up()
	{
		GetTree().ChangeSceneToFile("res://scenes/menu/settings/settings_page.tscn");
	}
}
