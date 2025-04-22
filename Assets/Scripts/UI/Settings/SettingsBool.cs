public class SettingsBool : Settings
{
    public ToggleSwitch BoolToggle;

    protected virtual void Start()
    {
        BoolToggle.ToggleByGroupManager(GameSettings.GetSettingBool(SettingsOption));
    }

    public override void HandleSettings()
    {
        GameSettings.SetSettingsBool(BoolToggle.CurrentValue, SettingsOption);

        base.HandleSettings();
    }
}
