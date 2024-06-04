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
    private bool isDropped = false;

    public GameObject brokenBarrel;


    public Rigidbody rb;
    public Collider col;

    private void Start()
    {
        
    }

    private void Update()
    {
        if (isDropped)
        {
            if (timer >= timeBeforeDestroy)
            {
                Instantiate(brokenBarrel, transform.position, Quaternion.Euler(0, 0, 90));
                Destroy(gameObject);
            }

            timer += Time.deltaTime;
        }
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

    public void DropBarrel()
    {
        isDropped = true;
        rb.isKinematic = false;
        rb.AddForce(transform.right * force, ForceMode.VelocityChange);
        col.providesContacts = true;
        transform.SetParent(null);
    }


    // Détruire tonneaux avec animation
}