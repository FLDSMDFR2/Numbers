using UnityEngine.UI;

public class ImageColorSetter : ColorSetter
{
    protected Image image;

    protected override void GetObject() 
    {
        image = GetComponent<Image>();
    }

    protected override void SetColor() 
    {
        if (image != null && ColorName != ColorManager.ColorNames.None && ColorManager.GetColorByName(ColorName) != null) image.color = ColorManager.GetColorByName(ColorName);
    }
}
