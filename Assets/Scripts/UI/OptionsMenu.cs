using Constants;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OptionsMenu : MonoBehaviour
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

    public void OnMusic()
    {
        PlayerPrefs.SetFloat(PlayerPrefsKeys.MusicState, 0.5f);
        backgroundMusic.GetComponent<AudioSource>().volume = 0.5f;
    }

    public void OffMusic()
    {
        PlayerPrefs.SetFloat(PlayerPrefsKeys.MusicState, 0.0f);
        backgroundMusic.GetComponent<AudioSource>().volume = 0.0f;
    }
    public void Back()
    {
        SceneManager.LoadScene((int) Scenes.MainMenuScene);
    }

    /// Pentru cand vom implementa si sunetele gloantelor si ale transformarilor ( Al doilea buton de optiuni):
    // public void OnSfx()
    //public void OffSfx()
}
