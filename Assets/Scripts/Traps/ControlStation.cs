using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ControlStation : MonoBehaviour
{
    public MeshRenderer buttonMeshRenderer;
    public Animator animator;
    public GameObject dangerZone;
    public bool alreadyPressed;
    private float _cooldownTime = 5f;
    private float _timer;

    public Trap trap;
    

    private void Awake()
    {
        // dangerZone.SetActive(false);
    }

    private void Update()
    {
        if (alreadyPressed)
        {
            _timer += Time.deltaTime;
        }

        // Verifie que le piege n'est plus actif. 
        if (_timer >= _cooldownTime && !trap.isActivated)
        {
            animator.SetBool("isActivated", false);
            buttonMeshRenderer.material.color = Color.red;
            alreadyPressed = false;
           // dangerZone.SetActive(false);
            
           _timer = 0;
        }
    }

    public void TriggerTrap()
    {
        if (!alreadyPressed)
        {
            trap.ActivateTrap();
            buttonMeshRenderer.material.color = Color.green;
            animator.SetBool("isActivated", true);
            alreadyPressed = true;
            _timer = 0;
        }
    }
}