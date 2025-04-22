using Unity.Mathematics;

public class MapTraversal
{
    public enum MapTraversalDirectionsIndex
    {
        Right = 0,
        Left,
        Up,
        Down,
        UpRight,
        UpLeft,
        DownRight,
        DownLeft,
        None
    }

    /// <summary>
    /// All directions around a point
    /// </summary>
    public static readonly int2[] NeighborsDirectionsAll = new[] {
        new int2(1, 0), // Right
        new int2(-1, 0),//Left
        new int2(0, 1), // Up
        new int2(0, -1),// Down
        new int2(1, 1), // diagonal top right
        new int2(-1, 1), // diagonal top left
        new int2(1, -1), // diagonal bottom right
        new int2(-1, -1) // diagonal bottom left
     };

    /// <summary>
    /// Orthogonal direction from point
    /// </summary>
    public static readonly int2[] NeighborsDirectionsOrtho = new[] {
        new int2(1, 0), // Right
        new int2(-1, 0),//Left
        new int2(0, 1), // Up
        new int2(0, -1)// Down
     };

    /// <summary>
    /// Diagonal direction from point
    /// </summary>
    public static readonly int2[] NeighborsDirectionsDiag = new[] {
        new int2(1, 1), // diagonal top right
        new int2(-1, 1), // diagonal top left
        new int2(1, -1), // diagonal bottom right
        new int2(-1, -1) // diagonal bottom left
     };

    public static MapTraversalDirectionsIndex DirectionToIndex(int2 dir)
    {
        if (dir.Equals(new int2(1, 0))) return MapTraversalDirectionsIndex.Right;
        else if (dir.Equals(new int2(-1, 0))) return MapTraversalDirectionsIndex.Left;
        else if (dir.Equals(new int2(0, 1))) return MapTraversalDirectionsIndex.Up;
        else if (dir.Equals(new int2(0, -1))) return MapTraversalDirectionsIndex.Down;

        else if(dir.Equals(new int2(1, 1))) return MapTraversalDirectionsIndex.UpRight;
        else if (dir.Equals(new int2(-1, 1))) return MapTraversalDirectionsIndex.UpLeft;
        else if (dir.Equals(new int2(1, 1))) return MapTraversalDirectionsIndex.DownRight;
        else if (dir.Equals(new int2(-1, -1))) return MapTraversalDirectionsIndex.DownLeft;
        else return MapTraversalDirectionsIndex.None;
    }

    public static bool IsOrthogonal(MapTraversalDirectionsIndex dir)
    {
        return dir == MapTraversalDirectionsIndex.Right || dir == MapTraversalDirectionsIndex.Left || dir == MapTraversalDirectionsIndex.Up || dir == MapTraversalDirectionsIndex.Down;
    }

    public static bool IsDiagonal(MapTraversalDirectionsIndex dir)
    {
        return dir == MapTraversalDirectionsIndex.UpRight || dir == MapTraversalDirectionsIndex.UpLeft || dir == MapTraversalDirectionsIndex.DownRight || dir == MapTraversalDirectionsIndex.DownLeft;
    }

}