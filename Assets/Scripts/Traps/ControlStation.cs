using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ControlStation : MonoBehaviour
{
    public MeshRenderer buttonMeshRenderer;
    public Animator animator;
    public GameObject dangerZone;
    public bool alreadyPressed;
    public float cooldownTime = 5f;
    private float _timer;
    private float activationTime;
    private bool isActivated;

    public Trap trap;
    
    public bool button = false; // Si c'est un bouton, on change la couleur du bouton en fonction de l'activation du piege.
    
    public AudioClip ErrorSound;
    

    private void Awake()
    {
        // dangerZone.SetActive(false);
    }

    private void Update()
    {
        if (alreadyPressed)
        {
            _timer += Time.deltaTime;
        }

        // Verifie que le piege n'est plus actif. 
        if (_timer >= cooldownTime && !trap.isActivated)
        {
            animator.SetBool("isActivated", false);
            if(button) buttonMeshRenderer.material.color = Color.red;
            isActivated = false; //?
            alreadyPressed = false;
           // dangerZone.SetActive(false);
            
           _timer = 0;
        }
    }

    public void TriggerTrap()
    {
        if (!alreadyPressed)
        {
            activationTime = Time.time;
            isActivated = true;
            
            //Si il n'y a qu'une seule station de controle, on active le piege directement.
            if (trap.controlStations.Count < 2)
            {
                trap.ActivateTrap();
                alreadyPressed = true;
                _timer = 0;
                animator.SetBool("isActivated", true);

            }
            //Sinon on verifie que toutes les stations de controle sont activées.
            else StartCoroutine(CheckOtherStations());
           
        }
    }

    
    // Verifie que toutes les stations de controle sont activées ensembles dans le temps imparti.
    private IEnumerator CheckOtherStations()
    {
        if(button) buttonMeshRenderer.material.color = Color.green;
        alreadyPressed = true;
        _timer = 0;
        yield return new WaitForSeconds(1);
        ControlStation sister = null;
        
        foreach (ControlStation controlStation in trap.controlStations)
        {
            Debug.Log($"controlStation.isActivated : {controlStation.isActivated} activationTime : {activationTime}");
            if (!controlStation.isActivated || Mathf.Abs(controlStation.GetActivationTime() - activationTime) > 0.5f)
            {
                Debug.Log($"CheckOtherStations is breaking early for controlStation: {controlStation.name}");
                if (controlStation != this)
                {
                    sister = controlStation;
                    Debug.Log($"sister is {sister.name}");
                }

                if(button) buttonMeshRenderer.material.color = Color.red;
                GetComponent<AudioSource>().PlayOneShot(ErrorSound);

                isActivated = false;
                yield break;
            }
        }
        Debug.Log($"CheckOtherStations is setting isActivated to true {this.name}");
        //if(button) buttonMeshRenderer.material.color = Color.green;
        animator.SetBool("isActivated", true);

        if(sister != null) sister.animator.SetBool("isActivated", true);
        if(button) buttonMeshRenderer.material.color = Color.yellow;

        trap.ActivateTrap();

    }

    public float GetActivationTime()
    {
        return activationTime;
    }
    
    
}