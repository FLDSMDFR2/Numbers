using System.Collections.Generic;
using UnityEngine;

public class CustomController : MonoBehaviour
{
    public List<GameObject> NumberBoxObject;

    public virtual void ToggleOn()
    {
        if (NumberBoxObject == null) return;
        
        foreach (GameObject go in NumberBoxObject)
        {
            go.SetActive(true);
        }
        
    }
    public virtual void ToggleOff()
    {
        if (NumberBoxObject == null) return;

        foreach (GameObject go in NumberBoxObject)
        {
            go.SetActive(false);
        }
    }
}
