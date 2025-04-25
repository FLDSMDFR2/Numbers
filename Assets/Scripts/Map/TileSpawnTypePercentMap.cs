using System;
using System.Collections.Generic;
using UnityEngine;

public class TileSpawnTypePercentMap : MonoBehaviour
{
    [Serializable]
    public class TileSpawnTypePercentsDtls
    {
        public bool Enabled = true;
        public MapSectorType TileType;
        public int BaseMin;
        public int MinCap;
        public int BaseMax;
        public int MaxCap;
        public int LevelIncrease;
        public int LevelOffset;
        public int StartLevel;
    }

    [SerializeField]
    public List<TileSpawnTypePercentsDtls> GOTDTypePercentMap = new List<TileSpawnTypePercentsDtls>();
    [SerializeField]
    public List<TileSpawnTypePercentsDtls> ChallengeTypePercentMap = new List<TileSpawnTypePercentsDtls>();
    [SerializeField]
    public List<TileSpawnTypePercentsDtls> BasicTypePercentMap = new List<TileSpawnTypePercentsDtls>();
    public virtual List<MapSectorType> GetTilesToSpawnByTypeAndLevel(MapType type, long Level)
    {

        var TypePercentMap = new List<TileSpawnTypePercentsDtls>();

        if (type == MapType.GOTD) TypePercentMap = GOTDTypePercentMap;
        else if (type == MapType.Challenge || IsCustomLevel(type, ref Level)) TypePercentMap = ChallengeTypePercentMap;
        else if (type == MapType.Basic) TypePercentMap = BasicTypePercentMap;
        else if (type == MapType.Custom) TypePercentMap = BuildCustomList();

        if (TypePercentMap == null || TypePercentMap.Count <= 0) return new List<MapSectorType>();

        var retVal = new List<MapSectorType>();

        foreach (var dtls in TypePercentMap)
        {
            if (!dtls.Enabled) continue;

            var count = GetToSpawnCountByLevel(Level, dtls);
            for (int i = 0; i < count; i++)
            {
                retVal.Add(dtls.TileType);
            }
        }

        return retVal;
    }

    protected virtual int GetToSpawnCountByLevel(long level, TileSpawnTypePercentsDtls dtls)
    {
        if (dtls.StartLevel > level) return 0;

        var min = dtls.BaseMin;
        var max = dtls.BaseMax;

        var levelOffestsToApply = dtls.LevelOffset > 0 ? (level - dtls.StartLevel) / dtls.LevelOffset : 0;

        min = Mathf.Clamp(min + (dtls.LevelIncrease * (int)levelOffestsToApply), min, dtls.MinCap);
        max = Mathf.Clamp(max + (dtls.LevelIncrease * (int)levelOffestsToApply), max, dtls.MaxCap);

        return RandomGenerator.SeededRange(min, max);
    }

    protected virtual List<TileSpawnTypePercentsDtls> BuildCustomList()
    {
        var retVal = new List<TileSpawnTypePercentsDtls>();
        TileSpawnTypePercentsDtls dtls;

        if (CustomValues.Square)
        {
            dtls = new TileSpawnTypePercentsDtls();
            dtls.Enabled = true;
            dtls.TileType = MapSectorType.ValueOrthogonal;
            dtls.BaseMin = CustomValues.MinSquare;
            dtls.MinCap = CustomValues.MinSquare;
            dtls.BaseMax = CustomValues.MaxSquare;
            dtls.MaxCap = CustomValues.MaxSquare;
            retVal.Add(dtls);
        }

        if (CustomValues.Diamond)
        {
            dtls = new TileSpawnTypePercentsDtls();
            dtls.Enabled = true;
            dtls.TileType = MapSectorType.ValueDiagonal;
            dtls.BaseMin = CustomValues.MinDiamond;
            dtls.MinCap = CustomValues.MinDiamond;
            dtls.BaseMax = CustomValues.MaxDiamond;
            dtls.MaxCap = CustomValues.MaxDiamond;
            retVal.Add(dtls);
        }

        if (CustomValues.Hexagon)
        {
            dtls = new TileSpawnTypePercentsDtls();
            dtls.Enabled = true;
            dtls.TileType = MapSectorType.ValueAll;
            dtls.BaseMin = CustomValues.MinHexagon;
            dtls.MinCap = CustomValues.MinHexagon;
            dtls.BaseMax = CustomValues.MaxHexagon;
            dtls.MaxCap = CustomValues.MaxHexagon;
            retVal.Add(dtls);
        }

        if (CustomValues.Rook)
        {
            dtls = new TileSpawnTypePercentsDtls();
            dtls.Enabled = true;
            dtls.TileType = MapSectorType.Rook;
            dtls.BaseMin = CustomValues.MinRook;
            dtls.MinCap = CustomValues.MinRook;
            dtls.BaseMax = CustomValues.MaxRook;
            dtls.MaxCap = CustomValues.MaxRook;
            retVal.Add(dtls);
        }

        return retVal;
    }

    protected virtual bool IsCustomLevel(MapType type, ref long level)
    {
        if (type != MapType.Custom) return false;
        if (CustomValues.Level <= 0) return false;

        level = CustomValues.Level;

        return true;
    }
}
