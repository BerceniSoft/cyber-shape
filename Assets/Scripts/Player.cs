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

    [field: SerializeField]
    public List<BulletType>
        AvailableBullets { get; private set; } = new();

    [field: SerializeField] public BulletType CurrentBullet { get; set; }
    [field: SerializeField] public bool CanShoot { get; set; }

    [SerializeField] private HudManager ui;

    private ProjectileOrbitalController _orbitalController;
    private Coroutine _changeBulletCoroutine = null;

    private void Start()
    {
        _orbitalController = GetComponent<ProjectileOrbitalController>();
    }

    private void Update()
    {
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

    private void CheckStatus()
    {
        if (CurrentHealth <= 0)
        {
            Destroy(gameObject);
            SceneManager.LoadScene(0);
        }
    }

    public void SetBullet(BulletType bulletType)
    {
        this.CurrentBullet = bulletType;
        this.CanShoot = false;
        if (_changeBulletCoroutine != null)
        {
            StopCoroutine(_changeBulletCoroutine);
        }

        _changeBulletCoroutine =
            StartCoroutine(_orbitalController.ChangeBullet(
                bulletType,
                () => this.CanShoot = true)
            );
    }
}