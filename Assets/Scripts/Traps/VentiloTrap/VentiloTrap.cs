using System;
using System.Collections;
using System.Collections.Generic;
using Enemies;
using Unity.VisualScripting;
using UnityEngine;

public class VentiloTrap : Trap
{
    public ParticleSystem ventiloParticleSystem;
    public Animator animator;
    
    public float trapDuration;
    private float timer;

    private List<Enemy> _slowedEnemies = new List<Enemy>();

    void Awake()
    {
        timer = 0;
        ventiloParticleSystem.Play();
    }

    private void FixedUpdate()
    {
        if ( timer < trapDuration ) timer += Time.deltaTime;
        else
        {
            animator.SetTrigger("stopTrap");
            ventiloParticleSystem.Stop();
        }
    }

    // Appelé par l'Event de l'animator à la fin de l'animation.
    private void stopTrap()
    {
        gameObject.SetActive(false);
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Enemy enemy = other.GetComponent<Enemy>();
            enemy.SlowDown();
            _slowedEnemies.Add(enemy);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Enemy"))
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
        foreach (Enemy enemy in _slowedEnemies)
        {
            enemy.ResetSpeed();
            _slowedEnemies.Remove(enemy);
        }
    }
}
