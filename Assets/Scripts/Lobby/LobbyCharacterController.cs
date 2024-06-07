using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

namespace Lobby
{
    public class LobbyCharacterController : MonoBehaviour
    {
        public float speed;
        public List<GameObject> playerModels;
        public int playerModelId;
        public Transform modelSpawn;
        public PlayerInput playerInput;
        public GameObject playerModel;
        public string characterSelectionScene;
        private bool _buttonAPressed;

        private void Awake()
        {
            playerModelId = Random.Range(0, 4);
            playerModel = Instantiate(playerModels[playerModelId], modelSpawn.position, modelSpawn.rotation, modelSpawn);
            playerModel.transform.localScale = new Vector3(1.45f, 1.45f, 1.45f);
        }

        private void FixedUpdate()
        {
            _buttonAPressed = playerInput.actions["Validate"].WasPressedThisFrame();
        }

        private void Update()
        {
            Vector2 moveInput = playerInput.actions["MoveLobby"].ReadValue<Vector2>();
            Vector3 move = new Vector3(0, 0, Math.Sign(moveInput.x));
            if (move.z != 0)
            {
                speed = 7;
                transform.Translate(move * speed * Time.deltaTime);
            }
            else
            {
                speed = 0;
            }
            
            Animator animator = modelSpawn.GetComponentInChildren<Animator>();
            if (moveInput.x != 0)
            {
                animator.SetBool("Walk", true);
            }
            else
            {
                animator.SetBool("Walk", false);
            }

            if (moveInput.x < 0)
            {
                playerModel.transform.rotation = Quaternion.Euler(0,180,0);
            }

            if (moveInput.x > 0)
            {
                playerModel.transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.GetComponent<InterractiveItem>())
            {
                if (other.GetComponent<InterractiveItem>().PlayButton)
                {
                    if (_buttonAPressed)
                    {
                        Debug.Log("Play");
                        SceneManager.LoadScene(characterSelectionScene);
                    }
                }

                if (other.GetComponent<InterractiveItem>().OptionButton)
                {
                    if (_buttonAPressed)
                    {
                        Debug.Log("Settings");
                    }
                }
                
                if (other.GetComponent<InterractiveItem>().QuitButton)
                {
                    if (_buttonAPressed)
                    {
                        Debug.Log("Quit");
                    }
                }


            }
        }
    }
}