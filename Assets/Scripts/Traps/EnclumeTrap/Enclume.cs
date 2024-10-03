using System;
using Enemies;
using UnityEngine;
using UnityEngine.Serialization;

public class Enclume : MonoBehaviour
{
    public int damage = 5;
    public float enclumeDuration = 2f;
    [FormerlySerializedAs("slowDownDuration")] public float stunDuration = 2f;

    public AudioClip fallClip;
    public AudioClip bongGroundClip;
    public AudioClip bongEnemyClip;
    public AudioSource audioSource;

    private bool _hasLanded = false;

    public bool isDropped = false; 
    public Rigidbody rb;

    public Collider col;
    

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
        audioSource.PlayOneShot(fallClip);
        isDropped = true;
        rb.isKinematic = false;
        col.providesContacts = true;
        transform.SetParent(null);
        //rb.useGravity = true;
        Destroy(gameObject, enclumeDuration);
    }
}