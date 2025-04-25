using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ToggleSwitchGroupManager : MonoBehaviour
{
    [Serializable]
    public class ToggleGroupDtls
    {
        public ToggleSwitch Toggle;
        public int GroupNumber;
    }

    [Header("Start Value")]
    public ToggleSwitch InitialToggleSwitch;

    [Header("Toggle Options")]
    public bool AllCanBeToggleOff;
    public bool AllCanBeToggleOn;
    public bool CanMultipleGroupsBeToggleOn; 


    public List<ToggleGroupDtls> ToggleSwitches = new List<ToggleGroupDtls>();

    protected virtual void Awake()
    {
        foreach (var ts in ToggleSwitches)
        {
            RegisterToggleButtonToGroup(ts.Toggle);
        }
    }

    protected virtual void Start()
    {
        var areAllToggledOff = true;
        foreach(var ts in ToggleSwitches)
        {
            if (!ts.Toggle.CurrentValue) continue;

            areAllToggledOff = false;
            break;
        }

        if (!areAllToggledOff || AllCanBeToggleOff) return;

        if (InitialToggleSwitch != null)
            InitialToggleSwitch.ToggleByGroupManager(true);
        else if (ToggleSwitches.Count > 0)
            ToggleSwitches[0].Toggle.ToggleByGroupManager(true);

    }

    protected virtual void RegisterToggleButtonToGroup(ToggleSwitch toggleSwitch)
    {
        toggleSwitch.SetupForToggleManager(this);
     }


    public virtual void ToggleGroup(ToggleSwitch toggleSwitch)
    {
        if (ToggleSwitches == null || ToggleSwitches.Count <= 1) return;

        if ((AllCanBeToggleOff && toggleSwitch.CurrentValue) ||
            (AllCanBeToggleOn && !toggleSwitch.CurrentValue))
        {
            toggleSwitch.ToggleByGroupManager(!toggleSwitch.CurrentValue);
        }
        else if (!CanMultipleGroupsBeToggleOn && !toggleSwitch.CurrentValue)
        {
            var group = ToggleSwitches.First(x => x.Toggle == toggleSwitch);

            if (group == null) 
            {
                DefaultToggle(toggleSwitch);
                return;
            }

            var currentValue = toggleSwitch.CurrentValue;
            foreach (var ts in ToggleSwitches)
            {
                if (ts == null || ts.Toggle == null) continue;

                if (ts.Toggle == toggleSwitch)
                    ts.Toggle.ToggleByGroupManager(!currentValue);
                else if (group.GroupNumber != ts.GroupNumber)
                    ts.Toggle.ToggleByGroupManager(currentValue);
            }
        }
        else
        {
            DefaultToggle(toggleSwitch);
        }
    }

    protected virtual void DefaultToggle(ToggleSwitch toggleSwitch)
    {
        var currentValue = toggleSwitch.CurrentValue;
        foreach (var ts in ToggleSwitches)
        {
            if (ts == null || ts.Toggle == null) continue;

            if (ts.Toggle == toggleSwitch)
                ts.Toggle.ToggleByGroupManager(!currentValue);
            else
                ts.Toggle.ToggleByGroupManager(currentValue);
        }
    }
}
