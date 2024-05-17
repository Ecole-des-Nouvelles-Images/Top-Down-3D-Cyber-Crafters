using System;
using System.Collections;
using System.Collections.Generic;
using Enemies;
using UnityEngine;

public class Enclume : MonoBehaviour
{
    public int damage = 5;
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

    private void OnCollisionEnter(Collision other)
    {
        if(other.transform.CompareTag("Enemy")) {
            other.gameObject.GetComponent<Enemy>().TakeDamage(damage);
        }
    }
}
