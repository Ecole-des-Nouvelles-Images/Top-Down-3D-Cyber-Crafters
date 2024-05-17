
using DG.Tweening;
using UnityEngine;

namespace Environement
{
    public class RandomCameraShake : MonoBehaviour
    {
        private float _timer;

        private void Awake()
        {
            _timer = 0;
        }

        private void FixedUpdate()
        {
            if (_timer >= 5)
            {
                transform.localPosition = new Vector3(28.5f, 16, -26.5f);
                transform.DOKill();
                _timer -= _timer;
            }

            transform.DOShakePosition(1f, new Vector3(0.025f, 0.01f, 0.015f), 13, 0, false, false).SetEase(Ease.Linear)
                .SetAutoKill(false);
            _timer += Time.deltaTime;
        }
    }
}
