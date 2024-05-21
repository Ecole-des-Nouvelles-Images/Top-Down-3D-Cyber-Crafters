using System;
using System.Collections.Generic;
using Enemies;
using UnityEngine;

public class TrappeTrap : Trap
{
    public Animator animator;
    private float timer;
    public float trapDuration;
    // Les enemis positionnés sur la trappe tombent du train et trigger une zone de mort.

    // Supprimer le sol du train et le remplacer par celui de la trappe ? 
    // Ajouter un triggered collider sous le train pour lancer mort de l'ennemi.
    
    //Le piege ne se ferme pas des fois ?
    
    [Header("SFX")]
    public AudioClip trapStartSound;

    public AudioClip trapStopSound;
    public AudioSource audioSource;

    private bool isActivating= false;

    private void FixedUpdate()
    {
        if (isActivated)
        {
            if (timer < trapDuration) timer += Time.deltaTime;
            else
            {
                //Animation de l'arrêt du piège ( AnimationEvent dans le clip appelle stopTrap() )
                animator.SetTrigger("stopTrap");
                isActivated = false;
            }
        }
    }

    public override void ActivateTrap()
    {
        if (isActivating)
        {
            Debug.Log($"ActivateTrap : Trap is already activating.");
            return;
        }
        Debug.Log("ActivateTrap is setting isActivating to true");

        isActivating = true;
        audioSource.PlayOneShot(trapStartSound);
        animator.SetTrigger("startTrap");
        playTrap();
        // Changer playTrap() par animation et ajouter animationevent à la fin de l'anim.
    }

    private void playTrap()
    {
        isActivated = true;
        timer = 0;
    }

    // Appelé par l'Event de l'animator à la fin de l'arrêt du piège.
    private void stopTrap()
    {
        audioSource.PlayOneShot(trapStopSound);
        isActivating = false;

    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") && isActivated)
        {
            Enemy enemy = other.GetComponent<Enemy>();
            enemy.healthPoints = 0;
            // enemy.Fall();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Enemy") && isActivated)
        {
            Enemy enemy = other.GetComponent<Enemy>();
            enemy.healthPoints = 0;
            // enemy.Fall();
        }
    }
}