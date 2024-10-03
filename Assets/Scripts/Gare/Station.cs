using System.Collections.Generic;
using Manager;
using Player;
using Train;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

namespace Gare
{
    public class Station : MonoBehaviour
    {
        public GameObject wagonShopGameObject;
        private bool rotating;
        private Quaternion targetRotation;
        private float rotationDuration = 1f;
        private float rotationTimeElapsed = 0f;
        public int WagonIndex;
        public List<GameObject> shopWagons = new List<GameObject>();
        public FakeWagon selectedWagon;
        
        public GameManager gameManager;
        public PlayerManager playerManager;
        private PlayerInput _playerInput;
        
        public AudioSource audioSource;
        public AudioClip changeWagonLeftClip;
        public AudioClip changeWagonRightClip;
        public AudioClip buyWagonClip;
        public AudioClip leaveStationClip;
        private void OnEnable()
        {
            selectedWagon = shopWagons[WagonIndex].GetComponent<FakeWagon>();
            _playerInput = playerManager.players[0].GetComponent<PlayerInput>();
            _playerInput = playerManager.players[0].GetComponent<PlayerInput>();
            _playerInput.currentActionMap = _playerInput.actions.FindActionMap("GareStation");
            _playerInput.SwitchCurrentActionMap("GareStation");
        }

        private void OnDisable()
        {
            _playerInput = playerManager.players[0].GetComponent<PlayerInput>();
            _playerInput = playerManager.players[0].GetComponent<PlayerInput>();
            _playerInput.currentActionMap = _playerInput.actions.FindActionMap("Gameplay");
            _playerInput.SwitchCurrentActionMap("Gameplay");
        }

        private void FixedUpdate()
        {
            
            if (rotating)
            {
                rotationTimeElapsed += Time.deltaTime;
                float rotationProgress = rotationTimeElapsed / rotationDuration;
                wagonShopGameObject.transform.rotation = Quaternion.Lerp(wagonShopGameObject.transform.rotation, targetRotation, rotationProgress);

                if (rotationProgress >= 1f)
                {
                    rotating = false;
                    rotationTimeElapsed = 0f;
                }
            }
            else
            {
                if (_playerInput.actions["Right"].WasPressedThisFrame())
                {
                    audioSource.PlayOneShot(changeWagonRightClip);
                    shopWagons[WagonIndex].SetActive(false);
                    WagonIndex -= 1;
                    if (WagonIndex > 5) { WagonIndex = 0; }
                    if (WagonIndex < 0) { WagonIndex = 5; }
                    shopWagons[WagonIndex].SetActive(true);
                    rotating = true;
                    selectedWagon = shopWagons[WagonIndex].GetComponent<FakeWagon>();
                    targetRotation = Quaternion.Euler(wagonShopGameObject.transform.eulerAngles.x, wagonShopGameObject.transform.eulerAngles.y, wagonShopGameObject.transform.eulerAngles.z + 60f);
                }

                if (_playerInput.actions["Left"].WasPressedThisFrame())
                {
                    audioSource.PlayOneShot(changeWagonLeftClip);
                    shopWagons[WagonIndex].SetActive(false);
                    WagonIndex += 1;
                    if (WagonIndex > 5) { WagonIndex = 0; }
                    if (WagonIndex < 0) { WagonIndex = 5; }
                    shopWagons[WagonIndex].SetActive(true);
                    rotating = true;
                    selectedWagon = shopWagons[WagonIndex].GetComponent<FakeWagon>();
                    targetRotation = Quaternion.Euler(wagonShopGameObject.transform.eulerAngles.x, wagonShopGameObject.transform.eulerAngles.y, wagonShopGameObject.transform.eulerAngles.z - 60f);
                }

                if (_playerInput.actions["Buy"].WasPressedThisFrame())
                {
                    audioSource.PlayOneShot(buyWagonClip);
                    selectedWagon.OnBuy();
                }
                
                if(_playerInput.actions["Leave"].WasPressedThisFrame())
                {
                    audioSource.PlayOneShot(leaveStationClip);
                    foreach (GameObject player in playerManager.players)
                    {
                        player.GetComponent<NavMeshAgent>().enabled = false;
                        player.transform.position = FindObjectOfType<TrainManager>().wagons[0].playerAnchor.position;
                        player.GetComponent<NavMeshAgent>().enabled = true;
                    }
                    gameManager.LeaveStation();
                }
            }
        }
    }
}

