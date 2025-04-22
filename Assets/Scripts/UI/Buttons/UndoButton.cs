using UnityEngine;

public class UndoButton : MonoBehaviour
{
    public virtual void ButtonPress()
    {
        GameEventSystem.UI_OnUndoButtonPress();
    }
}
