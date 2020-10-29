using UnityEngine;

namespace Example
{
    public sealed class Rotator : MonoBehaviour
    {
        [SerializeField] private float _frequency = 1f;
        [SerializeField] private float _amplitude = 1f;

        private void FixedUpdate()
        {
            var y = Mathf.Sin(Time.fixedTime * _frequency) * _amplitude;
            transform.localRotation = Quaternion.Euler(0, y, 0);
        }
    }
}