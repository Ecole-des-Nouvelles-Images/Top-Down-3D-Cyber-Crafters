using System.Collections.Generic;
using Enemies;
using UnityEngine;

public class VentiloTrap : Trap
{
    public ParticleSystem ventiloParticleSystem;
    public Animator animator;

    [HeaderAttribute("SFX")]
    public AudioClip ventiloStartSound;

    public AudioClip ventiloRunningSound;
    public AudioClip ventiloStopSound;
    public AudioSource audioSource;
    
    public float trapDuration;
    private float timer;

    private List<Enemy> _slowedEnemies = new List<Enemy>();

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
                audioSource.Stop();
                audioSource.PlayOneShot(ventiloStopSound);
            }
        }
    }
    public override void ActivateTrap()
    {
        // Animation de mise en place du piège.
        animator.SetTrigger("activateTrap");
        audioSource.PlayOneShot(ventiloStartSound);
    }

    // Appelé par l'Event de l'animator à la fin de la mise en route du piège. 
    private void playTrap()
    {
        isActivated = true;
        timer = 0;
        ventiloParticleSystem.Play();
        audioSource.clip = ventiloRunningSound;
        audioSource.loop = true;
        audioSource.Play();
    }

    // Appelé par l'Event de l'animator à la fin de l'arrêt du piège.
    private void stopTrap()
    {
        foreach (Enemy enemy in _slowedEnemies)
        {
            enemy.ResetSpeed();
            _slowedEnemies.Remove(enemy);
        }
        ventiloParticleSystem.Stop();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") && isActivated)
        {
            Enemy enemy = other.GetComponent<Enemy>();
            enemy.SlowDown();
            _slowedEnemies.Add(enemy);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Enemy") & isActivated)
        {
            other.GetComponent<Enemy>().SlowDown();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Enemy enemy = other.GetComponent<Enemy>();
            enemy.ResetSpeed();
            _slowedEnemies.Remove(enemy);
        }
    }

    private void OnDisable()
    {
        // foreach (Enemy enemy in _slowedEnemies)
        // {
        //     enemy.ResetSpeed();
        //     _slowedEnemies.Remove(enemy);
        // }
    }
}


// Piege s'active même sans appuyer sur A ?
// l'animation de mise en place du piege ne se lance pas
// l'animation de retrait du piege ne se finit pas
// le ventilateur ne doit pas disparaitre, seulement la DangerZone. 