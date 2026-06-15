using System;
using UnityEngine;

namespace TurretTraining
{
    /// <summary>
    /// The TargetBoard component represents a valid hit zone on the target.
    /// </summary>
    public sealed class TargetBoard : MonoBehaviour
    {
        public event EventHandler HitDetected;

        public void RegisterHit() => HitDetected?.Invoke(this, EventArgs.Empty);
    }
}