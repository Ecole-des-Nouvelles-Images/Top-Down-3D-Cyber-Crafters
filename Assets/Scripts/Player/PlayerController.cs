using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        [Header("Gestion Visuelle")] 
        public GameObject playerModel; // Objet Contenant le Player Model (Visual)
        public GameObject playerMesh; // Mesh du PlayerModel (Voir Liste "playerModels")
        public List<GameObject> playerModels = new List<GameObject>(); // Liste des PlayerModels disponibles
        private Animator _animator; // Animator du Joueur (Présent sur l'objet PlayerModel)
        
        [Header("Variables de Déplacement")] 
        [SerializeField] private Camera mainCamera; // Caméra nécéssaire aux calculs de déplacements
        public float speed;
        public float stoppingDistance;
        public float angularSpeed;
        private NavMeshAgent _navMeshAgent;
        
        [Header("Variables d'Input")]
        [SerializeField]private PlayerInput playerInput;
        private InputAction _buttonA;
        private bool _buttonAPressed;

        [Header("Variables de Gestion")] 
        public int playerId; // ID nécéssaire à la gestion des Spawns, de la Partie, des Couleurs

        private void Awake() {
            // Assignations de Variables
            _navMeshAgent = GetComponent<NavMeshAgent>();
            mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
            playerInput = GetComponent<PlayerInput>();
            playerId = FindObjectOfType<PlayerManager>().players.Count;
            
            // INSTANTIATION VISUELLE
            // /!\ TEMPORAIRE /!\ Choix d'un Player Model Random et (Pas Temporaire) Instantiation du Player Model
            // Choix Random
            int randomPm = Random.Range(0, playerModels.Count);
            // Instantiation
            Vector3 pmPosition = playerModel.transform.position;
            Instantiate(playerModels[randomPm], new Vector3(pmPosition.x, pmPosition.y, pmPosition.z),
                playerModel.transform.rotation, playerModel.transform);
            // Assignation des Variables
            _animator = playerModel.GetComponentInChildren<Animator>();
            playerMesh = playerModel.GetComponentInChildren<MeshRenderer>().gameObject;
            
            // Assignation Variables
            _navMeshAgent.stoppingDistance = stoppingDistance;
            _navMeshAgent.angularSpeed = angularSpeed;
            _buttonAPressed = false;
        }

        private void Start() {
            FindObjectOfType<PlayerManager>().players.Add(gameObject); // Ajout au manager après l'instanciation
            playerId = transform.parent.GetComponent<PlayerSpawner>().spawnId; // Récupération de l'ID du joueur
            // Assigation de la Couleur selon l'ID
            switch (playerId) {
                case 1:
                    playerMesh.GetComponent<MeshRenderer>().material.color = Color.red;
                    break;
                case 2:
                    playerMesh.GetComponent<MeshRenderer>().material.color = Color.blue;
                    break;
                case 3:
                    playerMesh.GetComponent<MeshRenderer>().material.color = Color.yellow;
                    break;
                case 4:
                    playerMesh.GetComponent<MeshRenderer>().material.color = Color.green;
                    break;
            }
        }

        private void Update() {
            Vector2 moveInput = playerInput.actions["Move"].ReadValue<Vector2>();
            _navMeshAgent.speed = speed;
            if (moveInput.magnitude <= 0.05f) {
                //ANIMATION DE WALK A METTRE EN FALSE
                return;
            }
            if (moveInput.magnitude >= 0.1f) { Move(moveInput); }
        }

        // METHODE DE MOUVEMENT
        private void Move(Vector2 input)
        {
            //ANIMATION DE WALK A METTRE EN TRUE
            
            // Calculs par rapport à l'angle de Camera
            Vector3 cameraForward = mainCamera.transform.forward;
            cameraForward.y = 0;
            cameraForward.Normalize();
            Vector3 cameraRight = mainCamera.transform.right;
            cameraRight.y = 0;
            cameraRight.Normalize();
            // Calcul de la destination
            Vector3 destination = transform.position + cameraRight * input.x + cameraForward * input.y;
            float distanceToDestination = Vector3.Distance(transform.position, destination);
            float moveDistance = speed * Time.deltaTime;
            if (distanceToDestination > moveDistance)
            {
                transform.position = Vector3.MoveTowards(transform.position, destination, moveDistance);
            }
            else
            {
                transform.position = destination;
            }

            Vector3 targetPosition = new Vector3(destination.x, playerModel.transform.position.y, destination.z);
            playerModel.transform.LookAt(targetPosition);
        }

        // Récupération de l'appui du bouton A (Pour les interractions)
        private void OnA(InputValue value) {
            if (value.isPressed) { _buttonAPressed = true; }
            else { _buttonAPressed = false; }
        }

        private void OnTriggerStay(Collider other) {
            if (other.CompareTag("Trap")) {
                if (_buttonAPressed) {
                    // ACTIVATION DU PIEGE
                    _buttonAPressed = false;
                }
            }
            if (other.CompareTag("ControlStation")) {
                if (_buttonAPressed) {
                    // ON CONTROL STATION
                    _buttonAPressed = false;
                }
            }
        }
    }
}
