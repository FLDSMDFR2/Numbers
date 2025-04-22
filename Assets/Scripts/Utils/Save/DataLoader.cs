
public class DataLoader : SaveData
{
    protected GameDataSave data;

    protected override void Awake()
    {
        LoadSaveData();

        GameEventSystem.Data_Update += GameEventSystem_Data_Update;
    }

    protected virtual void GameEventSystem_Data_Update()
    {
        Save();
    }

    #region "Save / Load"
    protected override void LoadSaveData()
    {
        data = SerializeManager.Load<GameDataSave>(this);
        if (data == null)
        {
            TraceManager.WriteTrace(TraceChannel.Main, TraceType.error, $"DataLoader {SaveName} Failed to load");
            data = new GameDataSave();
        }

        GameData.PuzzlesCompleted = data.PuzzlesCompleted;
        GameData.GOTDCompleted = data.GOTDCompleted;
        GameData.LastGOTDCompleted = data.LastGOTDCompleted;

        base.LoadSaveData();
    }

    public override object GetSaveData()
    {
        data.PuzzlesCompleted = GameData.PuzzlesCompleted;
        data.GOTDCompleted = GameData.GOTDCompleted;
        data.LastGOTDCompleted = GameData.LastGOTDCompleted;

        return data;
    }
    #endregion
}
