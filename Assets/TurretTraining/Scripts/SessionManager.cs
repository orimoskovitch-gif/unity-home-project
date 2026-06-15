using System;
using System.Collections.Generic;
using UnityEngine;

namespace TurretTraining
{
    public sealed class SessionManager : MonoBehaviour
    {
        public enum EndReason { TargetsDestroyed, TimerExpired, OutOfAmmo }

        [SerializeField] private Target[] _targets;
        [SerializeField] private TurretShooter _shooter;

        public float RemainingTime { get; private set; }
        public int RemainingShots { get; private set; }
        public int RemainingActiveTargets { get; private set; }
        public bool IsActive { get; private set; }

        public event EventHandler<SessionStats> SessionEnded;

        private TurretStructure _turretStructure;
        private float _sessionStartTime;
        private int _shotsFired;
        private int _targetsHit;
        private int _totalActiveTargets;
        private readonly List<Target> _subscribedTargets = new();

        private void Awake()
        {
            _turretStructure = _shooter.GetComponent<TurretStructure>();
        }

        public void BeginSession(GameplayConfig config)
        {
            Cleanup();

            _shooter.ShotFired += OnShotFired;
            RemainingTime = config.SessionDuration;
            RemainingShots = config.MaxShotCount;
            RemainingActiveTargets = 0;
            _shotsFired = 0;
            _targetsHit = 0;
            _totalActiveTargets = 0;
            _sessionStartTime = Time.time;

            for (var i = 0; i < _targets.Length; i++)
            {
                var isActive = i < config.ActiveTargets.Length && config.ActiveTargets[i];
                _targets[i].Toggle(isActive);
                if (!isActive) continue;
                RemainingActiveTargets++;
                _totalActiveTargets++;
                _targets[i].HitDetected += OnTargetHit;
                _subscribedTargets.Add(_targets[i]);
            }

            _turretStructure.enabled = true;
            _shooter.enabled = true;
            Cursor.lockState = CursorLockMode.Locked;
            IsActive = true;
        }

        private void Cleanup()
        {
            IsActive = false;
            _shooter.ShotFired -= OnShotFired;
            foreach (var t in _subscribedTargets)
                t.HitDetected -= OnTargetHit;
            _subscribedTargets.Clear();
        }

        private void Update()
        {
            if (!IsActive) return;
            RemainingTime -= Time.deltaTime;
            if (RemainingTime <= 0f)
            {
                RemainingTime = 0f;
                EndSession(EndReason.TimerExpired);
            }
        }

        private void OnShotFired(object sender, EventArgs e)
        {
            // Count the shot unconditionally — the hit is resolved before ShotFired fires
            // (TurretShooter raycasts first), so IsActive may already be false on the final
            // shot that destroys the last target. We still need to tally it.
            _shotsFired++;
            RemainingShots--;
            if (IsActive && RemainingShots <= 0)
                EndSession(EndReason.OutOfAmmo);
        }

        private void OnTargetHit(object sender, EventArgs e)
        {
            _targetsHit++;
            RemainingActiveTargets--;
            if (IsActive && RemainingActiveTargets <= 0)
                EndSession(EndReason.TargetsDestroyed);
        }

        private void EndSession(EndReason reason)
        {
            if (!IsActive) return;
            IsActive = false;
            _turretStructure.enabled = false;
            _shooter.enabled = false;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            var stats = new SessionStats(
                elapsedTime: Time.time - _sessionStartTime,
                shotsFired: _shotsFired,
                targetsHit: _targetsHit,
                totalActiveTargets: _totalActiveTargets,
                reason: reason);
            SessionEnded?.Invoke(this, stats);
        }
    }
}
