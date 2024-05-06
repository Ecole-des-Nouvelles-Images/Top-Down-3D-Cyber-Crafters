using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    //private EnemyManager = _parentManager;

    public NavMeshAgent navMeshAgent;

    public Animator animator;

    public int healthPoints;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SlowDown()
    {
        navMeshAgent.isStopped = true;
        animator.SetBool("isSlowed", true);
    }

    public void ResetSpeed()
    {
        navMeshAgent.isStopped = false;
        animator.SetBool("isSlowed", false);
    }
}
