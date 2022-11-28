using Constants;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField]
        private GameObject backgroundMusic;
        // Start is called before the first frame update
        void Start()
        {
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

        public void Play() 
        {
            if (PlayerPrefs.HasKey(PlayerPrefsKeys.CurrentScene) &&  PlayerPrefs.GetInt(PlayerPrefsKeys.CurrentScene) != (int) Scenes.MainMenuScene ) 
            {
                SceneManager.LoadScene(PlayerPrefs.GetInt(PlayerPrefsKeys.CurrentScene));
            } 
            else 
            {
                PlayerPrefs.SetInt(PlayerPrefsKeys.CurrentScene, (int) Scenes.Level1);
                SceneManager.LoadScene((int) Scenes.Level1);
            }
        }
        public void GoToOptions()
        {
            SceneManager.LoadScene((int) Scenes.OptionsMenu);
        }
        
        public void ExitGame()
        {
            Application.Quit();
        }
    }

}

