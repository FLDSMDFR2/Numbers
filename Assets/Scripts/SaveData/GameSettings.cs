public enum SettingsOption
{ 
    None= 0,
    Music,
    Sound
}
public class GameSettings
{
    public static bool Music;
    public static bool Sound;


    public static void SetSettingsBool(bool value, SettingsOption settingsOption)
    {
        if (settingsOption == SettingsOption.Music) Music = value;
        else if (settingsOption == SettingsOption.Sound) Sound = value;
    }

    public static bool GetSettingBool(SettingsOption settingsOption)
    {
        if (settingsOption == SettingsOption.Music) return Music;
        else if (settingsOption == SettingsOption.Sound) return Sound;

        return false;
    }
}
