using UnityEngine;

public class DateToInt : MonoBehaviour
{
    public static int GetDateInt()
    {
        return System.DateTime.Now.Year + System.DateTime.Now.Month + System.DateTime.Now.Day;
    }
}
