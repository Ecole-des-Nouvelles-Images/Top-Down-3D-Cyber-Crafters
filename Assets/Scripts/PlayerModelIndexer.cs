using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerModelIndexer : MonoBehaviourSingleton<PlayerModelIndexer>
{
    public  int player1ModelId;
    public  int player2ModelId;
    public  int player3ModelId;
    public  int player4ModelId;

    private void FixedUpdate()
    {
        if (SceneManager.GetSceneByName("Lobby").isLoaded)
        {
            DestroyImmediate(this.gameObject);
        }
    }
}
