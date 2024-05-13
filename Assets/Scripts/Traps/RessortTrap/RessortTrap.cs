using System;
using System.Collections;
using System.Collections.Generic;
using Enemies;
using UnityEngine;

public class RessortTrap : Trap
{
    public Animator animator;

    public float jumpHeight = 10f;
    public float jumpDuration = 1f;
    public float maxJumpDistance = 25f;
    
    private float timer;
    

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy")) launchEnemy(other.GetComponent<Enemy>());
    }

    private void launchEnemy(Enemy enemy)
    {
        //Informer la control Station du déclenchement du piège.
        isActivated = true;
        //Lancer l'animation de lancement du piège.
        animator.SetTrigger("launchTrap");
        //Propulser l'ennemi via translation et lerp.
        Vector3 targetPos = enemy.transform.position + Vector3.up * jumpHeight + enemy.transform.forward * maxJumpDistance;
        StartCoroutine(LerpEnemy(enemy, targetPos, jumpDuration));
        //Désactiver le piège.
       // gameObject.SetActive(false);
        //Piège en attente de réarmement. 
    }

    private IEnumerator LerpEnemy(Enemy enemy, Vector3 targetPos, float duration)
    {
        float time = 0;
        Vector3 startPos = enemy.transform.position;
        while (time < duration)
        {
            enemy.transform.position = Vector3.Lerp(startPos, targetPos, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        enemy.transform.position = targetPos;
        //Lancer l'animation de fin de lancement du piège.
        animator.SetTrigger("stopTrap");
        //Propulser l'ennemi via translation et lerp.
        //Désactiver le piège.
        //Piège en attente de réarmement. 
    }
}
