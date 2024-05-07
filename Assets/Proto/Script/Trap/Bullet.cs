using System;
using UnityEngine;

namespace Proto.Script.Trap
{
    public class Bullet : MonoBehaviour
    {
        public GameObject particleExplosion;
        private float _timer;

        private void Update()
        {
            _timer += Time.deltaTime;
            if (_timer >= 3.5f)
            {
                Destroy(gameObject);
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.CompareTag("Enemy"))
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
