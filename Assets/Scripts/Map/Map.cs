using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;
using static MapTraversal;

public enum MapType
{
    None = 0,
    GOTD,
    Challenge,
    Basic,
    Custom
}

public class Map : MonoBehaviour
{
    protected class ActionHistoryDtl
    {
        public MapSector oldDtls;

        public ActionHistoryDtl(MapSector sector)
        {
            oldDtls = sector;
        }
    }

    protected class RookBuilder
    {
        public MapSectorType Type;
        public List<int2> keys = new List<int2>();
    }

    public int MapSizeX;
    public int MapSizeY;

    public float SectorSizeX;
    public float SectorSizeY;

    public RectTransform ScreenWidth;
    public GridLayoutGroup GridLayout;
    public int GridLayoutSpacing;
    public int GridLayoutPadding;

    public TileSpawner TileSpawner;
    public TileSpawnTypePercentMap TileSpawnTypePercent;

    public int Seed;
    public int DataSeed;
    public int PuzzleNumber;

    protected Dictionary<int2, MapSector> sectorMap = new Dictionary<int2, MapSector>();

    protected List<int2> openSectors = new List<int2>();
    protected List<int2> usedSectors = new List<int2>();

    protected List<int2> highlightedSectors = new List<int2>();

    protected Dictionary<int, List<ActionHistoryDtl>> actionHistory = new Dictionary<int, List<ActionHistoryDtl>>();

    protected int rookSize = 1;
    protected int maxLinearDistance = 9;

    protected long mapLevel;
    protected MapType type;

    #region Testing
    [InspectorButton("OnButtonClicked")]
    public bool Rebuild;

    private void OnButtonClicked()
    {
        //GameEventSystem_Map_Complete();
    }
    #endregion

    protected virtual void Awake()
    {
        InitMap();

        GameEventSystem.Drag_StartDrag += GameEventSystem_Drag_StartDrag;
        GameEventSystem.Drag_StopDrag += GameEventSystem_Drag_StopDrag;

        GameEventSystem.Game_Start += GameEventSystem_Game_Start;
        GameEventSystem.Map_Complete += GameEventSystem_Map_Complete;

        GameEventSystem.UI_ResetButtonPress += GameEventSystem_UI_ResetButtonPress;
        GameEventSystem.UI_UndoButtonPress += GameEventSystem_UI_UndoButtonPress;
        GameEventSystem.UI_CompleteButtonPress += GameEventSystem_UI_CompleteButtonPress;       
    }

    #region Event Handlers
    protected virtual void GameEventSystem_UI_UndoButtonPress()
    {
        if (actionHistory.Keys.Count <= 0) return;

        var key = actionHistory.Keys.Count - 1;
        if (!actionHistory.ContainsKey(key)) return;

        foreach (var oldData in actionHistory[key])
        {
            sectorMap[oldData.oldDtls.SectorKey].Value = oldData.oldDtls.Value;
            sectorMap[oldData.oldDtls.SectorKey].SetSectorState(oldData.oldDtls.State);
        }

        actionHistory.Remove(key);
    }

    protected virtual void GameEventSystem_UI_ResetButtonPress()
    {
        RandomGenerator.SetSeed(PuzzleNumber);
        BuildMap();
    }

    protected virtual void GameEventSystem_UI_CompleteButtonPress()
    {
        GameEventSystem.Map_OnComplete();
    }

    protected virtual void GameEventSystem_Game_Start(MapType mapType)
    {
        type = mapType;
        PuzzleNumber = -1;

        RandomGenerator.SetSeed(UpdatePuzzleNumber());

        GameEventSystem.UI_OnUpdate();
        BuildMap();
    }

    protected virtual void GameEventSystem_Map_Complete()
    {
        if (type == MapType.Challenge) GameData.AddPuzzlesCompleted(1);
        else if (type == MapType.GOTD) GameData.TryAddGOTDCompleted(1);

        RandomGenerator.SetSeed(UpdatePuzzleNumber());

        if (GameSettings.Sound) AudioManager.Instance.Play("Complete1");

        BuildMap();
    }

    protected virtual int UpdatePuzzleNumber(int Number = -1)
    {
        if (Number < 0)
        {
            if (type == MapType.Challenge)
            {
                mapLevel = PuzzlesPerLevel.GetLevelByCompletedPuzzles(GameData.PuzzlesCompleted);
                PuzzleNumber = (int)(Seed + GameData.PuzzlesCompleted);
            }
            else if (type == MapType.GOTD)
            {
                PuzzleNumber = (int)(DataSeed + DateToInt.GetDateInt());
            }
            else
            {
                if (PuzzleNumber <= 0) PuzzleNumber = RandomGenerator.UnseededRange(0, int.MaxValue / 2);
                else PuzzleNumber++;
            }
        }
        else
        {
            PuzzleNumber = Number;
        }

        return PuzzleNumber;
    }
    #endregion

    #region HighLights

    #region StartDrag
    protected virtual void GameEventSystem_Drag_StartDrag(MapSector sector, MapTraversal.MapTraversalDirectionsIndex direction, MapTraversal.MapTraversalDirectionsIndex directionOrth)
    {
        if (sector.Type == MapSectorType.ValueOrthogonal)
        {
            StartDrag_ValueOrthogonal(sector, directionOrth);
        }
        else if (sector.Type == MapSectorType.ValueDiagonal)
        {
            StartDrag_ValueDiagonal(sector, direction);
        }
        else if (sector.Type == MapSectorType.ValueAll)
        {
            StartDrag_ValueAll(sector, direction);
        }
        else if (sector.Type == MapSectorType.VerticalRook || sector.Type == MapSectorType.HorizontalRook)
        {
            StartDrag_Rook(sector, direction);
        }
    }

    protected virtual void StartDrag_ValueOrthogonal(MapSector sector, MapTraversal.MapTraversalDirectionsIndex direction)
    {
        var highlightCount = sector.Value;
        var highlightSectorKey = sector.SectorKey;
        var allSectorsFound = true;

        highlightedSectors.Add(highlightSectorKey);

        while (highlightCount > 0)
        {
            highlightSectorKey += MapTraversal.NeighborsDirectionsAll[(int)direction];

            if (KeyOutOfBounds(highlightSectorKey) || sectorMap[highlightSectorKey].Type == MapSectorType.None)
            {
                allSectorsFound = false;
                break;
            }

            if (sectorMap.ContainsKey(highlightSectorKey) && (sectorMap[highlightSectorKey].State == MapSectorState.Used && sectorMap[highlightSectorKey].Type == MapSectorType.Used))
            {
                highlightedSectors.Add(highlightSectorKey);
                highlightCount--;
            }
        }

        foreach (var foundKeys in highlightedSectors)
        {
            if (allSectorsFound) sectorMap[foundKeys].SectorTile.HighlightOn();
            else sectorMap[foundKeys].SectorTile.HighlightError();
        }
    }

    protected virtual void StartDrag_ValueDiagonal(MapSector sector, MapTraversal.MapTraversalDirectionsIndex direction)
    {
        if (!MapTraversal.IsDiagonal(direction)) return;

        var highlightCount = sector.Value;
        var highlightSectorKey = sector.SectorKey;
        var allSectorsFound = true;

        highlightedSectors.Add(highlightSectorKey);

        while (highlightCount > 0)
        {
            highlightSectorKey += MapTraversal.NeighborsDirectionsAll[(int)direction];

            if (KeyOutOfBounds(highlightSectorKey) || sectorMap[highlightSectorKey].Type == MapSectorType.None)
            {
                allSectorsFound = false;
                break;
            }

            if (sectorMap.ContainsKey(highlightSectorKey) && (sectorMap[highlightSectorKey].State == MapSectorState.Used && sectorMap[highlightSectorKey].Type == MapSectorType.Used))
            {
                highlightedSectors.Add(highlightSectorKey);
                highlightCount--;
            }
        }

        foreach (var foundKeys in highlightedSectors)
        {
            if (allSectorsFound) sectorMap[foundKeys].SectorTile.HighlightOn();
            else sectorMap[foundKeys].SectorTile.HighlightError();
        }
    }

    protected virtual void StartDrag_ValueAll(MapSector sector, MapTraversal.MapTraversalDirectionsIndex direction)
    {
        var highlightCount = sector.Value;
        var highlightSectorKey = sector.SectorKey;
        var allSectorsFound = true;

        highlightedSectors.Add(highlightSectorKey);

        while (highlightCount > 0)
        {
            highlightSectorKey += MapTraversal.NeighborsDirectionsAll[(int)direction];

            if (KeyOutOfBounds(highlightSectorKey) || sectorMap[highlightSectorKey].Type == MapSectorType.None)
            {
                allSectorsFound = false;
                break;
            }

            if (sectorMap.ContainsKey(highlightSectorKey) && (sectorMap[highlightSectorKey].State == MapSectorState.Used && sectorMap[highlightSectorKey].Type == MapSectorType.Used))
            {
                highlightedSectors.Add(highlightSectorKey);
                highlightCount--;
            }
        }

        foreach (var foundKeys in highlightedSectors)
        {
            if (allSectorsFound) sectorMap[foundKeys].SectorTile.HighlightOn();
            else sectorMap[foundKeys].SectorTile.HighlightError();
        }
    }

    protected virtual void StartDrag_Rook(MapSector sector, MapTraversal.MapTraversalDirectionsIndex direction)
    {
        if (sector.Type == MapSectorType.VerticalRook)
        {
            if (direction == MapTraversalDirectionsIndex.UpRight) RookHighlight(sector.SectorKey, MapTraversalDirectionsIndex.Up, MapTraversalDirectionsIndex.Right);
            else if (direction == MapTraversalDirectionsIndex.UpLeft) RookHighlight(sector.SectorKey, MapTraversalDirectionsIndex.Up, MapTraversalDirectionsIndex.Left);
            else if (direction == MapTraversalDirectionsIndex.DownRight) RookHighlight(sector.SectorKey, MapTraversalDirectionsIndex.Down, MapTraversalDirectionsIndex.Right);
            else if (direction == MapTraversalDirectionsIndex.DownLeft) RookHighlight(sector.SectorKey, MapTraversalDirectionsIndex.Down, MapTraversalDirectionsIndex.Left);
        }
        else if (sector.Type == MapSectorType.HorizontalRook)
        {
            if (direction == MapTraversalDirectionsIndex.UpRight) RookHighlight(sector.SectorKey, MapTraversalDirectionsIndex.Right, MapTraversalDirectionsIndex.Up);
            else if (direction == MapTraversalDirectionsIndex.UpLeft) RookHighlight(sector.SectorKey, MapTraversalDirectionsIndex.Left, MapTraversalDirectionsIndex.Up);
            else if (direction == MapTraversalDirectionsIndex.DownRight) RookHighlight(sector.SectorKey, MapTraversalDirectionsIndex.Right, MapTraversalDirectionsIndex.Down);
            else if (direction == MapTraversalDirectionsIndex.DownLeft) RookHighlight(sector.SectorKey, MapTraversalDirectionsIndex.Left, MapTraversalDirectionsIndex.Down);
        }
    }

    protected virtual void RookHighlight(int2 key, MapTraversalDirectionsIndex first, MapTraversalDirectionsIndex second)
    {
        var allSectorsFound = true;

        highlightedSectors.Add(key);

        //first move
        var workingKey = key;
        var distance = rookSize + 1;
        while (distance > 0)
        {

            workingKey += MapTraversal.NeighborsDirectionsAll[(int)first];

            if (KeyOutOfBounds(workingKey) || sectorMap[workingKey].Type == MapSectorType.None)
            {
                allSectorsFound = false;
                break;
            }

            if (sectorMap.ContainsKey(workingKey) && (sectorMap[workingKey].State == MapSectorState.Used && sectorMap[workingKey].Type == MapSectorType.Used))
            {
                highlightedSectors.Add(workingKey);
                distance--;
            }
        }

        if (allSectorsFound)
        {
            //second move
            distance = rookSize;
            while (distance > 0)
            {

                workingKey += MapTraversal.NeighborsDirectionsAll[(int)second];

                if (KeyOutOfBounds(workingKey) || sectorMap[workingKey].Type == MapSectorType.None)
                {
                    allSectorsFound = false;
                    break;
                }

                if (sectorMap.ContainsKey(workingKey) && (sectorMap[workingKey].State == MapSectorState.Used && sectorMap[workingKey].Type == MapSectorType.Used))
                {
                    highlightedSectors.Add(workingKey);
                    distance--;
                }
            }
        }

        foreach (var foundKeys in highlightedSectors)
        {
            if (allSectorsFound) sectorMap[foundKeys].SectorTile.HighlightOn();
            else sectorMap[foundKeys].SectorTile.HighlightError();
        }
    }
    #endregion

    #region EndDrag
    protected virtual void GameEventSystem_Drag_StopDrag(bool confirmed)
    {
        if (confirmed)
        {
            var newHitoryIndex = actionHistory.Keys.Count;
            var historyData = new List<ActionHistoryDtl>();
            var assigned = new List<int2>();

            foreach (var sectorKey in highlightedSectors)
            {
                // if anything is errored then set back to normal
                if (sectorMap[sectorKey].SectorTile.IsErrorHighlight)
                {
                    sectorMap[sectorKey].SectorTile.HighlightNormal();
                }
                else
                {
                    // otherwise assign
                    historyData.Add(new ActionHistoryDtl(sectorMap[sectorKey].Clone()));
                    sectorMap[sectorKey].SectorTile.TempAssigned();
                    assigned.Add(sectorKey);
                }
            }

            if (historyData.Count > 0) actionHistory.Add(newHitoryIndex, historyData);
            if (assigned.Count > 0) StartCoroutine(PerformAssign(assigned));
        }
        else
        {
            foreach (var sectorKey in highlightedSectors)
            {
                sectorMap[sectorKey].SectorTile.HighlightNormal();
            }
        }
        highlightedSectors.Clear();
    }
    protected virtual IEnumerator PerformAssign(List<int2> assigned)
    {
        foreach (var sectorKey in assigned)
        {
            yield return new WaitForSeconds(0.08f);

            sectorMap[sectorKey].SectorTile.Assigned();
        }

        CheckComplete();
    }
    protected virtual void CheckComplete()
    {
        if (!IsMapComplete()) return;

        GameEventSystem.Map_OnComplete();
    }
    #endregion

    #endregion 

    protected virtual bool IsMapComplete()
    {
        foreach(var sectorKey in usedSectors)
        {
            if (sectorMap[sectorKey].State != MapSectorState.Assigned) return false;
        }

        return true;
    }

    protected virtual void InitMap()
    {
        actionHistory.Clear();
        sectorMap.Clear();
        usedSectors.Clear();
        openSectors.Clear();

        for (int x = 0; x < MapSizeX; x++)
        {
            for (int y = 0; y < MapSizeY; y++)
            {
                var key = new int2(x, y);
                openSectors.Add(key);
                sectorMap.Add(key, new MapSector(key, SectorSizeX, SectorSizeY));
                if (TileSpawner != null) TileSpawner.SpawnTile(sectorMap[key]);
            }
        }
    }

    protected virtual void SetupGridLayout()
    {
        if (GridLayout == null || ScreenWidth == null) return;

        GridLayout.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        GridLayout.constraintCount = MapSizeX;
        GridLayout.padding.right = GridLayoutPadding;
        GridLayout.padding.left = GridLayoutPadding;
        GridLayout.spacing = new Vector2(GridLayoutSpacing, GridLayoutSpacing);

        float screenWidth = ScreenWidth.sizeDelta.x - (GridLayoutPadding * 2);
        screenWidth = (screenWidth - (GridLayoutSpacing * (MapSizeX - 1)));
        screenWidth = screenWidth / MapSizeX;
        GridLayout.cellSize = new Vector2(screenWidth, screenWidth);
    }

    protected virtual void BuildMap()
    {
        ResetMap();

        if (TileSpawnTypePercent == null) return;

        var toSpawn = TileSpawnTypePercent.GetTilesToSpawnByTypeAndLevel(type, mapLevel);
        while (toSpawn.Count  > 0)
        {
            var lookIndex = RandomGenerator.SeededRange(0, toSpawn.Count);
            var typeToSpawn = toSpawn[lookIndex];

            TryCreateTile(typeToSpawn);

            toSpawn.Remove(typeToSpawn);
        }
    }

    protected virtual void ResetMap()
    {
        actionHistory.Clear();

        foreach (var sectorKey in usedSectors)
        {
            sectorMap[sectorKey].ResetMapSector();
            openSectors.Add(sectorKey);
        }

        usedSectors.Clear();
        openSectors.Clear();
        foreach(var key in sectorMap.Keys)
        {
            openSectors.Add(key);
        }

        SetupGridLayout();
    }

    protected virtual void TryCreateTile(MapSectorType TileType)
    {
        if (TileType == MapSectorType.ValueDiagonal || TileType == MapSectorType.ValueOrthogonal || TileType == MapSectorType.ValueAll)
        {
            CreateLinearTile(TileType);
        }
        else if (TileType == MapSectorType.Rook)
        {
            CreateRookTile(TileType);
        }
    }

    protected virtual void CreateLinearTile(MapSectorType TileType)
    {
        if (openSectors.Count <= 0)
        {
            return;
        }

        var key = GetTileKey();

        var maxDistance = 0;
        var direction = MapTraversal.MapTraversalDirectionsIndex.None;

        if (TileType == MapSectorType.ValueOrthogonal) GetDistanceAndDirection_v2(key, MapTraversal.NeighborsDirectionsOrtho, ref maxDistance, ref direction);
        else if (TileType == MapSectorType.ValueDiagonal) GetDistanceAndDirection_v2(key, MapTraversal.NeighborsDirectionsDiag, ref maxDistance, ref direction);
        else if (TileType == MapSectorType.ValueAll) GetDistanceAndDirection_v2(key, MapTraversal.NeighborsDirectionsAll, ref maxDistance, ref direction);

        if (maxDistance <= 0 || direction == MapTraversalDirectionsIndex.None)
        {
            return;
        }

        var distance = RandomGenerator.SeededRange(1, Mathf.Clamp(maxDistance, 1, maxLinearDistance));
        var workingKey = key;
        var count = 0;
        var tempUsed = new List<int2>();
        while (distance > 0)
        {

            workingKey += MapTraversal.NeighborsDirectionsAll[(int)direction];

            if (!sectorMap.ContainsKey(workingKey))
            {
                continue;
            }

            if (sectorMap[workingKey].Type != MapSectorType.None)
            {
                continue;
            }

            distance--;
            count++;
            tempUsed.Add(workingKey);
        }

        if (count <= 0)
        {
            return;
        }

        foreach (var tempKey in tempUsed)
        {
            sectorMap[tempKey].SetSectorType(MapSectorType.Used);
            sectorMap[tempKey].SetSectorState(MapSectorState.Used);
            openSectors.Remove(tempKey);
            usedSectors.Add(tempKey);
        }

        sectorMap[key].Value = count;
        sectorMap[key].SetSectorType(TileType);
        sectorMap[key].SetSectorState(MapSectorState.Used);
        openSectors.Remove(key);
        usedSectors.Add(key);
    }
    protected virtual void CreateRookTile(MapSectorType TileType)
    {
        if (openSectors.Count <= 0)
        {
            return;
        }

        var key = GetTileKey();

        var validMoves = new List<RookBuilder>();
        // up right
        var rookPattern = GetRookPattern(key, MapSectorType.VerticalRook, MapTraversalDirectionsIndex.Up, MapTraversalDirectionsIndex.Right);
        if (rookPattern != null) validMoves.Add(rookPattern);
        // up left
        rookPattern = GetRookPattern(key, MapSectorType.VerticalRook, MapTraversalDirectionsIndex.Up, MapTraversalDirectionsIndex.Left);
        if (rookPattern != null) validMoves.Add(rookPattern);
        // down right
        rookPattern = GetRookPattern(key, MapSectorType.VerticalRook, MapTraversalDirectionsIndex.Down, MapTraversalDirectionsIndex.Right);
        if (rookPattern != null) validMoves.Add(rookPattern);
        // down left
        rookPattern = GetRookPattern(key, MapSectorType.VerticalRook, MapTraversalDirectionsIndex.Down, MapTraversalDirectionsIndex.Left);
        if (rookPattern != null) validMoves.Add(rookPattern);
        // right up
        rookPattern = GetRookPattern(key, MapSectorType.HorizontalRook, MapTraversalDirectionsIndex.Right, MapTraversalDirectionsIndex.Up);
        if (rookPattern != null) validMoves.Add(rookPattern);
        // right down
        rookPattern = GetRookPattern(key, MapSectorType.HorizontalRook, MapTraversalDirectionsIndex.Right, MapTraversalDirectionsIndex.Down);
        if (rookPattern != null) validMoves.Add(rookPattern);
        // Left Up
        rookPattern = GetRookPattern(key, MapSectorType.HorizontalRook, MapTraversalDirectionsIndex.Left, MapTraversalDirectionsIndex.Up);
        if (rookPattern != null) validMoves.Add(rookPattern);
        // Left Down
        rookPattern = GetRookPattern(key, MapSectorType.HorizontalRook, MapTraversalDirectionsIndex.Left, MapTraversalDirectionsIndex.Down);
        if (rookPattern != null) validMoves.Add(rookPattern);

        if (validMoves.Count <= 0) return;

        var index = RandomGenerator.SeededRange(0, validMoves.Count);
        foreach (var tempKey in validMoves[index].keys)
        {
            sectorMap[tempKey].SetSectorType(MapSectorType.Used);
            sectorMap[tempKey].SetSectorState(MapSectorState.Used);
            openSectors.Remove(tempKey);
            usedSectors.Add(tempKey);
        }

        sectorMap[key].SetSectorType(validMoves[index].Type);
        sectorMap[key].SetSectorState(MapSectorState.Used);
        openSectors.Remove(key);
        usedSectors.Add(key);
    }

    protected virtual int2 GetTileKey()
    {
        int index = 0;
        if (usedSectors.Count <= 0)
        {
            // no used sectors yet so get an open one
            index = RandomGenerator.SeededRange(0, openSectors.Count);
            return openSectors[index];
        }

        var sectorsToCheck = new List<int2>();
        foreach (var sec in usedSectors)
        {
            sectorsToCheck.Add(sec);
        }

        while (sectorsToCheck.Count > 0)
        {
            index = RandomGenerator.SeededRange(0, sectorsToCheck.Count);
            var key = sectorsToCheck[index];

            var randomDirectionLook = new List<int2>();
            foreach (var dir in MapTraversal.NeighborsDirectionsAll)
            {
                randomDirectionLook.Add(dir);
            }

            while (randomDirectionLook.Count > 0)
            {
                var lookDirIndex = RandomGenerator.SeededRange(0, randomDirectionLook.Count);
                var lookDir = randomDirectionLook[lookDirIndex];

                if (sectorMap.ContainsKey(key + lookDir) && sectorMap[key + lookDir].Type == MapSectorType.None) return key + lookDir;

                randomDirectionLook.Remove(lookDir);
            }


            sectorsToCheck.Remove(key);
        }

        // nothing was found next to a used sector so just get an open one
        index = RandomGenerator.SeededRange(0, openSectors.Count);
        return openSectors[index];
    }

    protected virtual void GetDistanceAndDirection_v2(int2 sectorKey, int2[] lookDirections, ref int distance, ref MapTraversal.MapTraversalDirectionsIndex direction)
    {
        var bestDistCount = -1;
        var bestOverlapCount = -1;
        MapTraversalDirectionsIndex bestdirection = MapTraversalDirectionsIndex.None;

        var randomDirectionLook = new List<int2>();
        foreach (var dir in lookDirections)
        {
            randomDirectionLook.Add(dir);
        }

        while (randomDirectionLook.Count > 0)
        {
            var lookDirIndex = RandomGenerator.SeededRange(0, randomDirectionLook.Count);
            var lookDir = randomDirectionLook[lookDirIndex];

            var currentDistCount = 0;
            var currentOverlapCount = 0;

            var sector = sectorKey + lookDir;
            while (!KeyOutOfBounds(sector))
            {
                currentDistCount++;
                if (sectorMap[sector].Type != MapSectorType.None) currentOverlapCount++;
                
                sector = sector + lookDir;
            }

            currentDistCount = currentDistCount - currentOverlapCount;

            var overlapMultiplier = currentOverlapCount * 3;

            if (currentDistCount > 0)
            {
                if (overlapMultiplier > bestOverlapCount)
                {
                    bestDistCount = currentDistCount;
                    bestOverlapCount = overlapMultiplier;
                    bestdirection = MapTraversal.DirectionToIndex(lookDir);
                }
                else if ((currentDistCount > bestDistCount) && 
                    ((bestOverlapCount < 0) || (currentDistCount > bestOverlapCount)))
                {
                    bestDistCount = currentDistCount;
                    bestOverlapCount = overlapMultiplier;
                    bestdirection = MapTraversal.DirectionToIndex(lookDir);
                }
            }

            randomDirectionLook.Remove(lookDir);
        }

        distance = bestDistCount;
        direction = bestdirection;
    }

    protected virtual RookBuilder GetRookPattern(int2 sectorKey, MapSectorType type, MapTraversal.MapTraversalDirectionsIndex firstMove, MapTraversal.MapTraversalDirectionsIndex secondMove)
    {
        var rookPattern = new RookBuilder();
        rookPattern.Type = type;

        //first move
        var workingKey = sectorKey;
        var distance = rookSize + 1;
        while (distance > 0)
        {

            workingKey += MapTraversal.NeighborsDirectionsAll[(int)firstMove];

            // we will go out of bounds so not valid
            if (KeyOutOfBounds(workingKey)) return null;

            // skip this tile location if already in use
            if (sectorMap[workingKey].Type != MapSectorType.None) continue;

            distance--;
            rookPattern.keys.Add(workingKey);
        }

        //second move
        distance = rookSize;
        while (distance > 0)
        {
            workingKey += MapTraversal.NeighborsDirectionsAll[(int)secondMove];

            // we will go out of bounds so not valid
            if (KeyOutOfBounds(workingKey)) return null;

            // skip this tile location if already in use
            if (sectorMap[workingKey].Type != MapSectorType.None) continue;

            distance--;
            rookPattern.keys.Add(workingKey);
        }

        if (rookPattern.keys.Count != ((rookSize * 2) + 1)) return null;

        return rookPattern;
    }

    protected virtual bool KeyOutOfBounds(int2 key)
    {
        return !sectorMap.ContainsKey(key);
        //return (key.x >= MapSizeX || key.x < 0) || (key.y >= MapSizeY || key.y < 0);
    }
}
