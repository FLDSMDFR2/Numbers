using UnityEngine;

[System.Serializable]
public class Audio
{
    public string Name;

    public AudioClip Clip;

    [Range(0f, 1f)]
    public float Volume;
    [Range(0.01f, 3f)]
    public float Pitch;

    public bool Loop;

    [HideInInspector]
    public AudioSource source;
}
