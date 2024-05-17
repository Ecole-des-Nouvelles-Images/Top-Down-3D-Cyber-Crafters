using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnclumeTrap : Trap
{
    public GameObject enclume;
    public Transform enclumeSpawnPoint;
    public Animator animator;
    
    [Header("SFX")]
    public AudioClip activateClip;
    public AudioSource audioSource;

    private float timer;
    private float trapDuration = 5f;

    private void FixedUpdate()
    {
        if (isActivated)
        {
            if (timer < trapDuration) timer += Time.deltaTime;
            else
            {
                //Lancement de l'animation d'arrêt du piège. 
                animator.SetTrigger("stopTrap");
                isActivated = false;
            }
        }
    }

    public override void ActivateTrap()
    {
        audioSource.PlayOneShot(activateClip);
        animator.SetTrigger("activateTrap");
        Instantiate(enclume, enclumeSpawnPoint);
        isActivated = true;
        timer = 0;
    }
}