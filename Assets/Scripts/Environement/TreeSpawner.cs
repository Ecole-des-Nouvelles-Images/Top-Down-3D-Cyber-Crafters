using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;


namespace Environement
{
    public class TreeSpawner : MonoBehaviour
    {
        public GameObject treePrefab;
        public bool canSpawn;

        private void Awake()
        {
            canSpawn = true;
        }

        private void FixedUpdate()
        {
            int spawnRate = Random.Range(0, 100);
            Vector3 randomPos = new Vector3(transform.position.x, transform.position.y * Random.Range(-1f, 1f) - 1.5f,
                transform.position.z);
            if (spawnRate == 10 && canSpawn)
            {
                Instantiate(treePrefab, randomPos, quaternion.identity, transform);
            }
        }
    }
}
