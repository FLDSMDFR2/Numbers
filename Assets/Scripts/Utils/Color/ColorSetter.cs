using UnityEngine;

public class ColorSetter : MonoBehaviour
{
    public ColorManager.ColorNames ColorName;

    protected virtual void Awake()
    {
        GetObject();
    }

    protected virtual void Start()
    {
        SetColor();
    }

    protected virtual void GetObject(){ }

    protected virtual void SetColor() { }

}
