using UnityEngine;

namespace TurretTraining
{
    [CreateAssetMenu(fileName = "SessionPlaylist", menuName = "TurretTraining/Session Playlist")]
    public sealed class SessionPlaylist : ScriptableObject
    {
        public GameplayConfig[] Sessions;
    }
}
