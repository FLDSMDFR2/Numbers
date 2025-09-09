using System;
using System.Collections.Generic;
using UnityEngine;

public class PuzzlesPerLevel : MonoBehaviour
{
    [Serializable]
    public class PuzzlesPerLevelDtls
    {
        public int Levels;
        public int PuzzleCount;
    }

    [SerializeField]
    public List<PuzzlesPerLevelDtls> PuzzlesPerLevelMap = new List<PuzzlesPerLevelDtls> ();
    protected static List<PuzzlesPerLevelDtls> puzzlesPerLevelMap = new List<PuzzlesPerLevelDtls>();
    protected static Dictionary<int,int> levelToPuzzleCount = new Dictionary<int, int>();

    protected virtual void Awake()
    {
        puzzlesPerLevelMap.AddRange(PuzzlesPerLevelMap);

        var level = 0;
        foreach(var map in puzzlesPerLevelMap)
        {
            for (int i = 0; i < map.Levels; i++)
            {
                level++;
                levelToPuzzleCount[level] = map.PuzzleCount;
            }
        }
    }


    public static long GetLevelByCompletedPuzzles(long completedPuzzles)
    {
        long level = 0;
        foreach (var plm in puzzlesPerLevelMap)
        {
            for (int i = 0; i < plm.Levels; i++)
            {
                completedPuzzles -= plm.PuzzleCount;

                if (completedPuzzles <= 0) break;
                level++;
            }
            if (completedPuzzles <= 0) break;
        }

        return level+1;
    }

    public static long GetPuzzlesCompletedWithinLevel(long completedPuzzles)
    {
        var level = GetLevelByCompletedPuzzles(completedPuzzles);
        var count = 0;
        var levelCount = 0;
        foreach (var plm in puzzlesPerLevelMap)      
        {
            for (int i = 0; i < plm.Levels; i++)
            {
                levelCount++;
                if (levelCount == level) break;

                count += plm.PuzzleCount;
            }
            if (levelCount == level) break;
        }

        return completedPuzzles - count;
    }

    public static long GetMaxPuzzleCountForLevel(int level)
    {
        if (levelToPuzzleCount.ContainsKey(level)) return levelToPuzzleCount[level];

        return 0;
    }

    public static long GetTotalNumberComplete(int level, int Puzzle)
    {
        var puzzleCount = 0;
        for(int i = 1; i < level; i++)
        {
            puzzleCount += levelToPuzzleCount[i];
        }

        puzzleCount += Puzzle;

        return puzzleCount;
    }

}
