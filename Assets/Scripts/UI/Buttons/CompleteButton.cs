using UnityEngine;

public class CompleteButton : MonoBehaviour
{
    public virtual void ButtonPress()
    {
        GameEventSystem.UI_OnCompleteButtonPress();
    }
}
