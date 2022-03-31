using System;
using Manager;
using UnityEngine;

namespace Spaceship
{
    public class EnemySpaceship : BaseSpaceship, IDamagable
    {
        public event Action OnExploded;

        [SerializeField] private double enemyFireRate = 0.5;
        private float fireCounter = 0f;
        
        private void Awake()
        {
            Debug.Assert(enemyFireRate > 0, "enemyFireRate has to be more than zero");            
        }
        
        public void Init(int hp, float speed)
        {
            base.Init(hp, speed, defaultBullet);
        }
        
        public void TakeHit(int damage)
        {
            Hp -= damage;

            if (Hp > 0)
            {
                return;
            }
            
            Explode();
        }

        public void Explode()
        {
            SoundManager.Instance.Play(SoundManager.Sound.EnemyExplode);
            
            Debug.Assert(Hp <= 0, "HP is more than zero");
            Destroy(gameObject);
            OnExploded?.Invoke();
        }

        public override void Fire()
        {
            fireCounter += Time.deltaTime;
            if (fireCounter >= enemyFireRate)
            {
                SoundManager.Instance.Play(SoundManager.Sound.EnemyFire);
                
                var bullet = PoolManager.Instance.GetPooledObject(PoolManager.PoolObjectType.EnemyBullet);
                if (bullet)
                {
                    bullet.transform.position = gunPosition.position;
                    bullet.transform.rotation = Quaternion.identity;
                    bullet.SetActive(true);
                    bullet.GetComponent<Bullet>().Init(Vector2.down);    
                }
                
                fireCounter = 0;
            }
        }
    }
}