using UnityEngine;

namespace Proto.Script.Trap{
    public class Trap : MonoBehaviour {
        public MeshRenderer buttonMeshRenderer;
        public GameObject dangerZone;
        public bool alreadyPressed;
        private float _cooldownTime = 5f;
        private float _timer;

        private void Awake() {
            dangerZone.SetActive(false);
        }
        private void Update() { 
            if (alreadyPressed) { _timer += Time.deltaTime; }
            if (_timer >= _cooldownTime) {
                buttonMeshRenderer.material.color = Color.red;
                alreadyPressed = false;
                dangerZone.SetActive(false);
                _timer = 0;
            }
        }
        public void OnTrap() {
            if (!alreadyPressed) {
                dangerZone.SetActive(true);
                buttonMeshRenderer.material.color = Color.green;
                alreadyPressed = true;
                _timer = 0;
            }
        }
    }
}
