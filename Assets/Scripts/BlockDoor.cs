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
    }

    void OnDisable()
    {
        gameObject.GetComponentInChildren<Renderer>().material.color = Color.green;
        EventManager.alarm -= CloseDoors;
    }

    private void Update()
    {
        if(CanOpenDoors == true)
        {
            gameObject.GetComponentInChildren<Renderer>().material.color = Color.green;
        }
    }

    private void CloseDoors()
    {
        CanOpenDoors = false;
        gameObject.GetComponentInChildren<Renderer>().material.color = Color.red;
        animator.SetBool("isDoorOpen", false);
    }
}
