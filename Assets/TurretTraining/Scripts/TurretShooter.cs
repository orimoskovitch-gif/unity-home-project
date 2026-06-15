using System;
using UnityEngine;

namespace TurretTraining
{
    /// <summary>
    /// Reads shoot input and performs a raycast from the turret muzzle.
    /// Must be on the same GameObject as TurretStructure.
    /// </summary>
    public sealed class TurretShooter : MonoBehaviour
    {
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private AudioClip _shootClip;
        [SerializeField] private ParticleSystem _muzzleFlash;

        public event EventHandler ShotFired;

        private Transform _muzzle;

        private void Start()
        {
            _muzzle = GetComponent<TurretStructure>().Muzzle;
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
                Shoot();
        }

        public void Shoot()
        {
            if (_shootClip != null && _audioSource != null)
                _audioSource.PlayOneShot(_shootClip);

            if (_muzzleFlash != null) _muzzleFlash.Play();

            // Raycast before firing ShotFired so a simultaneous last-target kill is
            // fully counted before ammo-depletion ends the session.
            var ray = new Ray(_muzzle.position, _muzzle.forward);
            if (Physics.Raycast(ray, out var hit))
                hit.collider.GetComponent<TargetBoard>()?.RegisterHit();

            ShotFired?.Invoke(this, EventArgs.Empty);
        }
    }
}
