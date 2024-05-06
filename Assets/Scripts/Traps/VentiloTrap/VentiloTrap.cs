using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VentiloTrap : Trap
{
    public ParticleSystem ventiloParticleSystem;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            
        }
    }

    public override void TriggerTrap()
    {
        
    }
    
    
}
