using System;
using System.Collections;
using System.Collections.Generic;
using Manager;
using Train.Wagon;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

namespace Enemies
{
    public class EnemiesManager : MonoBehaviour
    {
        [Header("Objets Globaux")] 
        public SteamPipeManager currentSteamPipeManager; // Steam Pipe Manager du Wagon Actuel
        public Transform enemiesSpawnAnchor; // Point de Spawn des Ennemis hors du Train

        [Header("Variables Globales")] 
        public int currentWave; // Numéro de la Vague Actuelle
        public int lastWave; // Permet de Vérifier si il faut incrémenter le nombre d'ennemis
        public int enemiesNumber; // Nombre d'ennemis pour la vague actuelle (S'incrémente toutes les Vagues à voir dans la logique)
        public int tankNumber;
        public int fastNumber;
        public int neutralNumber;
        public bool spawnEnemies; // Booléen permettant de lancer le spawn des Ennemis
        //private float _spawnDelay; // Timer de spawn des Ennemis;
        private bool _areEnemiesTeleported;
        private bool _spawnTimerOn; // Booléen d'activation du Timer
        private bool _firstWave; // Booléen pour vérifier si il s'agit de la première vague
        private float _firstWaveTimer; // Timer du Spawn de la Première Vague
        public bool maxEnemiesSpawned; // Permet de stopper le spawn des Ennemis lorsque enemiesNumber est atteint
        public GameObject finishedWaveIndicator;
        public float waveInterval = 10;// Objet d'UI pour montrer qu'une Vague est finie
        
        [Header("Objets Locaux")] 
        public List<Enemy> enemies = new List<Enemy>(); // Ennemis Enfants en Vie
        private List<Enemy> _teleportedEnemies = new List<Enemy>(); // Ennemis téléportés dans le train (Pour le Spawn)
        public GameObject enemyPrefab; // Prefab d'Ennemi (A changer en Liste quand on aura trois types d'Ennemis)

        private void Start() {
            // Initialisation des variables et Récupération du SPM
            spawnEnemies = false;
            //_spawnDelay = 0;
            currentSteamPipeManager = FindObjectOfType<SteamPipeManager>();
            _firstWave = true;
            finishedWaveIndicator.SetActive(false);
        }

        [ContextMenu("Star next Wave")] // Méthode d'initialisation d'une Wave (Avec l'incrément)
        public void StartWave()
        {
            _areEnemiesTeleported = false;
            _teleportedEnemies.Clear();
            maxEnemiesSpawned = false;
            currentWave += 1;
            if (lastWave < currentWave) {
                if (_firstWave) {
                    enemiesNumber += 9;
                    tankNumber += 3;
                    neutralNumber += 3;
                    fastNumber += 3;
                    lastWave += 1; 
                }
                else {
                    enemiesNumber += 3;
                    tankNumber += 1;
                    neutralNumber += 1;
                    fastNumber += 1;
                    lastWave += 1; 
                }
                
            } // Incrémentation de la vague et du Nombre d'Ennemis à Spawner
            spawnEnemies = true;
        }
        
        public void CreateEnemy(Enemy.EnemyType enemyType, Vector3 position, Quaternion rotation)
        {
            GameObject enemy = Instantiate(enemyPrefab, position, rotation, transform);
            enemy.GetComponent<Enemy>().enemyType = enemyType;
//            enemy.GetComponent<Rigidbody>().velocity = Vector3.zero;
        }

        private void Update() {
            if (_firstWave) { _firstWaveTimer += Time.deltaTime; } // Incrémentation du Timer de la Première Vague
            if (_firstWaveTimer >= 15) { // Start de la Première Vague
                _firstWaveTimer = 0;
                StartWave();
                _firstWave = false;
            }
            if (enemies.Count == enemiesNumber) { // Activation du Timer de TP des Ennemis au Train si ils sont tous Spawnés
                maxEnemiesSpawned = true;
                spawnEnemies = false;
                _spawnTimerOn = true;
            }
            if (!maxEnemiesSpawned && spawnEnemies) { 
                EnemySpawn();
                maxEnemiesSpawned = true;
            } // Spawner les Ennemis
        }

        public void AddChild(Enemy child) { // Méthode appellée par les Ennemis pour s'ajouter à la liste du Manager une fois Spawn
            enemies.Add(child);
        }

        private void EnemySpawn() { // Instantiation des Ennemis au point de Spawn
            StartCoroutine(SpawnTankEnemies(tankNumber));
            StartCoroutine(SpawnFastEnemies(fastNumber));
            StartCoroutine(SpawnNeutralEnemies(neutralNumber));
            SortEnemies();
            maxEnemiesSpawned = false;
        }

        public void SortEnemies() { // Tri des Ennemis selon les SteamPipes actifs
            currentSteamPipeManager = FindObjectOfType<SteamPipeManager>();
            List<Enemy> sortedEnemies = new List<Enemy>();
            for (int i = 0; i < enemies.Count; i++) {
                int spIndex = i % currentSteamPipeManager.localSteamPipes.Count; // Chiant à Expliquer
                currentSteamPipeManager.localSteamPipes[spIndex].AddEnemy(enemies[i]);
                enemies[i].AddSteamPipe(currentSteamPipeManager.localSteamPipes[spIndex]);
                sortedEnemies.Add(enemies[i]);
            }
            enemies = sortedEnemies;
        }

        public void StartNextWagonTransition()
        {
            StartCoroutine(GoToNextWagon());
        }
        
        private void FixedUpdate() {
            foreach (Enemy enemy in enemies) { if (enemy.enabled == false) { enemy.enabled = true; } }

            if (maxEnemiesSpawned && !_areEnemiesTeleported && _spawnTimerOn && FindObjectOfType<GameManager>().inStation == false)
            {
                StartCoroutine(TeleportEnemiesToTrain());
            }

            if (enemies.Count <= 0 && maxEnemiesSpawned && _areEnemiesTeleported && !_firstWave  && FindObjectOfType<GameManager>().inStation == false)
            {
                Debug.Log("Next wave starting in 10 seconds");
                maxEnemiesSpawned = false;
                _areEnemiesTeleported = false;
                enemies.Clear();
                StartCoroutine(WaveInterval());
            }
        } // Il arrive que les script d'enemis se désactivent je sais pas pourquoi, donc il faut les spam pour qu'ils restent activés

        private IEnumerator GoToNextWagon() // GLOBALEMENT LA MÊME CHOSE QUE TeleportEnemiesToTrain MAIS UN PEU DIFFÉRENT
        {
            yield return new WaitForSeconds(10); // Temps entre un changement de Wagon et l'Arrivée des Ennemis
            foreach (Enemy enemy in enemies)
            {
                Rigidbody rb = enemy.GetComponent<Rigidbody>();
//                rb.velocity = Vector3.zero;
                enemy.GetComponent<NavMeshAgent>().enabled = false;
                enemy.transform.position = new Vector3(transform.position.x, 1, transform.position.z + 1);
                enemy.GetComponent<NavMeshAgent>().enabled = true;
                SortEnemies();
                yield return new WaitForSeconds(1);
            }
        } 
        
        private IEnumerator TeleportEnemiesToTrain()
        {
            _areEnemiesTeleported = true;
            foreach (Enemy enemy in enemies)
            {
                if (enemy.inTrain) { continue; }
                //Rigidbody rb = enemy.GetComponent<Rigidbody>();
                //rb.velocity = Vector3.zero;
                enemy.GetComponent<NavMeshAgent>().enabled = false;
                enemy.transform.position = new Vector3(transform.position.x, 1, transform.position.z + 1);
                enemy.GetComponent<NavMeshAgent>().enabled = true;
                enemy.inTrain = true;
                SortEnemies();
                yield return new WaitForSeconds(1.35f);
            }
            foreach (Enemy enemy in enemies)
            {
                if (enemy.inTrain) { continue; }
                //Rigidbody rb = enemy.GetComponent<Rigidbody>();
                //rb.velocity = Vector3.zero;
                enemy.GetComponent<NavMeshAgent>().enabled = false;
                enemy.transform.position = new Vector3(transform.position.x, 1, transform.position.z + 1);
                enemy.GetComponent<NavMeshAgent>().enabled = true;
                enemy.inTrain = true;
                SortEnemies();
                yield return new WaitForSeconds(1.35f);
            }
            foreach (Enemy enemy in enemies)
            {
                if (enemy.inTrain) { continue; }
                //Rigidbody rb = enemy.GetComponent<Rigidbody>();
                //rb.velocity = Vector3.zero;
                enemy.GetComponent<NavMeshAgent>().enabled = false;
                enemy.transform.position = new Vector3(transform.position.x, 1, transform.position.z + 1);
                enemy.GetComponent<NavMeshAgent>().enabled = true;
                enemy.inTrain = true;
                SortEnemies();
                yield return new WaitForSeconds(1.35f);
            }
        }

        private IEnumerator WaveInterval()
        {
            finishedWaveIndicator.SetActive(true);
            yield return new WaitForSeconds(waveInterval);
            finishedWaveIndicator.SetActive(false);
            StartWave();
        }
        
        
        private IEnumerator SpawnTankEnemies(int number)
        {
            for (int i = 0; i < number; i++)
            {
                CreateEnemy(Enemy.EnemyType.Tank, enemiesSpawnAnchor.position, enemiesSpawnAnchor.rotation);
                yield return new WaitForSeconds(0.01f); // Ajuster le délai entre chaque spawn
            }
        }

        private IEnumerator SpawnFastEnemies(int number)
        {
            for (int i = 0; i < number; i++)
            {
                CreateEnemy(Enemy.EnemyType.Fast, enemiesSpawnAnchor.position, enemiesSpawnAnchor.rotation);
                yield return new WaitForSeconds(0.01f); // Ajuster le délai entre chaque spawn
            }
        }

        private IEnumerator SpawnNeutralEnemies(int number)
        {
            for (int i = 0; i < number; i++)
            {
                CreateEnemy(Enemy.EnemyType.Neutral, enemiesSpawnAnchor.position, enemiesSpawnAnchor.rotation);
                yield return new WaitForSeconds(0.01f); // Ajuster le délai entre chaque spawn
            }
        }
    }
}
