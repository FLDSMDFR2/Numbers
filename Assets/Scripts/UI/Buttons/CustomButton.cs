using UnityEngine;

public class CustomButton : MonoBehaviour
{
    public virtual void ButtonPress()
    {
        GameEventSystem.UI_OnCustomButtonPress();
    }

    public virtual void CloseButtonPress()
    {
        GameEventSystem.UI_OnSettingsCloseButtonPress();
    }
}
