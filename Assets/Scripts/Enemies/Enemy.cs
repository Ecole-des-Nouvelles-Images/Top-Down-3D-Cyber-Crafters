using System.Collections;
using System.Collections.Generic;
using Train;
using Train.Wagon;
using UnityEngine;
using UnityEngine.AI;

namespace Enemies
{
    public class Enemy : MonoBehaviour
    {
        [Header("Variables Globales")] [SerializeField]
        private EnemiesManager parentManager; // Manager

        public SteamPipe targetedSteamPipe; // SteamPipe visé Par L'Ennemi

        [Header("Variables Locales")] public NavMeshAgent navMeshAgent;

        public bool
            inTrain; // Pour vérifier si l'Ennemi est présent à bord du Train (Pour des Manipulations Physiques en rapport avec le Spawn)

        public int healthPoints;
        public int attackPoints;
        public float speed;
        public List<GameObject> Models = new List<GameObject>();

        public enum EnemyType
        {
            Tank,
            Fast,
            Neutral
        }

        [SerializeField] public EnemyType enemyType;
        public Animator _animator;


        private void Awake()
        {
            parentManager = transform.parent.GetComponent<EnemiesManager>();
            parentManager.AddChild(this);
            navMeshAgent = GetComponent<NavMeshAgent>();
            navMeshAgent.destination = FindObjectOfType<SteamPipe>().gameObject.transform.position;
        }

        private void Start()
        {
            switch (enemyType)
            {
                case EnemyType.Tank:
                {
                    Vector3 position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
                    GameObject model = Instantiate(Models[0], position, transform.rotation, transform);
                    _animator = model.GetComponentInChildren<Animator>();
                    healthPoints = 30 / 5;
                    attackPoints = 15;
                    navMeshAgent.speed = 0.75f;
                    break;
                }
                case EnemyType.Fast:
                {
                    Vector3 position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
                    GameObject model = Instantiate(Models[1], position, transform.rotation, transform);
                    _animator = model.GetComponentInChildren<Animator>();
                    healthPoints = 10 / 5;
                    attackPoints = 5;
                    navMeshAgent.speed = 2f;
                    break;
                }
                case EnemyType.Neutral:
                {
                    Vector3 position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
                    GameObject model = Instantiate(Models[2], position, transform.rotation, transform);
                    _animator = model.GetComponent<Animator>();
                    healthPoints = 25 / 5;
                    attackPoints = 10;
                    navMeshAgent.speed = 1f;
                    break;
                }
            }
        }
        
         private void Update()
        {
           if (healthPoints <= 0)
           {
               Die();
           }
        }

        private void FixedUpdate()
        {
            if (navMeshAgent.speed > 0)
            {
                _animator.SetBool("Walk", true);
            }
            else
            {
                _animator.SetBool("Walk", false);
            }
        }

        private void OnDestroy()
        {
            FindObjectOfType<TrainManager>().scrapsCount += 1;
            parentManager.enemies.Remove(this);
            if (targetedSteamPipe != null)
            {
                targetedSteamPipe.assignedEnemies.Remove(this);
            }

            parentManager.SortEnemies();
        }

        public void AddSteamPipe(SteamPipe steamPipe)
        {
            targetedSteamPipe = steamPipe;
            navMeshAgent.destination = targetedSteamPipe.gameObject.transform.position;
        }

        //DEBUG COLLISIONS
        private void OnCollisionEnter(Collision collision)
        {
            //Debug.Log("Enemy collided with: " + collision.gameObject.name);
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.CompareTag("Steampipe"))
            {
                other.GetComponent<SteamPipe>().healthPoints -= attackPoints;
                navMeshAgent.isStopped = true;
                _animator.SetBool("Attack", true);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Steampipe"))
            {
                navMeshAgent.isStopped = false;
                _animator.SetBool("Attack", false);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            //throw new NotImplementedException();
        }

        public void TakeDamage(int damage)
        {
            healthPoints -= damage;
            _animator.SetTrigger("Hit");
            if (healthPoints <= 0)
            {
                Die();
            }
        }

        public void Fall()
        {
            _animator.SetBool("Fall", true);
        }

        public void Stun(float stunDuration)
        {
            if (navMeshAgent.isStopped) return;
            navMeshAgent.isStopped = true;
            //animate Stun
            _animator.SetBool("Stun", true);
            StartCoroutine(ResetStun(stunDuration));
        }

        public void SlowDown()
        {
            if (navMeshAgent.isStopped) return;
            if (enemyType == EnemyType.Tank) return;
            navMeshAgent.isStopped = true;
            //Animate SlowDown
            _animator.SetBool("Slow", true);
        }

        public IEnumerator ResetStun(float stunDuration)
        {
            yield return new WaitForSeconds(stunDuration);
            navMeshAgent.isStopped = false;
            _animator.SetBool("Stun", false);
        }

        public void ResetSpeed()
        {
            if (_animator == null)
            {
                Debug.Log($"animator of enemy {gameObject.name} is null");
                return;
            }

            _animator.SetBool("Slow", false);
            navMeshAgent.isStopped = false;
        }

        public void Die()
        {
            //Animation de mort
            _animator.SetTrigger("Die");
            _animator.SetBool("Walk", false);
            //Désactiver le NavMeshAgent
            navMeshAgent.isStopped = true;
        }

        public void DestroyEnemy()
        {
            Destroy(gameObject);
        }
    }
}