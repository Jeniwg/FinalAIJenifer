using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public delegate void Alarm();
    public static event Alarm alarm;
    public static event Alarm forget;
    public static event Alarm unlockAlarm;

    //Start Alarm
    public static void Go()
    {
        alarm();
    }

    //Forget Player
    public static void ForgetIt()
    {
        forget();
    }

    //UnlockAlarm
    public static void Unlock()
    {
        unlockAlarm();
    }
}
