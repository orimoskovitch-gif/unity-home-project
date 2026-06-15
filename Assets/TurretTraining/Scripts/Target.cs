using System;
using UnityEngine;

namespace TurretTraining
{
    /// <summary>
    /// The Target component provides information and the base functionality of
    /// a valid target in the scene.
    /// </summary>
    public sealed class Target : MonoBehaviour
    {
        [SerializeField] private TargetBoard _board;
        [SerializeField] private ParticleSystem _hitEffect;
        [SerializeField] private AudioClip _hitClip;

        public bool IsValidTarget => _board.gameObject.activeInHierarchy;
        public event EventHandler HitDetected;

        private void Awake()
        {
            _board.HitDetected += OnBoardHit;
        }

        public void Toggle(bool isValid)
        {
            _board.gameObject.SetActive(isValid);
        }

        private void OnBoardHit(object sender, EventArgs e)
        {
            // Play FX before deactivating the board so effects on the Target root
            // remain active. _hitEffect must NOT be a child of _board's GameObject.
            if (_hitEffect != null) _hitEffect.Play();

            if (_hitClip != null)
                AudioSource.PlayClipAtPoint(_hitClip, transform.position);

            Toggle(false);
            HitDetected?.Invoke(this, EventArgs.Empty);
        }
    }
}
