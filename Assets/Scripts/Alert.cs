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

    private bool isInAlert = false;
    void Start()
    {
        alert.enabled = false;
    }

    void OnEnable()
    {
        EventManager.alarm += AlternColor;
        EventManager.forget += StopAlert;
    }

    void OnDisable()
    {
        EventManager.alarm -= AlternColor;
        EventManager.forget -= StopAlert;
    }

    private void AlternColor()
    {
        isInAlert = true;
        alert.enabled = true;
        animator.SetBool("Alert", true);
    }

    private void StopAlert()
    {
        isInAlert = false;
        alert.enabled = false;
        animator.SetBool("Alert", false);
    }
}
