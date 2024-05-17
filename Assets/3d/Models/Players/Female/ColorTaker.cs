using System;
using UnityEngine;

namespace _3d.Models.Players.Female
{
    public class ColorTaker : MonoBehaviour
    {
        public SkinnedMeshRenderer parentMesh;
        
        private void Start()
        {
            GetComponent<SkinnedMeshRenderer>().material.color = parentMesh.materials[1].color;
        }
    }
}
