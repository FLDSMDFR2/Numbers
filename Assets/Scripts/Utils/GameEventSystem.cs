using UnityEngine;

public class GameEventSystem : MonoBehaviour
{
    #region UI Events
    public delegate void UIEvent();

    public static event UIEvent UI_Loaded;
    public static event UIEvent UI_Update;
    public static event UIEvent UI_ResetButtonPress;
    public static event UIEvent UI_UndoButtonPress;
    public static event UIEvent UI_CompleteButtonPress;
    public static event UIEvent UI_HomeButtonPress;

    public static event UIEvent UI_SettingsButtonPress;
    public static event UIEvent UI_SettingsCloseButtonPress;
    public static event UIEvent UI_SettingsValueChange;

    public static event UIEvent UI_CustomButtonPress;

    public static void UI_OnLoaded()
    {
        UI_Loaded?.Invoke();
    }
    public static void UI_OnUpdate()
    {
        UI_Update?.Invoke();
    }
    public static void UI_OnResetButtonPress()
    {
        UI_ResetButtonPress?.Invoke();
    }
    public static void UI_OnUndoButtonPress()
    {
        UI_UndoButtonPress?.Invoke();
    }
    public static void UI_OnCompleteButtonPress()
    {
        UI_CompleteButtonPress?.Invoke();
    }
    public static void UI_OnSettingsButtonPress()
    {
        UI_SettingsButtonPress?.Invoke();
    }
    public static void UI_OnSettingsCloseButtonPress()
    {
        UI_SettingsCloseButtonPress?.Invoke();
    }
    public static void UI_OnHomeButtonPress()
    {
        UI_HomeButtonPress?.Invoke();
    }
    public static void UI_OnSettingsValueChange()
    {
        UI_SettingsValueChange?.Invoke();
    }
    public static void UI_OnCustomButtonPress()
    {
        UI_CustomButtonPress?.Invoke();
    }
    #endregion

    #region Game Events
    public delegate void GameEvent();
    public delegate void GameStartEvent(MapType mapType);
    public static event GameEvent Game_Pause;
    public static event GameEvent Game_Resume;
    //public static event GameEvent Game_Start;
    public static event GameEvent Game_Over;
    
    public static event GameStartEvent Game_Start;


    public static void Game_OnPause()
    {
        Game_Pause?.Invoke();
    }
    public static void Game_OnResume()
    {
        Game_Resume?.Invoke();
    }
    public static void Game_OnStart(MapType mapType)
    {
        Game_Start?.Invoke(mapType);
    }
    public static void Game_OnGameOver()
    {
        Game_Over?.Invoke();
    }
    #endregion

    #region Map Events
    public delegate void MapEvent();

    public static event MapEvent Map_Complete;

    public static void Map_OnComplete()
    {
        Map_Complete?.Invoke();
    }
    #endregion

    #region Drag Events
    public delegate void DragEventStart(MapSector sector, MapTraversal.MapTraversalDirectionsIndex direction, MapTraversal.MapTraversalDirectionsIndex directionOrth);
    public delegate void DragEventStop(bool confirmed);

    public static event DragEventStart Drag_StartDrag;
    public static event DragEventStop Drag_StopDrag;

    public static void Drag_OnDragStart(MapSector sector, MapTraversal.MapTraversalDirectionsIndex direction, MapTraversal.MapTraversalDirectionsIndex directionOrth)
    {
        Drag_StartDrag?.Invoke(sector, direction, directionOrth);
    }

    public static void Drag_OnDragStop(bool confirmed)
    {
        Drag_StopDrag?.Invoke(confirmed);
    }
    #endregion

    #region Data Events
    public delegate void DataEvent();

    public static event DataEvent Data_Update;

    public static void Data_OnUpdate()
    {
        Data_Update?.Invoke();
    }
    #endregion
}
