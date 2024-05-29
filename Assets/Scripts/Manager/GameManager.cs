using System;
using System.Collections.Generic;
using Enemies;
using Environement;
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

        public float enemyTimer;
        private bool _waveChanging;

        public bool inStation;
        public bool justLeavedStation;
        public List<int> wavesToStation = new List<int>();

        public bool transitionToStation;
        public bool transitionToTrain;
        public Vector3 transitionPosition;

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
                trainManager.transform.Translate(transitionPosition * 0.25f * Time.deltaTime);
                if (trainManager.transform.position.z <= transitionPosition.z)
                {
                    transitionToStation = false;
                }
            }

            if (transitionToTrain)
            {
                trainManager.transform.Translate(transitionPosition * -0.25f * Time.deltaTime);
                if (trainManager.transform.position.z >= transitionPosition.z)
                {
                    transitionToTrain = false;
                }
            }
        }

        [ContextMenu("Leave Station")]
        public void LeaveStation()
        {
            if (!transitionToTrain)
            {
                inStation = false;
                justLeavedStation = true;
                WagonManager firstWagon = trainManager.wagons[0];
                transitionPosition = new Vector3(0,0,firstWagon.transform.localPosition.z);
            }
        }

        [ContextMenu("Spawn Train Station")]
        public void SpawnTrainStation()
        {
            if (!transitionToStation)
            {
                WagonManager lastWagon = trainManager.wagons[^1];
                Vector3 newStationPosition = new Vector3(0, 0, lastWagon.transform.localPosition.z + 35);
                trainStation.transform.position = newStationPosition;
                transitionToStation = true;
                transitionPosition = new Vector3(0,0,trainStation.transform.position.z);
            }
        }

        
    }
}
