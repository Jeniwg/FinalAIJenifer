using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public delegate void Alarm();
    public static event Alarm alarm;
    [SerializeField]
    public static bool go = false;

    private void Update()
    {
        if (go)
        {
            Debug.Log(go);
            alarm();
            go = false;
        }
        Debug.Log(go);

    }
}
