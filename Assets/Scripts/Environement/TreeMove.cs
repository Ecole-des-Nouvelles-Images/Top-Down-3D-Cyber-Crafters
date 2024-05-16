using System;
using UnityEngine;

namespace Environement
{
    public class TreeMove : MonoBehaviour
    {
        private float _timer;

        private void Update()
        {
            _timer += Time.deltaTime;
            transform.Translate(new Vector3(0,0,-0.5f));
            if (_timer >= 3)
            {
                Destroy(gameObject);
            }
        }
    }
}
