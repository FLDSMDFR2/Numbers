using System.Collections.Generic;
using UnityEngine;

public class UI_MainController : MonoBehaviour
{
    public GameObject MainDisplay;
    public GameObject GameDisplay;
    public GameObject SettingDisplay;
    public GameObject CustomDisplay;

    protected Dictionary<GameObject, bool> displayStates = new Dictionary<GameObject, bool>();

    protected virtual void Awake()
    {
        BuildDisplayStates();

        GameEventSystem.Game_Start += GameEventSystem_Game_Start;
        GameEventSystem.UI_SettingsButtonPress += GameEventSystem_UI_SettingsButtonPress;
        GameEventSystem.UI_SettingsCloseButtonPress += GameEventSystem_UI_SettingsCloseButtonPress;
        GameEventSystem.UI_HomeButtonPress += GameEventSystem_UI_HomeButtonPress;
        GameEventSystem.UI_CustomButtonPress += GameEventSystem_UI_CustomButtonPress;
    }

    protected virtual void Start()
    {
        UpdateDisplay();

        ShowMainDisplay();

        GameEventSystem.UI_OnLoaded();
    }

    protected virtual void BuildDisplayStates()
    {
        if (MainDisplay != null) displayStates.Add(MainDisplay, true);
        if (GameDisplay != null) displayStates.Add(GameDisplay, true);
        if (SettingDisplay != null) displayStates.Add(SettingDisplay, true);
        if (CustomDisplay != null) displayStates.Add(CustomDisplay, true);
    }

    protected virtual void UpdateDisplay()
    {
        foreach (var key in displayStates.Keys)
        {
            key.SetActive(displayStates[key]);
        }
    }

    protected virtual void ShowMainDisplay()
    {
        if (displayStates.ContainsKey(MainDisplay))displayStates[MainDisplay] = true;
        if (displayStates.ContainsKey(GameDisplay)) displayStates[GameDisplay] = false;
        if (displayStates.ContainsKey(SettingDisplay)) displayStates[SettingDisplay] = false;
        if (displayStates.ContainsKey(CustomDisplay)) displayStates[CustomDisplay] = false;
        UpdateDisplay();
    }

    protected virtual void ShowGameDisplay()
    {
        if (displayStates.ContainsKey(MainDisplay)) displayStates[MainDisplay] = false;
        if (displayStates.ContainsKey(GameDisplay)) displayStates[GameDisplay] = true;
        if (displayStates.ContainsKey(SettingDisplay)) displayStates[SettingDisplay] = false;
        if (displayStates.ContainsKey(CustomDisplay)) displayStates[CustomDisplay] = false;
        UpdateDisplay();
    }

    protected virtual void ShowSettingsDisplay()
    {
        //MainDisplay.SetActive(false);
        //GameDisplay.SetActive(false);

        //SettingDisplay.SetActive(true);
        if (displayStates.ContainsKey(SettingDisplay)) displayStates[SettingDisplay] = true;
        UpdateDisplay();
    }

    protected virtual void HideSettingsDisplay()
    {
        if (displayStates.ContainsKey(SettingDisplay)) displayStates[SettingDisplay] = false;
        if (displayStates.ContainsKey(CustomDisplay)) displayStates[CustomDisplay] = false;
        UpdateDisplay();
    }

    protected virtual void ShowCustomDisplay()
    {
        //MainDisplay.SetActive(false);
        //GameDisplay.SetActive(false);

        //SettingDisplay.SetActive(true);
        if (displayStates.ContainsKey(CustomDisplay)) displayStates[CustomDisplay] = true;
        UpdateDisplay();
    }

    #region Event Handlers
    protected virtual void GameEventSystem_Game_Start(MapType mapType)
    {
        ShowGameDisplay();
    }
    protected virtual void GameEventSystem_UI_SettingsButtonPress()
    {
        ShowSettingsDisplay();
    }
    protected virtual void GameEventSystem_UI_SettingsCloseButtonPress()
    {
        HideSettingsDisplay();
    }
    protected virtual void GameEventSystem_UI_HomeButtonPress()
    {
        ShowMainDisplay();
    }
    protected virtual void GameEventSystem_UI_CustomButtonPress()
    {
        ShowCustomDisplay();
    }
    #endregion

}
