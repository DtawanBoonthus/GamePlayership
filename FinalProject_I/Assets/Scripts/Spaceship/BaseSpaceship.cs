using UnityEngine;

namespace Spaceship
{
    public abstract class BaseSpaceship : MonoBehaviour
    {
        [SerializeField] protected Bullet defaultBullet;
        [SerializeField] protected Bullet defaultFireBullet;
        [SerializeField] protected Transform gunPosition;
        [SerializeField] protected Transform gunFireOnePosition;
        [SerializeField] protected Transform gunFireTwoPosition;

        protected AudioSource audioSource;
        
        public int Hp { get; protected set; }
        public float Speed { get; private set; }
        public Bullet Bullet { get; private set; }
        public Bullet BulletFire { get; private set; }

        private void Awake()
        {
            Debug.Assert(defaultBullet != null, "defaultBullet cannot be null");
            Debug.Assert(gunPosition != null, "gunPosition cannot be null");
        }        
        
        protected void Init(int hp, float speed, Bullet bullet)
        {
            Hp = hp;
            Speed = speed;
            Bullet = bullet;
        }

        protected void Init(int hp, Bullet bullet ,Bullet bulletFire)
        {
            Hp = hp;
            Bullet = bullet;
            BulletFire = bulletFire;
        }
        
        public abstract void Fire();
    }
}