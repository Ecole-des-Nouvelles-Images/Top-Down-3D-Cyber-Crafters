using System.Collections.Generic;
using UnityEngine;

namespace Train.Wagon
{
    public class SteamPipeManager : MonoBehaviour
    {
        [Header("Steam Pipes du Wagon")]
        public List<SteamPipe> localSteamPipes = new List<SteamPipe>(); // Steam Pipes du Wagon
        
        // Méthode appellée par les SteamPipes pour s'ajouter à la liste pour gérer le Wagon
        public void AddChild(SteamPipe child) { localSteamPipes.Add(child); }
    }
}
