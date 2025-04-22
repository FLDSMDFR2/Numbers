using System;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    protected static Dictionary<string, List<GameObject>> audioPool = new Dictionary<string, List<GameObject>>();

    public static AudioManager Instance;

    public bool EnableSound = true;
    public GameObject SoundPrefab;
    public Audio[] AudioClips;

    protected virtual void Awake()
    {
        DontDestroyOnLoad(gameObject);

        if (Instance != null)
        {
            TraceManager.WriteTrace(TraceChannel.Main, TraceType.error, "Multi AudioManager");
            return;
        }
        Instance = this;
    }

    public virtual GameObject Play(string name)
    {
        if (!EnableSound)
            return null;

        if (audioPool.ContainsKey(name) && audioPool[name].Count > 0)
        {
            foreach (var item in audioPool[name])
            {
                if (!item.activeSelf)
                {
                    item.GetComponent<Sound>()?.Play();
                    return item;
                }
            }
        }

        return Play(CreateObject(name));  
    }

    public virtual GameObject Play(GameObject obj)
    {
        obj.GetComponent<Sound>()?.Play();
        return obj;
    }

    public void Stop(GameObject obj)
    {
        if (!EnableSound)
            return;

        obj.GetComponent<Sound>()?.Stop();
    }

    public virtual void StopAll(string name)
    {
        if (!EnableSound)
            return;

        if (audioPool.ContainsKey(name) && audioPool[name].Count > 0)
        {
            foreach (var item in audioPool[name])
            {
                if (item.activeSelf)
                {
                    item.GetComponent<Sound>()?.Stop();
                }
            }
        }
    }

    protected virtual GameObject CreateObject(string name)
    {
        // look a parent object to add this new object too
        GameObject retVal = null;
        foreach (Transform child in this.transform)
        {
            if (child.name.Equals(name))
            {
                retVal = Instantiate(SoundPrefab, Vector3.zero, Quaternion.identity, child);
                break;
            }
        }

        // if we cant find a parent object create one
        if (retVal == null)
        {
            var parent = new GameObject(name);
            parent.transform.parent = this.transform;

            retVal = Instantiate(SoundPrefab, Vector3.zero, Quaternion.identity, parent.transform);
        }

        // if this is the first object of its type create list
        if (!audioPool.ContainsKey(name))
        {
            audioPool.Add(name, new List<GameObject>());
        }

        if (retVal != null)
        {
            var soundScript = retVal.GetComponent<Sound>();
            if (soundScript != null)
            {
                Audio sound = Array.Find(AudioClips, s => s.Name == name);
                if (sound != null)
                {
                    soundScript.Init(sound);
                }
                    
            }
        }

        // add new object to pool
        audioPool[name].Add(retVal);

        return retVal;
    }
}