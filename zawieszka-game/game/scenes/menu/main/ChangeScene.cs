using Godot;

namespace Zawieszka;

public partial class ChangeScene : Button
{
    [Export] private string Path { get; set; }

    public override void _Ready()
    {
        Pressed += () => GetTree().ChangeSceneToFile(Path);
    }
}