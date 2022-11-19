using UnityEngine;

namespace Projectiles
{
    public class Projectile : MonoBehaviour
    {
        public const float ProjectileHeight = 0.1f;
        public int Damage { get; private set; } = 100;
        public Transform gapTransform;
        public bool QueuedForShooting { get; set; } = false;
        public Sprite icon;

        private Bullet _bullet;

        private void Awake()
        {
            _bullet = GetComponentInChildren<Bullet>();
        }

        private bool ShouldOrbit()
        {
            return _bullet.ShouldOrbit();
        }

        public bool CanShoot()
        {
            return ShouldOrbit() && !QueuedForShooting;
        }

        public void Shoot(Vector3 target)
        {
            _bullet.Shoot(target);
            QueuedForShooting = false;
        }

        public void OrbitAround(Vector3 point, float orbitSpeed)
        {
            gapTransform.RotateAround(point, Vector3.up, 20 * Time.deltaTime * orbitSpeed);
            if (ShouldOrbit())
            {
                _bullet.transform.RotateAround(point, Vector3.up, 20 * Time.deltaTime * orbitSpeed);
            }
        }

        public void UpdateProjectilePosition(Vector3 deltaPos)
        {
            gapTransform.position += deltaPos;
            if (ShouldOrbit())
            {
                _bullet.transform.position += deltaPos;
            }
        }

        public void Init(Vector3 position, Quaternion rotation)
        {
            _bullet.transform.localPosition = position;
            _bullet.transform.localRotation = rotation;
            gapTransform.localPosition = position;
            gapTransform.localRotation = rotation;
        }

        public Vector3 GetBulletPosition()
        {
            return _bullet.transform.position;
        }
    }
}