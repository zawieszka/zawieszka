using Godot;

namespace Zawieszka;

public partial class MainMenu : Node
{
    public override void _Ready()
    {
    }

    public override void _Process(double delta)
    {
    }

    public void _on_server_button_down()
    {
        GetTree().ChangeSceneToFile("scenes/ServerMenu.tscn");
    }

    public void _on_button_button_down()
    {
        GetTree().ChangeSceneToFile("scenes/ClientMenu.tscn");
    }
    
}