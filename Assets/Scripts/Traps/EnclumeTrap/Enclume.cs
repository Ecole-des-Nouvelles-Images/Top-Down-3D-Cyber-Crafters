using System;
using System.Collections;
using System.Collections.Generic;
using Enemies;
using UnityEngine;

public class Enclume : MonoBehaviour
{
    private float timer = 0;
    public float enclumeDuration = 5f;
    private void FixedUpdate()
    {
        if ( timer < enclumeDuration ) timer += Time.deltaTime;
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Enemy")) {
            other.GetComponent<Enemy>().TakeDamage(1);
        }
    }
}
