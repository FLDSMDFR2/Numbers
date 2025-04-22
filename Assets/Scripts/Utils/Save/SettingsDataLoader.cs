public class SettingsDataLoader : SaveData
{
    protected GameSettingsSave data;

    protected override void Awake()
    {
        LoadSaveData();

        GameEventSystem.UI_SettingsValueChange += GameEventSystem_UI_SettingsValueChange;
    }

    private void GameEventSystem_UI_SettingsValueChange()
    {
        Save();
    }

    #region "Save / Load"
    protected override void LoadSaveData()
    {
        data = SerializeManager.Load<GameSettingsSave>(this);
        if (data == null)
        {
            TraceManager.WriteTrace(TraceChannel.Main, TraceType.error, $"DataLoader {SaveName} Failed to load");
            data = new GameSettingsSave();
        }

        GameSettings.Music = data.Music;
        GameSettings.Sound = data.Sound;

        base.LoadSaveData();
    }

    public override object GetSaveData()
    {
        data.Music = GameSettings.Music;
        data.Sound = GameSettings.Sound;

        return data;
    }
    #endregion
}
