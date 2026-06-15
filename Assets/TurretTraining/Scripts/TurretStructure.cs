using UnityEngine;

namespace TurretTraining
{
    /// <summary>
    /// The TurretStructure component provides information and references to
    /// the different parts of a turret.
    /// </summary>
    public sealed class TurretStructure : MonoBehaviour
    {
        [SerializeField] private Transform _muzzle;
        [SerializeField] private Transform _yawPivot;
        [SerializeField] private Transform _pitchPivot;
        [SerializeField] private float _keyboardSpeed = 20f;
        [SerializeField] private float _mouseSensitivity = 2f;
        [SerializeField] private bool _invertMouseY;

        public Transform Muzzle => _muzzle;
        public Transform YawPivot => _yawPivot;
        public Transform PitchPivot => _pitchPivot;

        // Accumulated angles — avoids the euler read-modify-write pitfall (Unity normalises
        // localEulerAngles to [0,360) on every read, causing drift and gimbal jitter).
        private float _yaw;
        private float _pitch;

        private void Start()
        {
            _yaw = _yawPivot.localEulerAngles.y;
            _pitch = _pitchPivot.localEulerAngles.z;
            Cursor.lockState = CursorLockMode.Locked;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
                Cursor.lockState = Cursor.lockState == CursorLockMode.Locked
                    ? CursorLockMode.None
                    : CursorLockMode.Locked;

            _yaw += GetMoveDirection(KeyCode.LeftArrow, KeyCode.RightArrow) * Time.deltaTime * _keyboardSpeed
                    + Input.GetAxis("Mouse X") * _mouseSensitivity;

            var mouseYSign = _invertMouseY ? 1f : -1f;
            _pitch += GetMoveDirection(KeyCode.DownArrow, KeyCode.UpArrow) * Time.deltaTime * _keyboardSpeed
                      + Input.GetAxis("Mouse Y") * _mouseSensitivity * mouseYSign;

            _yawPivot.localRotation   = Quaternion.Euler(0f, _yaw,   0f);
            _pitchPivot.localRotation = Quaternion.Euler(0f, 0f,   _pitch);
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
