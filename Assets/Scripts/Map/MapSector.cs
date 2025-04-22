using System;
using Unity.Mathematics;

public enum MapSectorType
{
    None,
    Used,
    ValueOrthogonal,
    ValueDiagonal,
    ValueAll,
    Rook,
    VerticalRook,
    HorizontalRook
}

public enum MapSectorState
{
    //States
    None = 0,
    Used,
    TempAssigned,
    Assigned
}


[Serializable]
public class MapSector
{
    public int2 SectorKey;

    public float SectorSizeX;
    public float SectorSizeY;
    public Tile SectorTile;

    public int Value;
    public MapSectorType Type = MapSectorType.None;
    public MapSectorState State = MapSectorState.None;

    public MapSector(int2 sectorKey, float sizeX, float sizeY) 
    {
        SectorKey = sectorKey;
        SectorSizeX = sizeX;
        SectorSizeY = sizeY;
    }

    public virtual MapSector Clone()
    {
        var clone = new MapSector(SectorKey, SectorSizeX, SectorSizeY);
        clone.SectorTile = SectorTile;
        clone.Value = Value;
        clone.Type = Type;
        clone.State = State;
        return clone;
    }

    public virtual void ResetMapSector()
    {
        Value = 0;
        SetSectorType(MapSectorType.None);
        SetSectorState(MapSectorState.None);
    }
    
    public virtual void SetSectorType(MapSectorType type)
    {
        Type = type;

    }

    public virtual void SetSectorState(MapSectorState state)
    {
        State = state;
        if (SectorTile != null) SectorTile.SetSector(this);
    }
}
