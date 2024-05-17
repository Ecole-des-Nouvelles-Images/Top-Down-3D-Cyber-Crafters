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
    
    public int damage = 1;
    
    [Header("SFX")]
    public AudioClip ressortStartSound;
    public AudioSource audioSource;
    
    private float timer;

    public Collider trapZone;

    // private void OnTriggerEnter(Collider other)
    // {
    //     if (other.CompareTag("Enemy")) launchEnemy(other.GetComponent<Enemy>());
    // }

    public override void ActivateTrap()
    {
        //Lancer l'animation de lancement du piège.
        animator.SetTrigger("launchTrap");
        audioSource.PlayOneShot(ressortStartSound);
        Collider[] hitColliders = Physics.OverlapBox(trapZone.bounds.center, trapZone.bounds.extents, trapZone.transform.rotation, LayerMask.GetMask("Enemy"));
       Debug.Log(hitColliders.Length);
        foreach (var hitCollider in hitColliders)
        {
            Enemy enemy = hitCollider.GetComponent<Enemy>();
            if (enemy != null)
            {
                launchEnemy(enemy);
            }
        }
    }

    private void launchEnemy(Enemy enemy)
    {
        //Informer la control Station du déclenchement du piège.
        isActivated = true;
        //Propulser l'ennemi via translation et lerp.
        Vector3 targetPos = enemy.transform.position + Vector3.up * jumpHeight - enemy.transform.forward * maxJumpDistance;
        StartCoroutine(LerpEnemy(enemy, targetPos, jumpDuration));
        //Infliger les dégâts à l'ennemi.
        enemy.TakeDamage(damage);
        //Désactiver le piège.
       // gameObject.SetActive(false);
        //Piège en attente de réarmement. 
        isActivated = false;
    }

    private IEnumerator LerpEnemy(Enemy enemy, Vector3 targetPos, float duration)
    {
        float time = 0;
        Vector3 startPos = enemy.transform.position;
        Vector3 peakPos = new Vector3(targetPos.x, startPos.y + jumpHeight, targetPos.z);
        while (time < duration)
        {
            float t = time / duration;
            // Use a parabolic curve for vertical movement
            float y = -4 * (t - 0.5f) * (t - 0.5f) + 1;
            // Use a linear curve for horizontal movement
            float xz = t;
            enemy.transform.position = new Vector3(
                Mathf.Lerp(startPos.x, peakPos.x, xz),
                Mathf.Lerp(startPos.y, peakPos.y, y),
                Mathf.Lerp(startPos.z, peakPos.z, xz)
            );
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
