using System.Collections.Generic;
using Train.Wagon;
using UnityEngine;

namespace Train {
    public class TrainManager : MonoBehaviour {
        public List<WagonManager> wagons = new List<WagonManager>(); // Liste des Wagons présents dans le Train
        public int scrapsCount; // Compte des Scraps (Monnaie) possédés pas les joueurs
        public GameObject wagonPrefab; // Objet qui est défini pour instancier un nouveau Wagon, il est remplacé par le Wagon acheté

        private void Awake() {
            foreach (WagonManager wagon in transform.GetComponentsInChildren<WagonManager>()) { wagons.Add(wagon); }
        }

        [ContextMenu("Add New Wagon")]
        public void AddWagon() { // Ajoute un Wagon à l'arrière du train
            Transform lastWagonTransform = wagons[wagons.Count - 1].gameObject.transform;
            Vector3 nextWagonPosition = new Vector3(0, 0, lastWagonTransform.position.z + 30);
            GameObject newWagon = Instantiate(wagonPrefab, nextWagonPosition, Quaternion.Euler(0, 180, 0), transform);
            wagons.Add(newWagon.GetComponent<WagonManager>());
        }
    }
}
