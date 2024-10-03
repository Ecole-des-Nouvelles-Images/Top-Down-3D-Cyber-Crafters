using System;
using Enemies;
using TMPro;
using UnityEngine;

namespace Train
{
    public class UiManager : MonoBehaviour
    {
        public TextMeshProUGUI tmpWaveNumber;
        public TextMeshProUGUI tmpScrapsNumber;

        public EnemiesManager enemiesManager;
        public TrainManager trainManager;

        private void FixedUpdate()
        {
            tmpWaveNumber.text = enemiesManager.currentWave.ToString();
            tmpScrapsNumber.text = trainManager.scrapsCount.ToString();
        }
    }
}
