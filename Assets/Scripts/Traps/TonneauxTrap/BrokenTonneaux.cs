using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokenTonneaux : MonoBehaviour
{
    public float timeBeforeDestroy = 2f;

    private float timer;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (timer >= timeBeforeDestroy)
        {
            Destroy(gameObject);
        }
        timer += Time.deltaTime;
    }
}