using Godot;

namespace Zawieszka.scenes;

public partial class ChangeScene : Button
{
    [Export] private string Path { get; set; } = null!;

    public override void _Ready()
    {
        Pressed += () => GetTree().ChangeSceneToFile(Path);
    }
}