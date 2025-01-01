using Godot;

namespace Zawieszka.scenes.game;

public partial class Game : Control
{
	[Export] private VBoxContainer _playerInfoContainer;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_playerInfoContainer.AddChild(PlayerInfo.PlayerInfoFromName("Bob"));
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}