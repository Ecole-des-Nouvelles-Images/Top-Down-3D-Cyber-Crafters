
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerManager : MonoBehaviour
    {
        [Header("Listes de gestion")]
        public List<GameObject> players = new List<GameObject>(); // Liste des Joueurs Actifs
        public List<Transform> playerSpawns = new List<Transform>(); // Liste des Spawns Actifs

        private PlayerInputManager _playerInputManager;
        private GameObject _lastPlayer; // Dernier Player Spawné par le Player Input Manager

        private void Awake() {
            // Initialisation de la liste de Spawns
            for (int i = 0; i < transform.childCount; i++) { playerSpawns.Add(transform.GetChild(i)); }
            //Récupération du PlayerInputManager
            _playerInputManager = GetComponent<PlayerInputManager>();
        }

        private void Start() {
            // Initialisation de la Teleportation des Joueurs
            PlayerInputManager.instance.playerJoinedEvent.AddListener(TeleportChildPlayer);
        }
        
        // Méthode de Téléportation d'un player
        private void TeleportChildPlayer(PlayerInput playerInput) {
            Transform spawn = playerSpawns[PlayerInputManager.instance.playerCount - 1];
            playerInput.GetComponent<PlayerController>().playerId = spawn.GetComponent<PlayerSpawner>().spawnId;
            playerInput.transform.position = new Vector3(spawn.transform.position.x, 1, spawn.transform.position.z);
            playerInput.transform.parent = spawn;
        }
    }
}
