using UnityEngine;

namespace TurretTraining
{
    /// <summary>
    /// The TurretStructure component provides information and references to
    /// the different parts of a turret.
    /// </summary>
    public sealed class TurretStructure
        : MonoBehaviour
    {
        [SerializeField] private Transform _muzzle;
        [SerializeField] private Transform _yawPivot;
        [SerializeField] private Transform _pitchPivot;
        public Transform Muzzle => _muzzle;
        public Transform YawPivot => _yawPivot;
        public Transform PitchPivot => _pitchPivot;

        private void Update()
        {
            var yaw = GetMoveDirection(KeyCode.LeftArrow, KeyCode.RightArrow);
            var yawAngles = _yawPivot.localEulerAngles;
            yawAngles.y += Time.deltaTime*yaw*20.0f;
            _yawPivot.localEulerAngles = yawAngles;

            var pitch = GetMoveDirection(KeyCode.DownArrow, KeyCode.UpArrow);
            var pitchAngles = _pitchPivot.localEulerAngles;
            pitchAngles.z += Time.deltaTime*pitch*20.0f;
            _pitchPivot.localEulerAngles = pitchAngles;
        }

        private static float GetMoveDirection(KeyCode neg, KeyCode pos)
        {
            var direction = 0.0f;
            if (Input.GetKey(neg))
                direction -= 1.0f;

            if (Input.GetKey(pos))
                direction += 1.0f;
            return direction;
        }
    }
}