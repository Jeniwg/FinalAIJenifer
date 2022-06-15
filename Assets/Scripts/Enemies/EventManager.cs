using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public delegate void Alarm();
    public static event Alarm alarm;
    public delegate void Forget();
    public static event Forget forget;
    public delegate void UnlockAlarm();
    public static event UnlockAlarm unlockAlarm;

    public static bool go = false;
    public static bool unlock = false;
    public static bool forgetIt = false;

    private void Update()
    {
        //if receiving go start Alarm
        if (go)
        {
            alarm();
            unlock = false;
            go = false;
            
        }

        //if receiving forgetIt start Forget
        if(forgetIt)
        {
            forget();
            forgetIt = false;
        }

        //if receiving unlock start unlockAlarm
        if (unlock)
        {
            unlockAlarm();
            unlock = false;
        }

    }
}
