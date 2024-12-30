using Godot;

namespace Zawieszka.Scenes.Settings;

public partial class SettingsPage : Node
{
    [Export] private LineEdit UsernameDisplay { get; set; }

    public override void _Ready()
    {
        base._Ready();
        var settings = SettingsManager.Instance.Settings;
        UsernameDisplay.Text = settings.Username;
    }

    private void _on_save_button_up()
    {
        SettingsManager.Instance.Settings = new Settings { Username = UsernameDisplay.Text.Trim() };
        SettingsManager.Instance.Save();
    }

    private void _on_back_button_up()
    {
        GetTree().ChangeSceneToFile("res://scenes/menu/main/main_menu.tscn");
    }
}