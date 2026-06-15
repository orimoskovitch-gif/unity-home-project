using UnityEngine;
using UnityEngine.SceneManagement;

namespace TurretTraining
{
    public sealed class MainMenuController : MonoBehaviour
    {
        [SerializeField] private string _gameSceneName = "GameScene";

        // Called by "Play" button onClick
        public void StartGame() => SceneManager.LoadScene(_gameSceneName);

        // Called by "Quit" button onClick
        public void Quit() => Application.Quit();
    }
}
