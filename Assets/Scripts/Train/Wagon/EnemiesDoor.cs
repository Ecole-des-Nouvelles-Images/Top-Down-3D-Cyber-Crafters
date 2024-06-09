using System.Collections;
using System.Collections.Generic;
using Train.Wagon;
using UnityEngine;

public class EnemiesDoor : MonoBehaviour
{
    public Animator doorAnimator;
    // Start is called before the first frame update
    void Start()
    {
        SteamPipeManager steamPipeManager = FindObjectOfType<SteamPipeManager>();
        steamPipeManager.OnAllSteamPipesDestroyed += OpenDoor;
    }

    void OpenDoor()
    {
        doorAnimator.SetTrigger("wagonLost");
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
