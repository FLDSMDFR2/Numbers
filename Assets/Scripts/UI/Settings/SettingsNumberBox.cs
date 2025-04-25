using System;
using TMPro;

public class SettingsNumberBox : Settings
{
    public int MaxValue;
    public int MinValue;
    public int NumberBoxValue;

    public TMP_InputField InputFieldValue;

    protected virtual void Awake()
    {
        ValidateText();
    }

    public override void HandleSettings()
    {
        //base.HandleSettings();

        ValidateText();

        GameEventSystem.UI_OnCustomNumberBoxUpdate();
    }

    protected virtual bool ValidateText()
    {
        if (!int.TryParse(InputFieldValue.text, out int value))
        {
            NumberBoxValue = -1;
            InputFieldValue.text = string.Empty;
            return false;
        }

        NumberBoxValue = Math.Clamp(value, MinValue, MaxValue);
        InputFieldValue.text = NumberBoxValue.ToString();
        return true;
    }
}
