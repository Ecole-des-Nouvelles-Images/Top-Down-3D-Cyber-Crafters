using System;
using Enemies;
using UnityEngine;
using UnityEngine.Serialization;

public class Enclume : MonoBehaviour
{
    public int damage = 5;
    private float timer = 0;
    public float enclumeDuration = 5f;
    [FormerlySerializedAs("slowDownDuration")] public float stunDuration = 2f;

    public AudioClip bongGroundClip;
    public AudioClip bongEnemyClip;
    public AudioSource audioSource;

    private bool _hasLanded = false;

    public bool isDropped = false; 
    public Rigidbody rb;
    

    private void FixedUpdate()
    {
        //if (isDropped)
        //{
        //    if (timer < enclumeDuration) timer += Time.deltaTime;
        //    else
        //    {
        //        Destroy(gameObject);
        //    }
        //}
    }

    private void OnCollisionEnter(Collision other)
    {
        if (_hasLanded) return;
        if (other.transform.CompareTag("Enemy"))
        {
            audioSource.PlayOneShot(bongEnemyClip);
            Enemy enemy = other.gameObject.GetComponent<Enemy>();
            enemy.TakeDamage(damage);
            enemy.Stun(stunDuration);
            
        }
        else audioSource.PlayOneShot(bongGroundClip);

        _hasLanded = true;
    }
    
    public void DropEnclume()
    {
        isDropped = true;
        rb.useGravity = true;
    }
}