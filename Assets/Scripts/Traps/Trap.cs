using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Trap : MonoBehaviour
{
    public bool isActivated = false;

    public float cooldownTime;
    private float _timer;
    
    

    // public string trapActionMap;

    // private PlayerInput _playerInput;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        // Si piège activé, déclencher et cooldown
        if (isActivated)
        {
            TriggerTrap();
            _timer += Time.deltaTime;
        }

        if (_timer >= cooldownTime)
        {
            //
        }
    }

    public virtual void TriggerTrap()
    {
        
    }

    // Active l'actionMap du piège quand le joueur prend son contrôle
    // public void OnControlStation(PlayerController playerController)
    // {
    //     _playerInput = playerController.GetComponent<PlayerInput>();
    //     _playerInput.currentActionMap = _playerInput.actions.FindActionMap(trapActionMap);
    //
    // }

    // Redonne le contrôle du personnage au joueur qui quitte le contrôle du piège.
    // void ExitTrap()
    // {
    //     isActivated = false;
    // _playerInput.currentActionMap = _playerInput.actions.FindActionMap("Gameplay");


}