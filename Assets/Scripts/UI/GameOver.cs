using Constants;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

namespace UI
{
    public class GameOver : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text scoreText;
        [SerializeField]
        private GameObject backgroundMusic;

        private void Start()
        {
            scoreText.text = "High Score: " + PlayerPrefs.GetInt(PlayerPrefsKeys.HighScore);
            if (PlayerPrefs.HasKey(PlayerPrefsKeys.MusicState))
            {
                backgroundMusic.GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat(PlayerPrefsKeys.MusicState);
            }
            else
            {
                PlayerPrefs.SetFloat(PlayerPrefsKeys.MusicState, 0.5f);
                backgroundMusic.GetComponent<AudioSource>().volume = 0.5f;
            }
        }

        public void RestartLevel() 
        {
            if (PlayerPrefs.HasKey(PlayerPrefsKeys.CurrentScene)) 
            {
                SceneManager.LoadScene(PlayerPrefs.GetInt(PlayerPrefsKeys.CurrentScene));
            } 
            else 
            {
                SceneManager.LoadScene((int) Scenes.Level1);
            }
        }

        public void MainMenu()
        {
            SceneManager.LoadScene((int) Scenes.MainMenuScene);
        }
        
        public void ExitGame()
        {
            Application.Quit();
        }
    }
}
