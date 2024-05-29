using System.Collections;
using System.Collections.Generic;
using Enemies;
using UnityEngine;

public class EnemyAnimation : MonoBehaviour
{
    public Enemy enemy; // Référence à l'objet parent Enemy

    void Awake()
    {
        enemy = GetComponentInParent<Enemy>();
    }
    
    public void OnDieAnimationEnd()
    {
        enemy.DestroyEnemy();
    }
}
