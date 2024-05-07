using System;
using UnityEngine;


    public class TrapPlayerDetector : MonoBehaviour
    {
        public GameObject uiGameObject;

        private void Awake() {
            uiGameObject.SetActive(false);
        }

        private void OnTriggerEnter(Collider other) {
            if (other.CompareTag("Player")) { uiGameObject.SetActive(true); }
        }
        private void OnTriggerExit(Collider other) {
            if (other.CompareTag("Player")) { uiGameObject.SetActive(false); }
        }
        
    }

