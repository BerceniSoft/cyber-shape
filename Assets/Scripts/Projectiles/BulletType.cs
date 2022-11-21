using UnityEngine;

namespace Projectiles
{
    [CreateAssetMenu(fileName = "BulletType", menuName = "BulletType")]
    public class BulletType : ScriptableObject
    {
        public enum BulletTypes
        {
            NORMAL,
            EXPLOSIVE,
            PIERCING,
            BOOMERANG
        }

        public Sprite sprite;
        public BulletTypes bulletType;
    }
}