
using System.Collections.Generic;
using Enemies;
using Environement;
using Player;
using Train;
using Train.Wagon;
using UnityEngine;


namespace Manager
{
    public class GameManager : MonoBehaviour
    {
        public TrainManager trainManager;
        public EnemiesManager enemiesManager;
        public GameObject trainStation;
        public List<TreeSpawner> treeSpawners = new List<TreeSpawner>();
        public RandomCameraShake randomCameraShake;
        public Animator environementAnimator;

        public float enemyTimer;
        private bool _waveChanging;

        public bool inStation;
        public bool justLeavedStation;
        public List<int> wavesToStation = new List<int>();

        public bool transitionToStation;
        public bool transitionToTrain;
        public Vector3 transitionPosition;

        public PlayerManager playerManager;

        private void Start() {
            inStation = false;
            justLeavedStation = false;
        }

        private void Update() {
            if (enemiesManager.maxEnemiesSpawned && enemiesManager.enemies.Count < 1 && !justLeavedStation) {
                foreach (int waveId in wavesToStation) {
                    if (enemiesManager.lastWave == waveId) {
                        inStation = true;
                        break;
                    }
                }
            }

            if (inStation)
            {
                foreach (TreeSpawner treeSpawner in treeSpawners)
                {
                    treeSpawner.canSpawn = false;
                }

                enemiesManager.waveInterval = Mathf.Infinity;
                randomCameraShake.enabled = false;
                SpawnTrainStation();
                //Spawner la gare
            }
            else
            {
                foreach (TreeSpawner treeSpawner in treeSpawners)
                {
                    treeSpawner.canSpawn = true;
                }
                enemiesManager.waveInterval = 15f;
                randomCameraShake.enabled = true;
                //Despawner la gare
            }

            if (transitionToStation)
            {
                if (FindObjectOfType<Camera>().orthographicSize < 10)
                {
                    FindObjectOfType<Camera>().orthographicSize += 0.01f;
                }
                environementAnimator.enabled = false;
                foreach (TreeSpawner treeSpawner in treeSpawners)
                {
                    treeSpawner.enabled = false;
                }

                if (trainManager.transform.position.z !<= transform.position.z)
                {
                    trainManager.transform.Translate(transitionPosition * 0.25f * Time.deltaTime);
                    environementAnimator.transform.Translate(transitionPosition * 0.25f * Time.deltaTime);
                }
                
                if (trainManager.transform.position.z <= transform.position.z)
                {
                    transitionToStation = false;
                    inStation = true;
                }
            }

            if (transitionToTrain)
            {
                
                trainManager.transform.Translate((-trainManager.transform.position) * 1 * Time.deltaTime);
                environementAnimator.transform.Translate((-trainManager.transform.position) * 1 * Time.deltaTime);
                if (FindObjectOfType<Camera>().orthographicSize > 8)
                {
                    FindObjectOfType<Camera>().orthographicSize -= 0.01f;
                }
                foreach (TreeSpawner treeSpawner in treeSpawners)
                {
                    treeSpawner.enabled = true;
                }
                if (transform.position.z <= trainManager.transform.position.z)
                {
                    transitionToTrain = false;
                    inStation = false;
                    justLeavedStation = true;
                    trainStation.gameObject.SetActive(false);
                    trainStation.transform.position = new Vector3(0, 0, 450);
                    environementAnimator.enabled = true;
                    
                }
            }
        }

        [ContextMenu("Leave Station")]
        public void LeaveStation()
        {
            if (!transitionToTrain)
            {
                WagonManager firstWagon = trainManager.wagons[0];
                transitionPosition = new Vector3(0,0,trainStation.transform.position.z);
                this.transform.position = transitionPosition;
                transitionToTrain = true;
            }
        }

        [ContextMenu("Spawn Train Station")]
        public void SpawnTrainStation()
        {
            if (!transitionToStation)
            {
                trainStation.gameObject.SetActive(true);

                //Change inputAction
                // PlayerInput playerInput = playerManager.players[0].GetComponent<PlayerInput>();
                // playerInput.currentActionMap = playerInput.actions.FindActionMap("GareStation");
                // playerInput.SwitchCurrentActionMap("GareStation");
                WagonManager lastWagon = trainManager.wagons[^1];
                Vector3 newStationPosition = new Vector3(0, 0, lastWagon.transform.position.z + 35);
                trainStation.transform.position = newStationPosition;
                transitionToStation = true;
                transitionPosition = new Vector3(0,0,-trainStation.transform.localPosition.z);
                this.transform.position = transitionPosition;
            }
        }

        
    }
}
