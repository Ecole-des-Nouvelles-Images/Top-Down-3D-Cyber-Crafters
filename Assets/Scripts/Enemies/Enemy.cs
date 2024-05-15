using System;
using System.Collections.Generic;
using Train;
using Train.Wagon;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

namespace Enemies {
    public class Enemy : MonoBehaviour
    {
        
        
        [Header("Variables Globales")] [SerializeField]
        private EnemiesManager parentManager; // Manager
        public SteamPipe targetedSteamPipe; // SteamPipe visé Par L'Ennemi

        [Header("Variables Locales")] 
        public NavMeshAgent navMeshAgent;
        public bool inTrain; // Pour vérifier si l'Ennemi est présent à bord du Train (Pour des Manipulations Physiques en rapport avec le Spawn)
        public int healthPoints;
        public int attackPoints;
        public int speed;
        public List<GameObject> Models = new List<GameObject>();
        public enum EnemyType {
            Tank,
            Fast,
            Neutral
        }
        [SerializeField] public EnemyType enemyType;
        private Animator _animator;

        private void Awake() {
            parentManager = transform.parent.GetComponent<EnemiesManager>();
            parentManager.AddChild(this);
            navMeshAgent = GetComponent<NavMeshAgent>();
            navMeshAgent.destination = FindObjectOfType<SteamPipe>().gameObject.transform.position;
        }

        private void Start() {
            switch (enemyType) {
                case EnemyType.Tank:
                {
                    Vector3 position = new Vector3(transform.position.x, transform.position.y + 1.25f, transform.position.z);
                    GameObject model = Instantiate(Models[0], position, transform.rotation, transform);
                    _animator = model.GetComponent<Animator>();
                    healthPoints = 30;
                    attackPoints = 15;
                    speed = 2;
                    model.GetComponent<MeshRenderer>().material.color = Color.red;
                    break;
                }
                case EnemyType.Fast: {
                    Vector3 position = new Vector3(transform.position.x, transform.position.y + 0.75f, transform.position.z);
                    GameObject model = Instantiate(Models[1], position, transform.rotation, transform);
                    _animator = model.GetComponent<Animator>();
                    healthPoints = 10;
                    attackPoints = 5;
                    speed = 6;
                    model.GetComponent<MeshRenderer>().material.color = Color.yellow;
                    break;
                }
                case EnemyType.Neutral: {
                    Vector3 position = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
                    GameObject model = Instantiate(Models[2], position, transform.rotation, transform);
                    _animator = model.GetComponent<Animator>();
                    healthPoints = 25;
                    attackPoints = 10;
                    speed = 4;
                    model.GetComponent<MeshRenderer>().material.color = Color.green;
                    break;
                }
            }
        }

        private void Update() {
            if (healthPoints <= 0) { Destroy(gameObject); }
        }

        private void OnDestroy() {
            FindObjectOfType<TrainManager>().scrapsCount += 1;
            parentManager.enemies.Remove(this);
            targetedSteamPipe.assignedEnemies.Remove(this);
            parentManager.SortEnemies();
        }

        public void AddSteamPipe(SteamPipe steamPipe) {
            targetedSteamPipe = steamPipe;
            navMeshAgent.destination = targetedSteamPipe.gameObject.transform.position;
        }

        //DEBUG COLLISIONS
        private void OnCollisionEnter(Collision collision)
        {
            Debug.Log("Enemy collided with: " + collision.gameObject.name);
        }
        private void OnTriggerStay(Collider other)
        {
            //throw new NotImplementedException();
        }

        private void OnTriggerExit(Collider other)
        {
            //throw new NotImplementedException();
        }

        private void OnTriggerEnter(Collider other)
        {
            //throw new NotImplementedException();
        }

        public void TakeDamage(int damage) {
            healthPoints -= damage;
            if (healthPoints <= 0) { Destroy(gameObject); }
        }

        public void SlowDown()
        {
            
        }

        public void ResetSpeed()
        {
            
        }
    }
}
