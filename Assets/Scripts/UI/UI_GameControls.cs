using UnityEngine;

public class UI_GameControls : MonoBehaviour
{
    public GameObject HomeButton;

    public GameObject FillerButton;
    public GameObject CompleteButton;
    public GameObject CustomButton;

    public GameObject SettingsButton;
    public GameObject ResetButton;
    public GameObject UndoButton;

    protected virtual void Awake()
    {
        GameEventSystem.Game_Start += GameEventSystem_Game_Start;
        GameEventSystem.UI_Update += GameEventSystem_UI_Update;
    }

    protected virtual void GameEventSystem_Game_Start(MapType mapType)
    {
        if (mapType == MapType.Custom)
        {
            FillerButton.SetActive(false);
            CompleteButton.SetActive(false);
            CustomButton.SetActive(true);
        }
        else if (mapType == MapType.Basic)
        {
            FillerButton.SetActive(false);
            CompleteButton.SetActive(true);
            CustomButton.SetActive(false);
        }
        else
        {
            FillerButton.SetActive(true);
            CompleteButton.SetActive(false);
            CustomButton.SetActive(false);
        }
    }

    protected virtual void GameEventSystem_UI_Update()
    {
        // do nothing
    }
}
