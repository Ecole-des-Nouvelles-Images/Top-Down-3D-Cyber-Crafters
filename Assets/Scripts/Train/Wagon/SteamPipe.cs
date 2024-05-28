using System;
using System.Collections.Generic;
using Enemies;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;


namespace Train.Wagon
{
    public class SteamPipe : MonoBehaviour
    {
        [Header("Ennemis assignés au SteamPipe")]
        public List<Enemy> assignedEnemies = new List<Enemy>();

        [Header("Objets Globaux")] [SerializeField]
        private SteamPipeManager parentSpm;

        [Header("Variables Locales")] 
        public int maxHealthPoints; // Points de Vie maximaux (Barre de Vie remplie) Pour l'UI
        public int healthPoints; // Points de Vie actuels du Steam Pipe
        public Image lifeBar; // Barre de Vie pour l'UI en World Space
        public BoxCollider enemyDetector; // Boite de Collision pour recevoir des dégats

        private void Awake() {
            parentSpm = transform.parent.GetComponent<SteamPipeManager>(); // Recherche puis Ajout au Steam Pipe Manager Local
            parentSpm.AddChild(this);
        }
        public void AddEnemy(Enemy enemy) { // Méthode appellée par les Enemis pour s'assigner au Steam Pipe
            assignedEnemies.Add(enemy);
        }

        private void OnDestroy() {
            parentSpm.localSteamPipes.Remove(this); // Suppréssion de la Liste des SteamPipes afin de pouvoir vérifier si tout les SteamPipes sont "Morts"
            foreach (Enemy enemy in assignedEnemies) { // Rediriger l'ennemi vers un autre SteamPipe
                enemy.targetedSteamPipe = null;
                enemy.GetComponentInChildren<Animator>().SetBool("Attack", false);
                if (parentSpm.localSteamPipes.Count > 0) {
                    int randomSpId = Random.Range(0, parentSpm.localSteamPipes.Count);
                    parentSpm.localSteamPipes[randomSpId].AddEnemy(enemy);
                    enemy.AddSteamPipe(parentSpm.localSteamPipes[randomSpId]);
                }
            }
        }

        private void Update() {
            lifeBar.fillAmount = healthPoints / (float)maxHealthPoints; // Remplissage de la Barre de Vie selon les PV
            if (healthPoints <= 0) {Destroy(gameObject);} // La Mort du Steam Pipe
            // A voir Pour ajouter plusieurs méchaniques selon l'état du Steam Pipe (Neuf, Abimé, Détruit) amenant donc 
            // des Particules, un changement de modèle, etc.
        }

        private void OnTriggerStay(Collider other) {
            if (other.CompareTag("Enemy") && enemyDetector && !other.GetComponent<Enemy>().navMeshAgent.isStopped) {
                healthPoints -= 1; // Retirer des PV en cas de Contact avec un Ennemi Actif
                other.GetComponentInChildren<Animator>().SetBool("Attack", true);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Enemy")) {
                other.GetComponentInChildren<Animator>().SetBool("Attack", false);
            }
        }
    }
}
