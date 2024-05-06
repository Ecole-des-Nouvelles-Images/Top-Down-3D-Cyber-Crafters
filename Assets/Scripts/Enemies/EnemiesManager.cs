using System;
using System.Collections;
using System.Collections.Generic;
using Train.Wagon;
using UnityEngine;
using UnityEngine.AI;

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
        public bool spawnEnemies; // Booléen permettant de lancer le spawn des Ennemis
        private float _spawnDelay; // Timer de spawn des Ennemis;
        private bool _spawnTimerOn; // Booléen d'activation du Timer
        private bool _firstWave; // Booléen pour vérifier si il s'agit de la première vague
        private float _firstWaveTimer; // Timer du Spawn de la Première Vague
        private bool _maxEnemiesSpawned; // Permet de stopper le spawn des Ennemis lorsque enemiesNumber est atteint
        public GameObject finishedWaveIndicator; // Objet d'UI pour montrer qu'une Vague est finie
        
        [Header("Objets Locaux")] 
        public List<Enemy> enemies = new List<Enemy>(); // Ennemis Enfants en Vie
        private List<Enemy> _teleportedEnemies = new List<Enemy>(); // Ennemis téléportés dans le train (Pour le Spawn)
        public GameObject enemyPrefab; // Prefab d'Ennemi (A changer en Liste quand on aura trois types d'Ennemis)

        private void Start() {
            // Initialisation des variables et Récupération du SPM
            spawnEnemies = false;
            _spawnDelay = 0;
            currentSteamPipeManager = FindObjectOfType<SteamPipeManager>();
            _firstWave = true;
            finishedWaveIndicator.SetActive(false);
        }

        [ContextMenu("Star next Wave")] // Méthode d'initialisation d'une Wave (Avec l'incrément)
        public void StartWave() {
            finishedWaveIndicator.SetActive(false);
            _teleportedEnemies.Clear();
            _maxEnemiesSpawned = false;
            currentWave += 1;
            if (lastWave > currentWave) { enemiesNumber += 10; lastWave += 1; } // Incrémentation de la vague et du Nombre d'Ennemis à Spawner
            spawnEnemies = true;
        }

        private void Update() {
            if (_firstWave) { _firstWaveTimer += Time.deltaTime; } // Incrémentation du Timer de la Première Vague
            if (_firstWaveTimer >= 15) { // Start de la Première Vague
                _firstWaveTimer = 0;
                StartWave();
                _firstWave = false;
            }
            if (enemies.Count == enemiesNumber) { // Activation du Timer de TP des Ennemis au Train si ils sont tous Spawnés
                _maxEnemiesSpawned = true;
                spawnEnemies = false;
                _spawnTimerOn = true;
            }
            if (!_maxEnemiesSpawned && spawnEnemies) { EnemySpawn(); } // Spawner les Ennemis
            if (_maxEnemiesSpawned) { // Lancement de la Téléportation
                if (_spawnTimerOn) { _spawnDelay += Time.deltaTime; }
                if (_spawnDelay >= 1) { TeleportEnemiesToTrain(); _spawnDelay = 0; }
            }
        }

        public void AddChild(Enemy child) { // Méthode appellée par les Ennemis pour s'ajouter à la liste du Manager une fois Spawn
            enemies.Add(child);
        }

        private void EnemySpawn() { // Instantiation des Ennemis au point de Spawn
            GameObject newEnemy = Instantiate(enemyPrefab, enemiesSpawnAnchor.position, enemiesSpawnAnchor.rotation, transform);
            newEnemy.GetComponent<Rigidbody>().velocity = Vector3.zero;
            SortEnemies();
        }

        private void TeleportEnemiesToTrain() { // Méthode de Téléportation des Ennemis un par un au Train
            foreach (Enemy enemy in enemies) {
                if (!enemy.inTrain) {
                    enemy.GetComponent<NavMeshAgent>().enabled = false;
                    enemy.GetComponent<Rigidbody>().velocity = Vector3.zero;
                    enemy.transform.position = transform.position; // Le Transform.Position correspond aux coordonnées ->
                    // de l'Enemy Spawner qui contient le script à un Endroit Fixe à l'entrée d'un Wagon
                    enemy.inTrain = true;
                    enemy.GetComponent<NavMeshAgent>().enabled = true;
                    enemy.GetComponent<NavMeshAgent>().SetDestination(enemy.targetedSteamPipe.transform.position);
                    _teleportedEnemies.Add(enemy);
                    break;
                }
            }
            if (_teleportedEnemies.Count >= enemiesNumber) {
                _spawnTimerOn = false;
                _teleportedEnemies.Clear();
            }
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
        } // Il arrive que les script d'enemis se désactivent je sais pas pourquoi, donc il faut les spam pour qu'ils restent activés

        private IEnumerator GoToNextWagon() // GLOBALEMENT LA MÊME CHOSE QUE TeleportEnemiesToTrain MAIS UN PEU DIFFÉRENT
        {
            yield return new WaitForSeconds(10); // Temps entre un changement de Wagon et l'Arrivée des Ennemis
            foreach (Enemy enemy in enemies)
            {
                Rigidbody rb = enemy.GetComponent<Rigidbody>();
                rb.velocity = Vector3.zero;
                enemy.GetComponent<NavMeshAgent>().enabled = false;
                enemy.transform.position = new Vector3(transform.position.x, 1, transform.position.z + 1);
                enemy.GetComponent<NavMeshAgent>().enabled = true;
                SortEnemies();
                yield return new WaitForSeconds(1);
            }
        }
    }
}
