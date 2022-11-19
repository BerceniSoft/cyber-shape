using System;
using System.Collections.Generic;
using Projectiles;
using UnityEngine;
using UnityEngine.UIElements;

namespace UI
{
    public class HudManager : MonoBehaviour
    {
        public PlayerHealth Hp { get; private set; }
        public List<Projectile> Bullets { get; private set; } = new();
        public Player Player { get; set; }
        public GameObject BulletUiPrefab { get; set; }

        private GameObject _bulletUiContainer;
        private const string BulletUIContainerName = "Bullets";

        private void Awake()
        {
            Hp = GetComponentInChildren<PlayerHealth>();
        }

        private void Start()
        {
            _bulletUiContainer = GameObject.Find(BulletUIContainerName);
        }

        private void Update()
        {
            AddBullets(Player.AvailableProjectiles);
        }

        private void AddBullets(List<Projectile> projectiles)
        {
            foreach (Projectile projectile in projectiles)
            {
                if (!Bullets.Contains(projectile))
                {
                    AddBullet(projectile);
                }
            }
        }

        private void AddBullet(Projectile projectile)
        {
            Bullets.Add(projectile);

            GameObject bulletUi = Instantiate(BulletUiPrefab, _bulletUiContainer.transform);

            bulletUi.GetComponentsInChildren<Image>()[2].sprite = projectile.icon;
            // TODO: Set bullet position
        }
    }
}