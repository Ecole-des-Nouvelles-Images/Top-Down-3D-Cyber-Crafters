using System;
using UnityEngine;

namespace Proto.Script.Trap
{
    public class Bullet : MonoBehaviour
    {
        public GameObject particleExplosion;
        public int damage = 1;
        public float speed = 10f; // Ajoutez une vitesse à la balle
        private float _timer;

        private void Update()
        {
            _timer += Time.deltaTime;
            if (_timer >= 2.5f)
            {
                Destroy(gameObject);
            }

            // Déplacez la balle en fonction de sa vitesse
            transform.position += transform.forward * (speed * Time.deltaTime);
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.CompareTag("Enemy"))
            {
                other.GetComponent<Enemies.Enemy>().TakeDamage(damage);
                Destroy(gameObject);
            }
            //Destroy(gameObject);
        }

        private void OnTriggerEnter(Collider other)
        {
            Debug.Log("Bullet collided with: " + other.gameObject.name + other.transform.parent.name);
            if (!other.CompareTag("DangerZone"))
            {
                Destroy(gameObject);
            }
        }

        private void OnDestroy()
        {
            Instantiate(particleExplosion, transform.position, transform.rotation);
        }
    }
}