using System;
using UnityEngine;
using UnityEngine.Events;

namespace TurretTraining
{
    /// <summary>
    /// The TargetBoard component represents a valid hit zone on the target.
    /// </summary>
    public sealed class TargetBoard
        : MonoBehaviour
    {
        [SerializeField] private UnityEvent _hit;
        public event EventHandler HitDetected;

        private void Awake()
        {
            _hit.AddListener(() => HitDetected?.Invoke(this, EventArgs.Empty));
        }
    }
}