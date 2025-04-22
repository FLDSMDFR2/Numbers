using System;

[Serializable]
public class GameDataSave
{
    public long PuzzlesCompleted = 0;
    public long GOTDCompleted = 0;
    public DateTime LastGOTDCompleted = DateTime.MinValue;
}
