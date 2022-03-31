using System;
using Manager;
using UnityEngine;

namespace Spaceship
{
    public class BossEnemyShip : BaseSpaceship, IDamagable
    {
        public event Action OnExploded;

        private double enemyFireRate = 0.5;
        private double enemyFireBulletRate = 1;
        private float fireCounter = 0f;
        private float fireBulletCounter = 0f;

        private void Awake()
        {
            Debug.Assert(enemyFireRate > 0, "enemyFireRate has to be more than zero");
            Debug.Assert(defaultFireBullet != null, "defaultFireBullet cannot be null");
            Debug.Assert(gunFireOnePosition != null, "gunFireOnePosition cannot be null");
            Debug.Assert(gunFireTwoPosition != null, "gunFireTwoPosition cannot be null");
        }
        
        public void Init(int hp)
        {
            base.Init(hp, defaultBullet, defaultFireBullet);
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
            SoundManager.Instance.Play(SoundManager.Sound.BossEnemyExplode);
            
            Debug.Assert(Hp <= 0, "HP is more than zero");
            Destroy(gameObject);
            OnExploded?.Invoke();
        }

        public override void Fire()
        {
            fireCounter += Time.deltaTime;
            
            fireBulletCounter += Time.deltaTime;
            
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

            if (fireBulletCounter >= enemyFireBulletRate)
            {
                SoundManager.Instance.Play(SoundManager.Sound.BossEnemyFire);
                
                var bulletFireOne = PoolManager.Instance.GetPooledObject(PoolManager.PoolObjectType.BossEnemyBullet);
                if (bulletFireOne)
                {
                    bulletFireOne.transform.position = gunFireOnePosition.position;
                    bulletFireOne.transform.rotation = Quaternion.identity;
                    bulletFireOne.SetActive(true);
                    bulletFireOne.GetComponent<Bullet>().Init(Vector2.down);    
                }
                
                var bulletFireTwo = PoolManager.Instance.GetPooledObject(PoolManager.PoolObjectType.BossEnemyBullet);
                if (bulletFireTwo)
                {
                    bulletFireTwo.transform.position = gunFireTwoPosition.position;
                    bulletFireTwo.transform.rotation = Quaternion.identity;
                    bulletFireTwo.SetActive(true);
                    bulletFireTwo.GetComponent<Bullet>().Init(Vector2.down);    
                }

                fireBulletCounter = 0;
            }
        }
    }
}