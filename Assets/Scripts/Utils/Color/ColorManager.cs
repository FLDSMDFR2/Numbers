using System;
using System.Collections.Generic;
using UnityEngine;

public class ColorManager : MonoBehaviour
{
    public enum ColorNames
    {
        None,
        Icon,
        Icon2,
        Icon3,
        Icon4,
        Text,
        Text2,
        Text3,
        Text4,
        Color1,
        Color2,
        Color3,
        Color4,
        Background,
        TileBorderNormalColor,
        TileBorderHighlightColor,
        TileBorderErrorColor,
        TileBorderAssignedColor,
        TileFillNormalColor,
        TileFillHighlightColor,
        TileFillErrorColor,
        TileFillAssignedColor,
    }

    [Serializable]
    public class ColorManagerMap
    {
        public ColorNames Name;
        public Color Color;
    }

    [SerializeField]
    [ArrayElementTitle("Name")]
    public List<ColorManagerMap> Colors = new List<ColorManagerMap>();


    protected static Dictionary<ColorNames, Color> colorsMap = new Dictionary<ColorNames, Color>();


    protected virtual void Awake()
    {
        foreach(var c in Colors)
        {
            colorsMap[c.Name] = c.Color;
        }
    }

    public static Color GetColorByName(ColorNames name)
    {
        if (colorsMap.ContainsKey(name)) return colorsMap[name];

        return Color.red;
    }
}
