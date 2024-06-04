using System;
using System.Collections.Generic;
using Train;
using UnityEngine;

namespace Gare {
    public class FakeWagon : MonoBehaviour {
        public int price;
        private static readonly int FresnelColor = Shader.PropertyToID("_fresnelColor");
        public bool buying;
        public bool buyable;
        private float _buyCooldownTimer;
        public GameObject wagonPrefab;
        public List<MeshRenderer> wagonRenderers = new List<MeshRenderer>();


        private void Update()
        {
            if (FindObjectOfType<TrainManager>().scrapsCount >= price) {
                foreach (MeshRenderer meshRenderer in wagonRenderers) {
                    for (int i = 0; i < meshRenderer.materials.Length; i++) {
                        meshRenderer.materials[i].SetColor(FresnelColor, Color.green);
                    }
                }
                buyable = true;
            }
            else if (FindObjectOfType<TrainManager>().scrapsCount < price) {
                foreach (MeshRenderer meshRenderer in wagonRenderers) {
                    for (int i = 0; i < meshRenderer.materials.Length; i++) {
                        meshRenderer.materials[i].SetColor(FresnelColor, Color.red);
                    }
                }
                buyable = false;
            }

            if (buying) { _buyCooldownTimer += Time.deltaTime; }
            if (_buyCooldownTimer >= 1f) { buying = false; }
        }
        

        public void OnBuy() {
            if (!buying)
            {
                buying = true;
                FindObjectOfType<TrainManager>().wagonPrefab = wagonPrefab;
                FindObjectOfType<TrainManager>().AddWagon();
                FindObjectOfType<TrainManager>().transform.position = new Vector3(0, 0,
                    FindObjectOfType<TrainManager>().transform.position.z - 30);
                FindObjectOfType<TrainManager>().scrapsCount -= price;
                FindObjectOfType<Station>().transform.position =
                    new Vector3(0, 0, FindObjectOfType<Station>().transform.position.z + 30);

            }
        }
    }
}
