using System.Collections.Generic;
using UnityEngine;

public class ToggleSwitchGroupManager : MonoBehaviour
{
    [Header("Start Value")]
    public ToggleSwitch InitialToggleSwitch;

    [Header("Toggle Options")]
    public bool AllCanBeToggleOff;

    public List<ToggleSwitch> ToggleSwitches = new List<ToggleSwitch>();

    protected virtual void Awake()
    {
        var switched = GetComponentsInChildren<ToggleSwitch>();
        foreach (var ts in switched)
        {
            RegisterToggleButtonToGroup(ts);
        }
    }

    protected virtual void Start()
    {
        var areAllToggledOff = true;
        foreach(var ts in ToggleSwitches)
        {
            if (!ts.CurrentValue) continue;

            areAllToggledOff = false;
            break;
        }

        if (!areAllToggledOff || AllCanBeToggleOff) return;

        if (InitialToggleSwitch != null)
            InitialToggleSwitch.ToggleByGroupManager(true);
        else if (ToggleSwitches.Count > 0)
            ToggleSwitches[0].ToggleByGroupManager(true);

    }

    protected virtual void RegisterToggleButtonToGroup(ToggleSwitch toggleSwitch)
    {
        toggleSwitch.SetupForToggleManager(this);

        if (ToggleSwitches.Contains(toggleSwitch)) return;

        ToggleSwitches.Add(toggleSwitch);
     }


    public virtual void ToggleGroup(ToggleSwitch toggleSwitch)
    {
        if (ToggleSwitches == null || ToggleSwitches.Count <= 1) return;

        if (AllCanBeToggleOff && toggleSwitch.CurrentValue)
        {
            toggleSwitch.ToggleByGroupManager(!toggleSwitch.CurrentValue);
        }
        else if (!AllCanBeToggleOff && toggleSwitch.CurrentValue)
        {
            var currentValue = toggleSwitch.CurrentValue;
            foreach (var ts in ToggleSwitches)
            {
                if (ts == null) continue;

                if (ts == toggleSwitch)
                    ts.ToggleByGroupManager(!currentValue);
                else
                    ts.ToggleByGroupManager(currentValue);
            }
        }
        else
        {
            var currentValue = toggleSwitch.CurrentValue;
            foreach (var ts in ToggleSwitches)
            {
                if (ts == null) continue;

                if (ts == toggleSwitch)
                    ts.ToggleByGroupManager(!currentValue);
                else
                    ts.ToggleByGroupManager(currentValue);
            }
        }
    }
}
