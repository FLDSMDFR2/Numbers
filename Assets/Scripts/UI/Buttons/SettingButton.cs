using UnityEngine;

public class SettingButton : MonoBehaviour
{
    public virtual void ButtonPress()
    {
        GameEventSystem.UI_OnSettingsButtonPress();
    }

    public virtual void CloseButtonPress()
    {
        GameEventSystem.UI_OnSettingsCloseButtonPress();
    }
}
