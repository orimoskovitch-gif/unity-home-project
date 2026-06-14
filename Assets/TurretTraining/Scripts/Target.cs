using UnityEngine;

namespace TurretTraining
{
    /// <summary>
    /// The Target component provides information and the base functionality of
    /// a valid target in the scene.
    /// </summary>
    public sealed class Target
        : MonoBehaviour
    {
        [SerializeField] private TargetBoard _board;
        public bool IsValidTarget => _board.gameObject.activeSelf;

        public void Toggle(bool isValid)
        {
            Debug.Log($"TogglingTargetState: target={name}, isValid={isValid}");
            _board.gameObject.SetActive(isValid);
        }
    }
}