using System;
using UnityEngine;


    public class TrapPlayerDetector : MonoBehaviour
    {
        public GameObject uiGameObject;
        public GameObject leaveTrapUI;

        public TrapHLShader trapHLShader;
        private void Awake() {
            uiGameObject.SetActive(false);
        }

        private void OnTriggerEnter(Collider other) {
            if (other.CompareTag("Player"))
            {
                uiGameObject.SetActive(true);
                if (trapHLShader != null) trapHLShader.ActivateTrap();
            }
        }
        private void OnTriggerExit(Collider other) {
            if (other.CompareTag("Player"))
            {
                uiGameObject.SetActive(false);
            }
        }
        
    }

