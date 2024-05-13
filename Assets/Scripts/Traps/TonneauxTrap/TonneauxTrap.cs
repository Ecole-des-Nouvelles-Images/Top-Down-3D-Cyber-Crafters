using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TonneauxTrap : Trap
{
    public GameObject tonneaux;
    public Transform tonneauxSpawnPoint;
    public Animator animator;
    
    private float timer;
    private float trapDuration = 10f;
    
    private void FixedUpdate()
    {
        if (isActivated)
        {
            if (timer < trapDuration) timer += Time.deltaTime;
            else
            {
                //Lancement de l'animation d'arrêt du piège. 
                animator.SetTrigger("stopTrap");
                isActivated = false;
            }
        }
    }
    
    public override void ActivateTrap()
    {
        Vector3 spawnPosition = tonneauxSpawnPoint.position;
        Quaternion spawnRotation = tonneaux.transform.rotation;
        animator.SetTrigger("activateTrap");
        Instantiate(tonneaux);//, spawnPosition, spawnRotation);
        isActivated = true;
        timer = 0;
    }
    
}
