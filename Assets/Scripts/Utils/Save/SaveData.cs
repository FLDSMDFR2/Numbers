using UnityEngine;

public class SaveData : MonoBehaviour
{
    public string SaveName;

    protected bool isLoaded;

    protected virtual void Awake()
    {
        LoadSaveData();
    }

    protected virtual void LoadSaveData()
    {
        isLoaded = true;
    }

    public virtual object GetSaveData()
    {
        return null;
    }

    public virtual bool IsLoaded()
    {
        return isLoaded;
    }

    protected virtual void Save()
    {
        if (IsLoaded()) SerializeManager.Save(this);
    }

    protected virtual void OnApplicationQuit()
    {
        //Save();
    }
}