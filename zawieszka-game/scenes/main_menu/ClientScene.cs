using Godot;
using System;

public partial class ClientScene : Button
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		this.Pressed += () => GetTree().ChangeSceneToFile("res://scenes/client_menu.tscn");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
