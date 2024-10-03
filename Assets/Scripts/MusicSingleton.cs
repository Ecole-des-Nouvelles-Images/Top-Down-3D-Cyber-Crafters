using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicSingleton : MonoBehaviourSingleton<MusicSingleton>
{
    private void Update()
    {
        if (SceneManager.GetSceneByName("GameScene").isLoaded)
        {
            DestroyImmediate(this.gameObject);
        }
    }
}
