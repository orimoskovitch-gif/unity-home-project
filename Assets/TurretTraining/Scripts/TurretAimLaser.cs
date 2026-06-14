using System;
using UnityEngine;

namespace TurretTraining
{
    /// <summary>
    /// The TurretAimLaser behaviour provides a simple way to visualize a ray
    /// cast from the turret's muzzle towards some point in the distance.
    /// </summary>
    public sealed class TurretAimLaser
        : MonoBehaviour
    {
        [SerializeField] private LineRenderer _line;
        [SerializeField] private float _maxDistance;
        private Transform _source;
        public LineRenderer Line => _line;
        public float MaxDistance => _maxDistance;

        private void Start()
        {
            ValidateLine();
            _source = GetComponent<TurretStructure>().Muzzle;
        }

        private void Update()
        {
            var ray = new Ray(_source.position, _source.forward);
            var end = ray.GetPoint(_maxDistance);
            if (Physics.Raycast(ray, out var hitInfo))
                end = hitInfo.point;

            Line.SetPosition(0, _source.position);
            Line.SetPosition(1, end);
        }

        private void ValidateLine()
        {
            if (_line != null)
                return;

            enabled = false;
            throw new InvalidOperationException("attempted to start the aim laser without a line renderer!");
        }
    }
}