using Godot;
using FileAccess = Godot.FileAccess;

namespace Zawieszka.Scenes.Settings;

public class Settings
{
    public string Username { get; init; } = "";
}

public class SettingsManager
{
    private const string Path = "user://zawieszka-settings.ini";
    public static SettingsManager Instance { get; } = new ();
    public Settings Settings { get; set; } = new ();

    private SettingsManager()
    {
        Load();
    }
    
    private void Load()
    {
        try
        {
            using var saveFile = FileAccess.Open(Path, FileAccess.ModeFlags.Read);
            var username = saveFile.GetLine();
            Settings = new Settings() { Username = username };
        }
        catch (Exception e)
        {
            GD.Print("Invalid Settings file");
        }
    }

    public void Save()
    {
        using var saveFile = FileAccess.Open(Path, FileAccess.ModeFlags.Write);
        saveFile.StoreLine(Settings.Username);
    }
}