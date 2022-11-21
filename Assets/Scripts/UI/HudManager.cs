using System;
using System.Collections;
using System.Collections.Generic;
using Projectiles;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class HudManager : MonoBehaviour
    {
        public PlayerHealth Hp { get; private set; }
        private List<BulletType> Bullets { get; set; } = new();
        [field: SerializeField] public Player Player { get; set; }
        [field: SerializeField] public GameObject BulletUiPrefab { get; set; }

        private GameObject _bulletUiContainer;
        private List<GameObject> _bulletUiList = new();
        private static readonly int SelectBulletTriggerHash = Animator.StringToHash(SelectBulletTrigger);
        private const string BulletUIContainerName = "Bullets";
        private const string BulletIconName = "Bullet";
        private const string SelectBulletTrigger = "SelectBullet";
        private const int BulletUiSize = 50;
        private const int BulletUiMargin = 10;

        private int _selectedBulletIndex = -1;

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
            AddBullets(Player.AvailableBullets);
            StartCoroutine(SelectBullet(Player.CurrentBullet));
        }

        private void AddBullets(List<BulletType> projectiles)
        {
            foreach (BulletType projectile in projectiles)
            {
                if (!Bullets.Contains(projectile))
                {
                    AddBullet(projectile);
                }
            }
        }

        private void AddBullet(BulletType projectile)
        {
            GameObject bulletUi = Instantiate(BulletUiPrefab, _bulletUiContainer.transform);

            bulletUi.transform.Find(BulletIconName).GetComponent<Image>().sprite = projectile.sprite;
            var leftOffset = BulletUiSize * Bullets.Count + BulletUiMargin * Bullets.Count;
            bulletUi.GetComponent<RectTransform>().anchoredPosition = new Vector2(leftOffset, 0);
            Bullets.Add(projectile);
            _bulletUiList.Add(bulletUi);
        }

        private IEnumerator SelectBullet(BulletType bulletType)
        {
            // Get the index of the bullet in the list
            var index = Bullets.IndexOf(bulletType);

            if (index == -1) yield break;
            if (_selectedBulletIndex == index) yield break;

            yield return new WaitForEndOfFrame();

            if (_selectedBulletIndex != -1)
            {
                var currentBulletUi = _bulletUiList[_selectedBulletIndex];
                var currentBulletAnimator = currentBulletUi.GetComponent<Animator>();
                currentBulletAnimator.SetBool(SelectBulletTriggerHash, false);
            }

            var bulletUi = _bulletUiList[index];
            var animator = bulletUi.GetComponent<Animator>();
            animator.SetBool(SelectBulletTriggerHash, true);
            _selectedBulletIndex = index;
        }
    }
}