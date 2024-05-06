using System;
using UnityEngine;

namespace Train.Wagon
{
    public class ExitDoor : MonoBehaviour
    {
        [Header("Wagon Parent")]
        public WagonManager localWagonManager;
        private void OnTriggerEnter(Collider other) {
            if (other.CompareTag("Player")) {
                Debug.Log(other.gameObject + " entered in Exit Door");
                localWagonManager.playersInsideExit += 1; // Ajout du joueur détecté au compte de la porte
            }
        }
        private void OnTriggerExit(Collider other) {
            if (other.CompareTag("Player")){
                Debug.Log(other.gameObject + " exited of Exit Door");
                localWagonManager.playersInsideExit -= 1; // Retirement du joueur détecté au compte de la porte
            }
        }
    }
}
