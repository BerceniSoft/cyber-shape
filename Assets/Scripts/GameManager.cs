using System;
using System.Collections;
using Constants;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Texture2D crosshairImg;
    private int pause  = 0;
    [SerializeField] private GameObject panou;
    
    [Header("Enemy settings")]
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private int constEnemies = 7;
    [SerializeField] private int maxWidth = 10;
    [SerializeField] private int maxDistance = 5;
    [SerializeField] private float secondsTillSpawn = 3f;
    [SerializeField] private float percentToNextWave = .75f;

    private bool _spawning = false;
    private AudioSource backgroundMusic;
    private void Start()
    {
        PlayerPrefs.SetInt(PlayerPrefsKeys.GamePause,pause);

        var hotSpot = new Vector2(crosshairImg.width / 2f, crosshairImg.height / 2f);
        Cursor.SetCursor(crosshairImg, hotSpot, CursorMode.Auto);
        backgroundMusic = GetComponent<AudioSource>();

        if (PlayerPrefs.HasKey(PlayerPrefsKeys.MusicState))
        {
            backgroundMusic.volume = PlayerPrefs.GetFloat(PlayerPrefsKeys.MusicState);
        }
        else
        {
            PlayerPrefs.SetFloat(PlayerPrefsKeys.MusicState, 0.5f);
            backgroundMusic.volume = 0.5f;
        }

        StartCoroutine(SpawnEnemies());
    }

    private void Update()
    {

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(pause == 0)
            {
                PauseGame();
            } 
            else
            {
                ResumeGame();
            }
                
        }

        if (!_spawning)
        {
            CheckNoEnemies();
        }
    }

    private void CheckNoEnemies()
    {
        if (GameObject.FindGameObjectsWithTag("Enemy").Length < percentToNextWave * constEnemies)
        {
            StartCoroutine(SpawnEnemies());
        }
    }
    
    private Vector3 GetRandomPosition()
    {
        while (true)
        {
            var randomPosition = new Vector3(Random.Range(-maxWidth, maxWidth), 0.5f, Random.Range(-maxWidth, maxWidth));
            var distance = Vector3.Distance(transform.position, randomPosition);
            if (distance < maxDistance)
            {
                continue;
            }

            return randomPosition;
        }
    }

    private IEnumerator SpawnEnemies()
    {
        _spawning = true;
        yield return new WaitForSeconds(secondsTillSpawn);
        for (var i = 0; i < constEnemies; i++)
        {
            Instantiate(enemyPrefab, GetRandomPosition(), Quaternion.identity);
        }

        _spawning = false;
    }


    public void PauseGame()
    {
        pause = 1;
        PlayerPrefs.SetInt(PlayerPrefsKeys.GamePause,pause);
        Time.timeScale = 0f;
        this.panou.SetActive(true);
    }
    public void ResumeGame()
    {
        pause = 0;
        PlayerPrefs.SetInt(PlayerPrefsKeys.GamePause,pause);
        Time.timeScale = 1f;
        this.panou.SetActive(false);
    }

    public void GoBackMain()
    {
        pause = 0;
        PlayerPrefs.SetInt(PlayerPrefsKeys.GamePause,pause);
        Time.timeScale = 1f;
        this.panou.SetActive(false);
        SceneManager.LoadScene((int) Scenes.MainMenuScene);
    }




}
