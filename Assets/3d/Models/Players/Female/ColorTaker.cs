using System;
using UnityEngine;

namespace _3d.Models.Players.Female
{
    public class ColorTaker : MonoBehaviour
    {
        public Renderer parentMesh;
        
        private void Start()
        {
            GetComponent<Renderer>().material.color = parentMesh.materials[1].color;
        }
    }
}
