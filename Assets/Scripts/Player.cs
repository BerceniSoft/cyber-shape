using System.Collections.Generic;
using Constants;
using Projectiles;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [field: SerializeField] public int CurrentHealth { get; private set; } = 4;
    [field: SerializeField] public int MaxHealth { get; private set; } = 4;

    [field: SerializeField] public bool IsRhythmActive { get; private set; } = true;

    [field: SerializeField]
    public List<Projectile>
        AvailableProjectiles { get; private set; } = new();
    
    [SerializeField] private float speed = 5.0f;
    [SerializeField] private HudManager ui;

    private Rigidbody _rigidbody;
    private Camera _mainCamera;
    private ProjectileOrbitalController _orbitalController;
    private RhythmTimer _rTimer;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _rTimer = GetComponentInParent<RhythmTimer>();
        _orbitalController = GetComponent<ProjectileOrbitalController>();
    }

    private void Start()
    {
        _mainCamera = Camera.main;
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
        if (Input.GetButtonDown("Fire1") && (_rTimer.CheckTime(0.10f) || !IsRhythmActive))
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
            Destroy(gameObject);
            SceneManager.LoadScene(0);
        }
    }
}