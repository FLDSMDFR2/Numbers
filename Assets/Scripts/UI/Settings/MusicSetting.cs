using System.Collections.Generic;
using UnityEngine;

public class MusicSetting : MonoBehaviour
{

    public List<string> BackgroundMusic = new List<string>();

    protected GameObject musicSound;
    protected Sound sound;
    protected bool lastMusicSetting;

    protected virtual void Awake()
    {
        GameEventSystem.UI_Loaded += GameEventSystem_UI_Loaded;
        GameEventSystem.UI_SettingsValueChange += GameEventSystem_UI_SettingsValueChange;       
    }

    protected virtual void GameEventSystem_UI_Loaded()
    {
        GameEventSystem_UI_SettingsValueChange();
    }

    protected virtual void GameEventSystem_UI_SettingsValueChange()
    {
        if (lastMusicSetting == GameSettings.Music) return;
        lastMusicSetting = GameSettings.Music;

        if (GameSettings.Music)
        {
            PlayNextBackground();
        }
        else if (musicSound != null)
        {
            if (sound != null) sound.Sound_Complete -= Sound_Sound_Complete;
            AudioManager.Instance.Stop(musicSound);
        }
    }

    protected virtual void PlayNextBackground()
    {
        var nextBackground = SelectNextBackgroundMusic();
        if (nextBackground == null) return;
        musicSound = AudioManager.Instance.Play(nextBackground);
        if (musicSound != null) sound = musicSound.GetComponent<Sound>();
        if (sound != null) sound.Sound_Complete += Sound_Sound_Complete;
    }

    protected virtual void Sound_Sound_Complete(Sound sound)
    {
        PlayNextBackground();
    }

    protected virtual string SelectNextBackgroundMusic()
    {
        if (BackgroundMusic == null || BackgroundMusic.Count <= 0) return null;
        if (BackgroundMusic.Count == 1) return BackgroundMusic[0];

        var index = Random.Range(0, BackgroundMusic.Count);
        return BackgroundMusic[index];
    }
}
