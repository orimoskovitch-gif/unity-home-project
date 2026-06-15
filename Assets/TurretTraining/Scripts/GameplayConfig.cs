using UnityEngine;

namespace TurretTraining
{
    [CreateAssetMenu(fileName = "GameplayConfig", menuName = "TurretTraining/Gameplay Config")]
    public sealed class GameplayConfig : ScriptableObject
    {
        [Min(1f)]
        public float SessionDuration = 60f;

        [Min(1)]
        public int MaxShotCount = 30;

        [Tooltip("One entry per target slot in SessionManager; true = active at session start")]
        public bool[] ActiveTargets;
    }
}
