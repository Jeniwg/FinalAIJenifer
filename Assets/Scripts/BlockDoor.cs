using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockDoor : MonoBehaviour
{
    [SerializeField]
    private Animator animator;
    public bool CanOpenDoors = true;
    void OnEnable()
    {
        EventManager.alarm += CloseDoors;
        EventManager.unlockAlarm += OpenDoors;
    }

    void OnDisable()
    {
        EventManager.alarm -= CloseDoors;
        EventManager.unlockAlarm -= OpenDoors;
    }

    //When Event Alarm close door, change color to red and player can't open
    private void CloseDoors()
    {
        CanOpenDoors = false;
        gameObject.GetComponentInChildren<Renderer>().material.color = Color.red;
        animator.SetBool("isDoorOpen", false);
    }

    //When Event unlockAlarm player can open doors and door's color change to green
    private void OpenDoors()
    {
        CanOpenDoors = true;
        gameObject.GetComponentInChildren<Renderer>().material.color = Color.green;
    }
}
