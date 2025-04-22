using UnityEngine;

public class HomeButton : MonoBehaviour
{
    public virtual void ButtonPress()
    {
        GameEventSystem.UI_OnHomeButtonPress();
    }
}
