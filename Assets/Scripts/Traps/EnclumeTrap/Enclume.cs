using Enemies;
using UnityEngine;

public class Enclume : MonoBehaviour
{
    public int damage = 5;
    private float timer = 0;
    public float enclumeDuration = 5f;
    public float slowDownDuration = 2f;

    public AudioClip bongGroundClip;
    public AudioClip bongEnemyClip;
    public AudioSource audioSource;

    private bool _hasLanded = false;
    private void FixedUpdate()
    {
        if (timer < enclumeDuration) timer += Time.deltaTime;
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (_hasLanded) return;
        if (other.transform.CompareTag("Enemy"))
        {
            audioSource.PlayOneShot(bongEnemyClip);
            Enemy enemy = other.gameObject.GetComponent<Enemy>();
            enemy.TakeDamage(damage);
            enemy.SlowDown(slowDownDuration);
            
        }
        else audioSource.PlayOneShot(bongGroundClip);

        _hasLanded = true;
    }
}