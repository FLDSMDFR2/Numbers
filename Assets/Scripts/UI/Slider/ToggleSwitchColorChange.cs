using UnityEngine;
using UnityEngine.UI;

public class ToggleSwitchColorChange : ToggleSwitch
{

    [Header("Elements To Recolor")]
    public Image BackgroundImage;
    public Image HandleImage;

    [Space]
    public bool RecolorBackGround;
    public bool RecolorHandle;

    [Header("Colors")]
    public Color BackGroundColorOff = Color.white;
    public Color BackGroundColorOn = Color.white;

    public Color HandleColorOff = Color.white;
    public Color HandleColorOn = Color.white;

    protected bool isBackgroundNull;
    protected bool isHandleNull;

    protected new void OnValidate()
    {
        base.OnValidate();

        CheckForNull();
        ChangeColors();
    }

    protected override void Awake()
    {
        base.Awake();

        CheckForNull();
        ChangeColors();
    }

    protected virtual void OnEnable()
    {
        transitionEffect += ChangeColors;
    }
    protected virtual void OnDisaable()
    {
        transitionEffect -= ChangeColors;
    }

    protected virtual void CheckForNull()
    {
        isBackgroundNull = BackgroundImage == null;
        isHandleNull = HandleImage == null;
    }

    protected virtual void ChangeColors()
    {
        if (RecolorBackGround && !isBackgroundNull) BackgroundImage.color = Color.Lerp(BackGroundColorOff, BackGroundColorOn, slider.value);
        if (RecolorHandle && !isHandleNull) HandleImage.color = Color.Lerp(HandleColorOff, HandleColorOn, slider.value);
    }
}
