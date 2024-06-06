using System;
using UnityEngine;


    public class TrapPlayerDetector : MonoBehaviour
    {
        public GameObject uiGameObject;
        public GameObject trapHL;

        public GameObject activedUI;

        public Trap trap;

        //public TrapHLShader trapHLShader;
        private void Awake() {
            uiGameObject.SetActive(false);
        }

        private void OnTriggerEnter(Collider other) {
            if (other.CompareTag("Player"))
            {
                uiGameObject.SetActive(true);
                //if (trapHLShader != null) trapHLShader.ActivateTrap();
                if(!trap.isActivated) trapHL.SetActive(true);
                else
                {
                    if(activedUI != null) activedUI.SetActive(true);
                }
            }
        }
        private void OnTriggerExit(Collider other) {
            if (other.CompareTag("Player"))
            {
                uiGameObject.SetActive(false);
                trapHL.SetActive(false);
                if(activedUI != null) activedUI.SetActive(false);
            }
        }
        
    }

