using UnityEngine;

public class PlayButton : MonoBehaviour
{
    public MapType Type;

    public virtual void ButtonPress()
    {
        GameEventSystem.Game_OnStart(Type);
    }
}
