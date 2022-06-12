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
        gameObject.GetComponentInChildren<Renderer>().material.color = Color.green;
        EventManager.alarm -= CloseDoors;
        EventManager.unlockAlarm -= OpenDoors;
    }

    private void CloseDoors()
    {
        CanOpenDoors = false;
        gameObject.GetComponentInChildren<Renderer>().material.color = Color.red;
        animator.SetBool("isDoorOpen", false);
    }

    private void OpenDoors()
    {
        CanOpenDoors = true;
        gameObject.GetComponentInChildren<Renderer>().material.color = Color.green;
    }
}
