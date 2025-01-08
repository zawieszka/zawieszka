using Godot;

namespace Zawieszka.scenes.game;

public partial class PlayerInfo : TextureRect
{
    private static readonly PackedScene PlayerInfoScene =
        ResourceLoader.Load<PackedScene>("res://scenes/game/PlayerInfo.tscn");

    [Export] private Label PlayerName { get; set; } = null!;

    public static PlayerInfo FromName(string playerName)
    {
        var playerInfo = PlayerInfoScene.Instantiate<PlayerInfo>();
        playerInfo.PlayerName.Text = playerName;

        return playerInfo;
    }
}