using System.Collections;
using Constants;
using Evolution;
using UI;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private bool withEnemyWaves = true;
    [SerializeField] private Texture2D crosshairImg;
    [SerializeField] private GameObject panel;

    [Header("Enemy settings")]
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private float maxWidth;
    [SerializeField] private float secondsTillSpawn = 3f;
    [SerializeField] private float percentToNextWave = .75f;

    [Header("Level settings")]
    [SerializeField] private int waves = 3;
    [SerializeField] private int enemiesPerWave = 5;
    [SerializeField]
    private int level = 1;

    [Header("Pickups")]
    [SerializeField] private GameObject _hpPickup;
    [SerializeField] private GameObject _dmgPickup;
    [SerializeField] private GameObject _shieldPickup;
    [SerializeField] public int pickupTotal { get; set; }

    private bool _spawning = false;
    private int _pause = 0;
    private int _waveCount = 0;
    private HudManager _hudManager;
    private Player _player;
    private Vector3 _playerPosition;
    private Vector3 _floorSize;
    private Camera _camera;
    private int _levelIndex;
    private int _pickupCount;

    private void Awake()
    {
        _levelIndex = SceneManager.GetActiveScene().buildIndex - (int) Scenes.Level1;

        _hudManager = GameObject.Find("HUD").GetComponent<HudManager>();
        _player = GetComponentInChildren<Player>();
        _player.UnlockBulletsForLevel(_levelIndex + 1);
        _floorSize = GameObject.FindWithTag("Floor").GetComponent<MeshRenderer>().bounds.size;
        maxWidth = _floorSize.x / 3; 
        _camera = Camera.main;
        pickupTotal = 0;
        _pickupCount = 0;
    }

    private void Start()
    {
        PlayerPrefs.SetInt(PlayerPrefsKeys.GamePause, _pause);

        var hotSpot = new Vector2(crosshairImg.width / 2f, crosshairImg.height / 2f);
        Cursor.SetCursor(crosshairImg, hotSpot, CursorMode.Auto);

        _player.GetComponent<Evolvable>().Evolve(_levelIndex);

        if(_levelIndex == 4)
        {
            PlayerPrefs.SetInt(PlayerPrefsKeys.GameProgress, 100);
            // PopUp Msg (" Congrats!!") sau trigger Achivement.
        }

        if (withEnemyWaves)
        {
            NextWave();
        }
    }

    private void Update()
    {
        _playerPosition = _player.transform.position;
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (_pause == 0)
            {
                PauseGame();
            }
            else
            {
                ResumeGame();
            }
        }

        if (pickupTotal < 4)
        {
            SpawnPickup();
        }

        if (!_spawning && withEnemyWaves)
        {
            CheckNoEnemies();
        }
    }

    private void SpawnPickup()
    {
       // Instantiate(_hpPickup, new Vector3(Random.Range(-_floorSize.x/2, _floorSize.x / 2), 1.7f, Random.Range(-_floorSize.z / 2, _floorSize.z / 2)), Quaternion.identity);
       // pickupTotal++;
        switch (_pickupCount)
        {
            case 0: Instantiate(_hpPickup, new Vector3(Random.Range(-_floorSize.x/2, _floorSize.x / 2), 1.7f, Random.Range(-_floorSize.z / 2, _floorSize.z / 2)), Quaternion.identity); break;
            case 1: Instantiate(_dmgPickup, new Vector3(Random.Range(-_floorSize.x/2, _floorSize.x / 2), 1.7f, Random.Range(-_floorSize.z / 2, _floorSize.z / 2)), Quaternion.identity); break;
            case 2: Instantiate(_shieldPickup, new Vector3(Random.Range(-_floorSize.x/2, _floorSize.x / 2), 1.7f, Random.Range(-_floorSize.z / 2, _floorSize.z / 2)), Quaternion.identity); break;
        }
        _pickupCount = (_pickupCount + 1) % 3;
        pickupTotal++;
    }

    private void CheckNoEnemies()
    {
        if (GameObject.FindGameObjectsWithTag(Tags.Enemy).Length < percentToNextWave * enemiesPerWave)
        {
            NextWave();
        }
    }

    private Vector3 GetRandomPosition()
    {
        
        while (true)
        {
            // get random point around the player
            var randomPosition =
                new Vector3(Random.Range(_playerPosition.x - maxWidth, _playerPosition.x + maxWidth), 0.5f, 
                    Random.Range(_playerPosition.z - maxWidth, _playerPosition.z + maxWidth));
            
            // check if the point is inside the map
            if (randomPosition.x < -_floorSize.x / 2 || randomPosition.x > _floorSize.x / 2 ||
                randomPosition.z < -_floorSize.z / 2 || randomPosition.z > _floorSize.z / 2)
            {
                continue;
            }
            
            // check if the point is outside camera view
            if (Mathf.Abs(randomPosition.x - _playerPosition.x) < _camera.orthographicSize ||
                Mathf.Abs(randomPosition.z - _playerPosition.z) < _camera.orthographicSize * _camera.aspect)
            {
                continue;
            }

            if (Physics.CheckSphere(randomPosition, 0.7f, 7))
            {
                continue;
            }

            return randomPosition;
        }
    }

    private void NextWave()
    {
        if (_waveCount + 1 > (_levelIndex + 1) * waves)
        {
            if(PlayerPrefs.GetInt(PlayerPrefsKeys.GameMode) == 0)
            {
                if(_levelIndex == 0)
                {
                    PlayerPrefs.SetInt(PlayerPrefsKeys.GameProgress, 20);
                }
                else if(_levelIndex == 1)
                {
                    PlayerPrefs.SetInt(PlayerPrefsKeys.GameProgress, 40);
                }
                else if(_levelIndex == 2)
                {
                    PlayerPrefs.SetInt(PlayerPrefsKeys.GameProgress, 60);
                }
                else if(_levelIndex == 3)
                {
                    PlayerPrefs.SetInt(PlayerPrefsKeys.GameProgress, 80);
                }
                ProgressToNextLevel();                
            }
            else if(PlayerPrefs.GetInt(PlayerPrefsKeys.GameMode) == 1)
            {
                _waveCount = 1;
            }

        }
        else
        {
            _waveCount++;
            _hudManager.WavesUI.UpdateWaves((_levelIndex + 1) * waves - _waveCount + 1);
            StartCoroutine(SpawnEnemies());
        }
    }

    private IEnumerator SpawnEnemies()
    {
        _spawning = true;
        yield return new WaitForSeconds(secondsTillSpawn);
        for (var i = 0; i < enemiesPerWave; i++)
        {
            var enemyObject = Instantiate(enemyPrefab, GetRandomPosition(), Quaternion.identity);

            // Evolve to random stage (max is determined by current level).
            var enemyEvolvable = enemyObject.GetComponentInChildren<Evolvable>();
            var stageIndex = Random.Range(0, _levelIndex + 1);
            enemyEvolvable.Evolve(stageIndex);

            _hudManager.FollowEnemy(enemyObject);
        }

        _spawning = false;
    }

    private void ProgressToNextLevel()
    {
        var activeSceneIndex = SceneManager.GetActiveScene().buildIndex;
        if (activeSceneIndex < (int) Scenes.Level5)
        {
            PlayerPrefs.SetInt(PlayerPrefsKeys.CurrentScene, activeSceneIndex + 1);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }

    private void PauseGame()
    {
        _pause = 1;
        PlayerPrefs.SetInt(PlayerPrefsKeys.GamePause, _pause);
        Time.timeScale = 0f;
        panel.SetActive(true);
    }

    public void ResumeGame()
    {
        _pause = 0;
        PlayerPrefs.SetInt(PlayerPrefsKeys.GamePause, _pause);
        Time.timeScale = 1f;
        panel.SetActive(false);
    }

    public void GoBackMain()
    {
        _pause = 0;
        PlayerPrefs.SetInt(PlayerPrefsKeys.GamePause, _pause);
        Time.timeScale = 1f;
        panel.SetActive(false);
        SceneManager.LoadScene((int)Scenes.MainMenuScene);
    }
}