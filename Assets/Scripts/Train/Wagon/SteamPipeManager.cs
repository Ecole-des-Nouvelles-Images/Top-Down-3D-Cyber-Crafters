using System;
using System.Collections.Generic;
using UnityEngine;

namespace Train.Wagon
{
    public class SteamPipeManager : MonoBehaviour
    {
        public Action OnAllSteamPipesDestroyed;
        
        [Header("Steam Pipes du Wagon")]
        public List<SteamPipe> localSteamPipes = new List<SteamPipe>(); // Steam Pipes du Wagon
        
        // Méthode appellée par les SteamPipes pour s'ajouter à la liste pour gérer le Wagon
        public void AddChild(SteamPipe child) { localSteamPipes.Add(child); }

        private void Update()
        {
            // On vérifie si tous les SteamPipes du Wagon sont détruits
            if (localSteamPipes.Count == 0)
            {
                // On désactive tous les pièges du Wagon qui nécessitent un changement d ActionMap
              
                OnAllSteamPipesDestroyed?.Invoke();
            }
        }
    }
}
