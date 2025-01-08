using System.Threading.Tasks.Dataflow;
using Godot;

namespace Zawieszka.scenes.game;

public partial class Game : Control
{
    private static readonly PackedScene GameScene = ResourceLoader.Load<PackedScene>("res://scenes/game/Game.tscn");

    [Export] private VBoxContainer PlayerInfoContainer { get; set; } = null!;

    public static Game FromPlayerNames(List<string> playerNames)
    {
        var game = GameScene.Instantiate<Game>();
        playerNames.ForEach(playerName => game.PlayerInfoContainer.AddChild(PlayerInfo.FromName(playerName)));

        return game;
    }

    public override void _Ready()
    {
        PlayerInfoContainer.AddChild(PlayerInfo.FromName("john"));
        PlayerInfoContainer.AddChild(PlayerInfo.FromName("jane"));
    }
}