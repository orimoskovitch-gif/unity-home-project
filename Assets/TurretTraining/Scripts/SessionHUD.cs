using UnityEngine;
using UnityEngine.UI;

namespace TurretTraining
{
    public sealed class SessionHUD : MonoBehaviour
    {
        [Header("Live HUD")]
        [SerializeField] private SessionManager _session;
        [SerializeField] private Text _timerText;
        [SerializeField] private Text _shotsText;
        [SerializeField] private Text _targetsText;

        [Header("End Screen")]
        [SerializeField] private GameObject _endPanel;
        [SerializeField] private Text _endReasonText;
        [SerializeField] private Text _durationText;
        [SerializeField] private Text _shotsFiredText;
        [SerializeField] private Text _targetsHitText;
        [SerializeField] private Text _accuracyText;
        [SerializeField] private Button _nextButton;
        [SerializeField] private Button _menuButton;

        // Cache last-displayed values so Update() only writes Text.text (which triggers a
        // mesh rebuild) when the visible content actually changes.
        private float _lastRoundedTime = float.NaN;
        private int _lastShots = -1;
        private int _lastTargets = -1;

        private void Awake()
        {
            // Fallback discovery in case Inspector wiring was lost (e.g. stale compiled DLL
            // that didn't yet include the field when the scene was serialised).
            if (_endPanel != null)
            {
                if (_nextButton == null)
                    _nextButton = _endPanel.transform.Find("NextButton")?.GetComponent<Button>();
                if (_menuButton == null)
                    _menuButton = _endPanel.transform.Find("MenuButton")?.GetComponent<Button>();
            }
            ValidateFields();
            if (_endPanel != null) _endPanel.SetActive(false);
        }

        private void ValidateFields()
        {
            if (_timerText   == null) Debug.LogError("SessionHUD: _timerText is not assigned",   this);
            if (_shotsText   == null) Debug.LogError("SessionHUD: _shotsText is not assigned",   this);
            if (_targetsText == null) Debug.LogError("SessionHUD: _targetsText is not assigned", this);
            if (_endPanel    == null) Debug.LogError("SessionHUD: _endPanel is not assigned",    this);
            if (_endReasonText  == null) Debug.LogError("SessionHUD: _endReasonText is not assigned",  this);
            if (_durationText   == null) Debug.LogError("SessionHUD: _durationText is not assigned",   this);
            if (_shotsFiredText == null) Debug.LogError("SessionHUD: _shotsFiredText is not assigned", this);
            if (_targetsHitText == null) Debug.LogError("SessionHUD: _targetsHitText is not assigned", this);
            if (_accuracyText   == null) Debug.LogError("SessionHUD: _accuracyText is not assigned",   this);
            if (_nextButton  == null) Debug.LogError("SessionHUD: _nextButton is not assigned",  this);
            if (_menuButton  == null) Debug.LogError("SessionHUD: _menuButton is not assigned",  this);
        }

        private void Update()
        {
            if (_timerText != null)
            {
                var t = _session.RemainingTime;
                var rounded = Mathf.Round(t * 10f) / 10f;
                if (rounded != _lastRoundedTime)
                {
                    _timerText.text = $"Time: {t:F1}s";
                    _lastRoundedTime = rounded;
                }
            }

            if (_shotsText != null)
            {
                var s = _session.RemainingShots;
                if (s != _lastShots)
                {
                    _shotsText.text = $"Shots: {s}";
                    _lastShots = s;
                }
            }

            if (_targetsText != null)
            {
                var tg = _session.RemainingActiveTargets;
                if (tg != _lastTargets)
                {
                    _targetsText.text = $"Targets: {tg}";
                    _lastTargets = tg;
                }
            }
        }

        public void ShowEndScreen(SessionStats stats, bool isLast)
        {
            if (_endReasonText != null)
                _endReasonText.text = stats.Reason switch
                {
                    SessionManager.EndReason.TargetsDestroyed => "All targets destroyed!",
                    SessionManager.EndReason.TimerExpired     => "Time's up!",
                    SessionManager.EndReason.OutOfAmmo        => "Out of ammo!",
                    _                                         => "Session over"
                };

            if (_durationText   != null) _durationText.text   = $"Time:     {stats.ElapsedTime:F1}s";
            if (_shotsFiredText != null) _shotsFiredText.text = $"Shots:    {stats.ShotsFired}";
            if (_targetsHitText != null) _targetsHitText.text = $"Targets:  {stats.TargetsHit} / {stats.TotalActiveTargets}";
            if (_accuracyText   != null) _accuracyText.text   = $"Accuracy: {stats.Accuracy:P0}";

            if (_nextButton != null) _nextButton.gameObject.SetActive(!isLast);
            if (_menuButton != null) _menuButton.gameObject.SetActive(isLast);
            if (_endPanel   != null) _endPanel.SetActive(true);
        }

        public void HideEndScreen()
        {
            if (_endPanel != null) _endPanel.SetActive(false);
        }
    }
}
