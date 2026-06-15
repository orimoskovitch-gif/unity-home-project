using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TurretTraining
{
    public sealed class SessionController : MonoBehaviour
    {
        [SerializeField] private SessionPlaylist _playlist;
        [SerializeField] private SessionManager _manager;
        [SerializeField] private SessionHUD _hud;
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private AudioClip _sessionEndClip;

        private int _currentIndex;

        private void Start()
        {
            _manager.SessionEnded += OnSessionEnded;
            StartSession(0);
        }

        private void StartSession(int index)
        {
            _currentIndex = index;
            _hud.HideEndScreen();
            _manager.BeginSession(_playlist.Sessions[index]);
        }

        private void OnSessionEnded(object sender, SessionStats stats)
        {
            if (_sessionEndClip != null && _audioSource != null)
                _audioSource.PlayOneShot(_sessionEndClip);

            var isLast = _currentIndex >= _playlist.Sessions.Length - 1;
            _hud.ShowEndScreen(stats, isLast);
        }

        // Called by the "Next" button's onClick in the inspector
        public void OnNextSessionPressed()
        {
            var next = _currentIndex + 1;
            if (next < _playlist.Sessions.Length)
                StartSession(next);
        }

        // Called by the "Main Menu" button's onClick in the inspector
        public void OnReturnToMenuPressed()
        {
            SceneManager.LoadScene("MainMenu");
        }
    }
}
