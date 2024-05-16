using Enemies;
using Player;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

namespace Train.Wagon
{
    public class WagonManager : MonoBehaviour
    {
        [Header("Objets Généraux")] 
        public PlayerManager playerManager; // Player Manager
        public GameObject train; // Objet qui contient tout les Wagons
        public EnemiesManager enemyManager; // Enemies Manager qui gère les Ennemis et le système de Vagues
        
        [Header("Objets Locaux")]
        public SteamPipeManager localSteamPipeManager; // Manager des SteamPipes présents dans le Wagon
        public ExitDoor wagonExitDoor; // Porte de Sortie pour le changement de Wagon
        public GameObject wagonEnableable; // Objets à activer lorsque le wagon est actif
        public Transform playerAnchor; // Point de TP des joueurs
        public Transform enemyAnchor;

        [Header("Variables Locales")] public Vector3 originalPosition; // Position du Wagon lorsqu'il est instancié
        public bool steamPipesTaken; // Booléen activé lorsque tout les SteamPipes sont Down
        public int playersInsideExit; // Compte des joueurs présents dans la Porte de sortie
        private bool _allPlayersInDoor; // Booléen à activer lorsque tout les joueurs sont dans la Porte de sortie
        public bool changeWagon; // Booléen controlant le fait que le Wagon change et passe au suivant
        public bool wagonChanged; // Booléen activé lorsque le Wagon est passé au suivant
        private bool enemyPseudoFreeze;

        private void Awake() {
            // Récupération des Objets Importants
            localSteamPipeManager = GetComponentInChildren<SteamPipeManager>();
            playerManager = FindObjectOfType<PlayerManager>();
            enemyManager = FindObjectOfType<EnemiesManager>();
            //train = transform.parent.gameObject;

            // Initialisation du Wagon
            // Désactiver la porte de sortie et TOUTES ses variables pour ne pas provoquer de transition sans l'avoir trigger
            wagonExitDoor.gameObject.SetActive(false); 
            playersInsideExit = 0;
            changeWagon = false;
            wagonChanged = false;
            _allPlayersInDoor = false;
        }
        
        private void Start() {
            originalPosition = transform.position; // Récupération de la position de base pour éffectuer la transition au prochain Wagon.
            train = transform.parent.GetComponent<TrainManager>().gameObject;
        }

        private void FixedUpdate() {
            if (wagonEnableable.activeSelf) {
                if (!steamPipesTaken) {
                    localSteamPipeManager = GetComponentInChildren<SteamPipeManager>();
                    steamPipesTaken = true;
                }
                if (localSteamPipeManager.localSteamPipes.Count <= 0 && wagonEnableable.activeSelf) { wagonExitDoor.gameObject.SetActive(true); }
                if (playersInsideExit == PlayerInputManager.instance.playerCount) { _allPlayersInDoor = true; }
                else { _allPlayersInDoor = false; }
                if (_allPlayersInDoor && wagonExitDoor.gameObject.activeSelf) {
                    if (!changeWagon && !wagonChanged) {
                        originalPosition = train.transform.position;
                        changeWagon = true;
                        enemyPseudoFreeze = true;
                    }
                }
                if (wagonChanged) { enabled = false; }
            }

            if (changeWagon) {
                TrainManager trainManager = train.GetComponent<TrainManager>();
                int index = trainManager.wagons.FindIndex(w => w == this);
                WagonManager nextWagon = trainManager.wagons[index + 1];
                Vector3 newPosition = new Vector3(0, 0, originalPosition.z - 30);
                train.transform.Translate(newPosition * 0.25f * Time.deltaTime);
                Debug.Log("Wagon Changing");
                if (transform.transform.position.z <= originalPosition.z - 30)
                {
                    
                    if (index <= trainManager.wagons.Count - 1) {
                        changeWagon = false;
                        nextWagon.wagonEnableable.SetActive(true);
                        foreach (GameObject player in playerManager.players) {
                            player.GetComponent<NavMeshAgent>().enabled = false;
                            player.transform.position = nextWagon.playerAnchor.position;
                            player.GetComponent<NavMeshAgent>().enabled = true;
                            Debug.Log("Wagon Changed");
                        }
                        wagonEnableable.SetActive(false);
                        wagonChanged = true;
                        enemyManager.StartNextWagonTransition();
                        enemyManager.SortEnemies();
                        enabled = false;
                    }
                }
            }
        }
    }
}
