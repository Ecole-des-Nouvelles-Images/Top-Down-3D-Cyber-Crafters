using System;
using UnityEngine;

namespace Scenes
{
    public class InvisibleWallScript : MonoBehaviour
    {
        public bool left;
        public bool right;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                if (left)
                {
                    Vector3 goback = new Vector3(other.transform.position.x - this.transform.position.x, other.transform.position.y, other.transform.position.z);
                    other.transform.Translate(goback * 1 * Time.deltaTime);
                }
                else if (right)
                {
                    Vector3 goback = new Vector3(other.transform.position.x - this.transform.position.x, other.transform.position.y, other.transform.position.z);
                    other.transform.Translate(goback * 1 * Time.deltaTime);
                }
            }
        }
    }
}
