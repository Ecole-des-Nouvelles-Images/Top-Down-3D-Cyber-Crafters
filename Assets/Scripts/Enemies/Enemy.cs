using System;
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

        [Header("Variables Locales")] 
        public NavMeshAgent navMeshAgent;
        public bool inTrain; // Pour vérifier si l'Ennemi est présent à bord du Train (Pour des Manipulations Physiques en rapport avec le Spawn)
        public int healthPoints;

        private void Awake()
        {
            throw new NotImplementedException();
        }

        private void Start()
        {
            throw new NotImplementedException();
        }

        private void Update()
        {
            throw new NotImplementedException();
        }

        private void OnDestroy()
        {
            throw new NotImplementedException();
        }

        public void AddSteamPipe(SteamPipe steamPipe) {
            targetedSteamPipe = steamPipe;
            navMeshAgent.destination = targetedSteamPipe.gameObject.transform.position;
        }

        private void OnTriggerStay(Collider other)
        {
            throw new NotImplementedException();
        }

        private void OnTriggerExit(Collider other)
        {
            throw new NotImplementedException();
        }

        private void OnTriggerEnter(Collider other)
        {
            throw new NotImplementedException();
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
