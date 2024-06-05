using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Lobby
{
    public class InterractiveItem : MonoBehaviour
    {
        public GameObject arrow;
        public bool PlayButton;
        public bool OptionButton;
        public bool QuitButton;
        
        

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                arrow.GetComponent<Renderer>().material.color = Color.green;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                arrow.GetComponent<Renderer>().material.color = Color.white;
            }
        }

        private void Update()
        {
            
        }
        
        
    }
}
