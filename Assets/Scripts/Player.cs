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

    [field: SerializeField]
    public BulletType CurrentBullet { get; set; }

    [SerializeField] private HudManager ui;
    
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
}