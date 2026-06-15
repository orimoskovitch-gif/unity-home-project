namespace TurretTraining
{
    public readonly struct SessionStats
    {
        public readonly float ElapsedTime;
        public readonly int ShotsFired;
        public readonly int TargetsHit;
        public readonly int TotalActiveTargets;
        public readonly SessionManager.EndReason Reason;

        public float Accuracy => ShotsFired > 0 ? (float)TargetsHit / ShotsFired : 0f;

        public SessionStats(
            float elapsedTime,
            int shotsFired,
            int targetsHit,
            int totalActiveTargets,
            SessionManager.EndReason reason)
        {
            ElapsedTime = elapsedTime;
            ShotsFired = shotsFired;
            TargetsHit = targetsHit;
            TotalActiveTargets = totalActiveTargets;
            Reason = reason;
        }
    }
}
