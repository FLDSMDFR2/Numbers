using TMPro;
using UnityEngine;

public class Settings : MonoBehaviour
{
    public SettingsOption SettingsOption;
    public TextMeshProUGUI SettingsNameText;

    public virtual void HandleSettings()
    {
        GameEventSystem.UI_OnSettingsValueChange();
    }
}
