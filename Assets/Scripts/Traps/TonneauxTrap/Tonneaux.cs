using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tonneaux : MonoBehaviour
{
    public float force = 10f;

    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.AddForce(-transform.forward * force, ForceMode.VelocityChange);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<Enemies.Enemy>().TakeDamage(1);
        }
    }
    // Détruire tonneaux avec animation
}
