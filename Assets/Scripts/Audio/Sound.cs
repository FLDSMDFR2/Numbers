using System.Collections;
using UnityEngine;

public class Sound : MonoBehaviour
{
    public delegate void SoundEvent(Sound sound);
    public event SoundEvent Sound_Complete;

    public Audio Source;

    public virtual void Init(Audio source)
    {
        Source.source = gameObject.AddComponent<AudioSource>();
        Source.Name = source.Name;
        Source.Clip = source.Clip;
        Source.Volume = source.Volume;
        Source.Pitch = source.Pitch;
        Source.source.clip = source.Clip;
        Source.Loop = source.Loop;

        Source.source.playOnAwake = false;
        Source.source.volume = source.Volume;
        Source.source.pitch = source.Pitch;
        Source.source.loop = source.Loop;
    }

    public void Play()
    {
        this.gameObject.SetActive(true);

        if (Source.source.isPlaying) return;

        Source.source.Play();

        if (!Source.source.loop)
        {
            StartCoroutine(AudioCompleted());
        }      
    }

    protected virtual IEnumerator AudioCompleted()
    {
        // wait for clip length in secs
        yield return new WaitForSeconds(Source.source.clip.length);
        //yield return new WaitWhile(() => Source.source.isPlaying);

        Sound_Complete?.Invoke(this);

        Stop();
    }

    public void Stop()
    { 
        Source.source.Stop();
        this.gameObject.SetActive(false);
    }
}