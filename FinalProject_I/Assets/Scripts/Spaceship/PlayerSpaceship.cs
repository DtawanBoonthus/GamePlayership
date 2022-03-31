using System;
using Manager;
using UnityEngine;

namespace Spaceship
{
    public class PlayerSpaceship : BaseSpaceship, IDamagable
    {
        public event Action OnExploded;

        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
        }

        public void Init(int hp, float speed)
        {
            base.Init(hp, speed, defaultBullet);
        }

        public override void Fire()
        {
            SoundManager.Instance.Play(SoundManager.Sound.PlayerFire);
            
            var bullet = PoolManager.Instance.GetPooledObject(PoolManager.PoolObjectType.PlayerBullet);
            if (bullet)
            {
                bullet.transform.position = gunPosition.position;
                bullet.transform.rotation = Quaternion.identity;
                bullet.SetActive(true);
                bullet.GetComponent<Bullet>().Init(Vector2.up);                
            }            
            
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
            SoundManager.Instance.Play(SoundManager.Sound.PlayerExplode);
            Debug.Assert(Hp <= 0, "HP is more than zero");
            Destroy(gameObject);
            OnExploded?.Invoke();
        }
    }
}