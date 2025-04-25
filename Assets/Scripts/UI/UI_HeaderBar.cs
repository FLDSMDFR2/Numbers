using System;
using TMPro;
using UnityEngine;

public class UI_HeaderBar : MonoBehaviour
{
    public TextMeshProUGUI LevelText;
    public TextMeshProUGUI PuzzleCompleteText;
    public TextMeshProUGUI SeedText;

    protected MapType type;

    protected virtual void Awake()
    {
        GameEventSystem.Game_Start += GameEventSystem_Game_Start;
        GameEventSystem.UI_Update += GameEventSystem_UI_Update;
    }

    protected virtual void GameEventSystem_Game_Start(MapType mapType)
    {
        type = mapType;
        GameEventSystem_UI_Update();
    }

    protected virtual void GameEventSystem_UI_Update()
    {
        LevelText.enabled = false;
        SeedText.enabled = false;
        PuzzleCompleteText.enabled = false;

        //if (type != MapType.Challenge && type != MapType.GOTD && type != MapType.Custom) return;

        PuzzleCompleteText.text = "";
        LevelText.enabled = false;
        SeedText.enabled = false;
        PuzzleCompleteText.enabled = true;

        if (type == MapType.Challenge)
        {
            PuzzleCompleteText.text = PuzzlesPerLevel.GetLevelByCompletedPuzzles(GameData.PuzzlesCompleted) + " - " + PuzzlesPerLevel.GetPuzzlesCompletedWithinLevel(GameData.PuzzlesCompleted);
        }
        else if (type == MapType.GOTD && GameData.LastGOTDCompleted.Date == DateTime.Now.Date)
        {
            PuzzleCompleteText.text = "COMPLETE";
        }
        else if (type == MapType.Custom)
        {
            if (CustomValues.Level > 0 ) PuzzleCompleteText.text = CustomValues.Level + " - " + "\u221E";
            else PuzzleCompleteText.text = "CUSTOM";
        }
        else if (type == MapType.Basic)
        {
            PuzzleCompleteText.text = "\u221E";
        }
    }
}
