using TMPro;


public class TextColorSetter : ColorSetter
{
    protected TextMeshProUGUI text;

    protected override void GetObject()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    protected override void SetColor()
    {
        if (text != null && ColorName != ColorManager.ColorNames.None && ColorManager.GetColorByName(ColorName) != null) text.color = ColorManager.GetColorByName(ColorName);
    }
}
