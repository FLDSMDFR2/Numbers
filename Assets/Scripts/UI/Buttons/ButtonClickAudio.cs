using UnityEngine;

public class ButtonClickAudio : MonoBehaviour
{
    public string AudioFileName;

    public virtual void PlayAudio()
    {
        if (GameSettings.Sound) AudioManager.Instance.Play(AudioFileName);
    }
}
