using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ControlStation : MonoBehaviour
{
    public GameObject uiGameObject;
    public Trap trap;
    private void Awake()
    {
        uiGameObject.SetActive(false);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            uiGameObject.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            uiGameObject.SetActive(false);
        }
    }

    public void ActivateTrap()
    {
        if (trap.isActivated == false)
        {
            trap.isActivated = true;
        }
        else Debug.Log("Trap in Cooldown");
    }

}