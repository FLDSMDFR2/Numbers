using System;

public class GameData
{
    public static long PuzzlesCompleted = 0;
    public static long GOTDCompleted = 0;
    public static DateTime LastGOTDCompleted = DateTime.MinValue;

    public static void TryAddGOTDCompleted(int ToAdd)
    { 
        if (LastGOTDCompleted.Date == DateTime.Now.Date) return;

        LastGOTDCompleted = DateTime.Now;
        GOTDCompleted += ToAdd;
        GameEventSystem.UI_OnUpdate();
        GameEventSystem.Data_OnUpdate();
    }

    public static void AddPuzzlesCompleted(int ToAdd)
    {
        PuzzlesCompleted += ToAdd;
        GameEventSystem.UI_OnUpdate();
        GameEventSystem.Data_OnUpdate();
    }
}
