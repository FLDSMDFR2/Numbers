using UnityEngine;

public class ResetButton : MonoBehaviour
{
    public virtual void ButtonPress()
    {
        GameEventSystem.UI_OnResetButtonPress();
    }
}
