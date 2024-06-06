using System;
using System.Collections.Generic;
using UnityEngine;

namespace Lobby
{
    public class LobbyManager : MonoBehaviour
    {
        public List<GameObject> characters = new List<GameObject>();
        public LobbyCharacterController characterController;

        private void Start()
        {
            for (int i = 0; i < characters.Count; i++)
            {
                if (i == characterController.playerModelId)
                {
                    characters[i].SetActive(false);
                }
            }
        }
    }
}
