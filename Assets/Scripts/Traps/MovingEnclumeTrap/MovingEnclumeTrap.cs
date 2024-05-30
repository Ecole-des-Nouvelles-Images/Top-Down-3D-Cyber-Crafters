using System;
using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;
using UnityEngine.InputSystem;


//Line renderer sur l'enclume pour montrer ou tombe l'enclume

public class MovingEnclumeTrap : MonoBehaviour
{
    public Enclume enclumePrefab;
    public Enclume enclume;
    public Transform enclumeSpawnPoint;
    public Animator animator;
    public float moveSpeed = 5f; // Vitesse de déplacement de l'enclume
    public float moveRange = 10f; // Distance maximale que l'enclume peut parcourir
    
    private Vector3 moveDirection;
    private Vector3 initialPosition;

    private float coolDown;

    private PlayerInput _playerInput;
    private bool _isActivated = false;

    public GameObject enclumeHolder;
    

    private void Start()
    {
        initialPosition = enclumeSpawnPoint.position;
    }

    private void Update()
    {
        if (_isActivated)
        {
            //Déplacement de l'enclume
            Vector2 input = _playerInput.currentActionMap.FindAction("Move").ReadValue<Vector2>();
            moveDirection = new Vector3(input.x, 0, input.y);
            Vector3 newPosition = enclumeHolder.transform.position += moveDirection * (moveSpeed * Time.deltaTime);
            Vector3 clampedPosition = initialPosition + Vector3.ClampMagnitude(newPosition - initialPosition, moveRange);
            enclumeHolder.transform.position = clampedPosition;

            if (_playerInput.actions["Drop"].WasPressedThisFrame())
            {
                enclume.DropEnclume();
                Destroy(enclume.gameObject, 2);
                StartCoroutine(InstantiateEnclumeAfterDelay(5f)); // change delay with cooldowntime
            }

            if (_playerInput.actions["Exit"].WasPressedThisFrame())
            {
                
                _playerInput.SwitchCurrentActionMap("GamePlay");
                _isActivated = false;
            }

            
        }
    }

    public void OnControlStation(PlayerController playerController)
    {
        if (!_isActivated)
        {
            _playerInput = playerController.GetComponent<PlayerInput>();
            _playerInput.currentActionMap.Disable();
            _playerInput.currentActionMap =
                playerController.GetComponent<PlayerInput>().actions.FindActionMap("MovingEnclume");
            _playerInput.SwitchCurrentActionMap("MovingEnclume");
            _isActivated = true;
        }
    }
    
    private IEnumerator InstantiateEnclumeAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        enclume = Instantiate(enclumePrefab
            , enclumeSpawnPoint);
        
    }
}
