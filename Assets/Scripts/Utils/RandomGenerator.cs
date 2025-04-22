using System;
using System.Text;
using UnityEngine;

public class RandomGenerator : MonoBehaviour
{
    public string Seed;
    private static string suppliedSeed;
    private static int currentSeed;
    protected static System.Random random;

    void Awake()
    {
        SetSeed(Seed);
    }

    public static string GetSuppliedSeed()
    {
        return suppliedSeed;
    }
    public static int GetCurrentSeed()
    {
        return currentSeed;
    }

    public static void SetSeed(string newSeed)
    {
        currentSeed = GetSeed(newSeed);
        suppliedSeed = newSeed;
        random = new System.Random();
        UnityEngine.Random.InitState(currentSeed);
    }

    public static void SetSeed(int newSeed)
    {
        currentSeed = newSeed;
        suppliedSeed = newSeed.ToString();
        random = new System.Random();
        UnityEngine.Random.InitState(currentSeed);
    }

    /// <summary>
    /// Seeded value between Min (inclusive) Max (Exclusive)
    /// </summary>
    /// <param name="Min"></param>
    /// <param name="Max"></param>
    /// <returns></returns>
    public static int SeededRange(int Min, int Max)
    {
        return UnityEngine.Random.Range(Min, Max);
    }

    /// <summary>
    /// Seeded value between Min (inclusive) Max (Exclusive)
    /// </summary>
    /// <param name="Min"></param>
    /// <param name="Max"></param>
    /// <returns></returns>
    public static float SeededRange(float Min, float Max)
    {
        return UnityEngine.Random.Range(Min, Max);
    }

    /// <summary>
    /// UnSeeded value between Min (inclusive) Max (Exclusive)
    /// </summary>
    /// <param name="Min"></param>
    /// <param name="Max"></param>
    /// <returns></returns>
    public static int UnseededRange(int Min, int Max)
    {
        return random.Next(Min, Max);
    }
    /// <summary>
    /// UnSeeded value between Min (inclusive) Max (Exclusive)
    /// </summary>
    /// <param name="Min"></param>
    /// <param name="Max"></param>
    /// <returns></returns>
    public static float UnseededRange(float Min, float Max)
    {
        return ((float)(random.Next((int)(Min * 1000), (int)(Max * 1000)))) / 1000;
    }
    /// <summary>
    /// Seeded Random Bool value
    /// </summary>
    /// <returns></returns>
    public static bool SeededRandomBool()
    {
        return SeededRange(0, 100) >= 50;
    }
    /// <summary>
    /// UnSeeded Random Bool value
    /// </summary>
    /// <returns></returns>
    public static bool UnseededRandomBool()
    {
        return UnseededRange(0, 100) >= 50;
    }

    /// <summary>
    /// Seeded value between Min (inclusive) Max (Exclusive)
    /// </summary>
    /// <param name="Min"></param>
    /// <param name="Max"></param>
    /// <returns></returns>
    public static bool SeededIsPercentage(float Percentage)
    {
        return SeededRange(0f, 100f) < Percentage;
    }
    /// <summary>
    /// UnSeeded value between Min (inclusive) Max (Exclusive)
    /// </summary>
    /// <param name="Min"></param>
    /// <param name="Max"></param>
    /// <returns></returns>
    public static bool UnseededIsPercentage(float Percentage)
    {
        return UnseededRange(0f, 100f) < Percentage;
    }

    #region GenerationSeed
    protected static int GetSeed(string seedValue)
    {
        byte[] bytes = Encoding.ASCII.GetBytes(seedValue);
        int retval = 0;
        foreach (var b in bytes)
        {
            retval += Convert.ToInt32(b);
        }
        return retval;
    }
    #endregion

}