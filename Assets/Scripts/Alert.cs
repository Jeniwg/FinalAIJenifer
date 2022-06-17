using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Alert : MonoBehaviour
{
    [SerializeField]
    private Image alert;
    [SerializeField]
    private Animator animator;

    void Start()
    {
        alert.enabled = false;
    }

    void OnEnable()
    {
        EventManager.alarm += AlternColor;
        EventManager.unlockAlarm += StopAlert;
    }

    void OnDisable()
    {
        EventManager.alarm -= AlternColor;
        EventManager.unlockAlarm -= StopAlert;
    }

    //When Event Alarm start animation of redlights
    private void AlternColor()
    {
        alert.enabled = true;
        animator.SetBool("Alert", true);
    }

    //When Event forget end animation of redlights
    private void StopAlert()
    {
        alert.enabled = false;
        animator.SetBool("Alert", false);
    }
}
