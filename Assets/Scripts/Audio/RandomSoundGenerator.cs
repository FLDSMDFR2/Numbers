using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSoundGenerator : MonoBehaviour
{
    [Serializable]
    public class SoundPercentTriggers
    {
        public string SoundName;
        public float TimeToTrigger;
        public float PercentChangeToTrigger;
    }

    [SerializeField]
    public List<SoundPercentTriggers> Sounds;


    // Start is called before the first frame update
    void Start()
    {
        if (Sounds != null & Sounds.Count > 0)
        {
            foreach(var sound in Sounds)
            {
                StartCoroutine(SoundTrigger(sound));
            }
        }
    }

    protected virtual IEnumerator SoundTrigger(SoundPercentTriggers sound)
    {
        yield return new WaitForSeconds(sound.TimeToTrigger);

        if (RandomGenerator.UnseededIsPercentage(sound.PercentChangeToTrigger)) AudioManager.Instance.Play(sound.SoundName);

        StartCoroutine(SoundTrigger(sound));
    }
}
