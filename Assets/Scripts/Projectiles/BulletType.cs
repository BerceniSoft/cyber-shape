using UnityEngine;

namespace Projectiles
{
    [CreateAssetMenu(fileName = "BulletType", menuName = "BulletType")]
    public class BulletType : ScriptableObject
    {
        public enum BulletTypes
        {
            ARROW,
            DART,
            GRENADE,
            BULLET,
            SPEAR
        }

        public Sprite sprite;
        public BulletTypes bulletType;
        public Mesh mesh;
    }
}