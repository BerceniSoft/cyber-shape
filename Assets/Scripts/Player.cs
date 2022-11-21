using System.Collections.Generic;
using Constants;
using Projectiles;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using Evolution;

public class Player : MonoBehaviour
{
    [field: SerializeField] public int CurrentHealth { get; private set; } = 4;
    [field: SerializeField] public int MaxHealth { get; private set; } = 4;

    [field: SerializeField] public bool IsRhythmActive { get; private set; } = true;

    [field: SerializeField]
    public List<BulletType>
        AvailableBullets { get; private set; } = new();

    [field: SerializeField]
    public BulletType CurrentBullet { get; set; }

    [SerializeField] private float speed = 5.0f;
    [SerializeField] private HudManager ui;
    [SerializeField] private float pityTime = 0.13f;
    [SerializeField] private int scoreEvolve = 5;

    private Rigidbody _rigidbody;
    private Camera _mainCamera;
    private ProjectileOrbitalController _orbitalController;
    private RhythmTimer _rTimer;
    private int _tempScore = 0;
    private Evolvable _evolution;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _rTimer = GetComponentInParent<RhythmTimer>();
        _orbitalController = GetComponent<ProjectileOrbitalController>();
        _evolution = GetComponent<Evolvable>();
        ui = GameObject.Find("HUD").GetComponent<HudManager>();
    }

    private void CheckHighScore()
    {
        if (!PlayerPrefs.HasKey(PlayerPrefsKeys.HighScore))
        {
            PlayerPrefs.SetInt(PlayerPrefsKeys.HighScore, 0);
        }
    }

    public void AddScore(int val)
    {
        _tempScore += val;
        if (PlayerPrefs.GetInt(PlayerPrefsKeys.HighScore) < _tempScore)
        {
            PlayerPrefs.SetInt(PlayerPrefsKeys.HighScore, _tempScore);
        }

        ui.ScoreUI.UpdateScore(_tempScore, PlayerPrefs.GetInt(PlayerPrefsKeys.HighScore));
        if (_tempScore % scoreEvolve == 0)
        {
            _evolution.Evolve();
        }
    }

    private void Start()
    {
        _mainCamera = Camera.main;
        CheckHighScore();
    }

    private void Update()
    {
        HandleShootInput();
        MovementControl();
        CheckStatus();
    }

    public void TakeDamage(int dmg)
    {
        CurrentHealth -= dmg;
    }

    public void UpdateMaxHealth(int max)
    {
        if (CurrentHealth + (max - MaxHealth) > 0)
            CurrentHealth += max - MaxHealth;

        MaxHealth = max;
        ui.Hp.DrawHealth();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Tags.Enemy))
        {
            TakeDamage(1);
        }
    }

    private void MovementControl()
    {
        var horizontal = Input.GetAxis("Horizontal");
        var vertical = Input.GetAxis("Vertical");
        _rigidbody.AddTorque(new Vector3(vertical / 3, 0, -horizontal / 3) * speed);
    }

    private void HandleShootInput()
    {
        if (Input.GetButtonDown("Fire1") && (_rTimer.CheckTime(pityTime) || !IsRhythmActive))
        {
            // Cast a ray from the camera to see where the click intersects with the floor
            var ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out var hit))
            {
                _orbitalController.EnqueueShoot(hit.point);
            }
        }
    }

    private void CheckStatus()
    {
        if (CurrentHealth <= 0)
        {
            PlayerPrefs.SetInt(PlayerPrefsKeys.CurrentScene, SceneManager.GetActiveScene().buildIndex);
            PlayerPrefs.Save();
            Destroy(gameObject);
            SceneManager.LoadScene((int) Scenes.GameOverMenu);
        }
    }
}
