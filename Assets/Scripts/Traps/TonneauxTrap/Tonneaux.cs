using System;
using System.Collections;
using System.Collections.Generic;
using Enemies;
using UnityEngine;

public class Tonneaux : MonoBehaviour
{
    public float force = 5f;
    public int damage = 1;
    public float stunDuration = 2f;
    
    public float timeBeforeDestroy = 2f;
    private float timer;

    public GameObject brokenBarrel;
    

    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.AddForce(transform.right * force, ForceMode.VelocityChange);
    }

    private void Update()
    {
        if (timer >= timeBeforeDestroy)
        {
            Instantiate(brokenBarrel, transform.position, Quaternion.Euler(0, 0, 90));
            Destroy(gameObject);
        }
        timer += Time.deltaTime;
        
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.transform.CompareTag("Enemy"))
        {
            Enemy enemy = other.gameObject.GetComponent<Enemy>();
            enemy.TakeDamage(damage);
            enemy.Stun(stunDuration);
            Instantiate(brokenBarrel, transform.position, Quaternion.Euler(0, 0, 90));
            Destroy(gameObject);
        }
    }
    
    
    // Détruire tonneaux avec animation
}