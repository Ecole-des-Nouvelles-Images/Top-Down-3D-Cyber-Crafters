using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnclumeTrap : Trap
{
    public Enclume enclumePrefab;
    public Enclume enclume;
    public Transform enclumeSpawnPoint;
    public Animator animator;

    private float coolDown;
    
    [Header("SFX")]
    //public AudioClip activateClip;
    public AudioSource audioSource;

    // private float timer;
    // private float trapDuration = 5f;

    // private void FixedUpdate()
    // {
    //     if (isActivated)
    //     {
    //         if (timer < trapDuration) timer += Time.deltaTime;
    //         else
    //         {
    //             //Lancement de l'animation d'arrêt du piège. 
    //             animator.SetTrigger("stopTrap");
    //             isActivated = false;
    //             enclume = Instantiate(enclumePrefab
    //                 , enclumeSpawnPoint);
    //     
    //             isActivated = true;
    //             timer = 0;
    //         }
    //     }
    // }



    public override void ActivateTrap()
    {
        if (enclume.gameObject == null) return;
        //audioSource.PlayOneShot(activateClip); ( moved on enclume )
       // animator.SetTrigger("activateTrap");
        enclume.DropEnclume();
        Destroy(enclume.gameObject, 2);
        StartCoroutine(InstantiateEnclumeAfterDelay(5f)); // change delay with cooldowntime
    }

    private IEnumerator InstantiateEnclumeAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        enclume = Instantiate(enclumePrefab
            , enclumeSpawnPoint);
        
    }
}