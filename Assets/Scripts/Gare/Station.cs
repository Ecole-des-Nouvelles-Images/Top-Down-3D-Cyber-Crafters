using System;
using System.Collections.Generic;
using Manager;
using Player;
using UnityEditor.Build.Content;
using UnityEngine;
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
        private void OnEnable()
        {
            selectedWagon = shopWagons[WagonIndex].GetComponent<FakeWagon>();
            _playerInput = playerManager.players[0].GetComponent<PlayerInput>();
            _playerInput = playerManager.players[0].GetComponent<PlayerInput>();
            _playerInput.currentActionMap = _playerInput.actions.FindActionMap("GareStation");
            _playerInput.SwitchCurrentActionMap("GareStation");
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
                    selectedWagon.OnBuy();
                }
                
                if(_playerInput.actions["Exit"].WasPressedThisFrame())
                {
                    gameManager.LeaveStation();
                }
            }
        }
    }
}

