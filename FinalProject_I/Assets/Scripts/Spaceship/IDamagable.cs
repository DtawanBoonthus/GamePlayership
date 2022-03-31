using System;

namespace Spaceship
{
    public interface IDamagable
    {
        event Action OnExploded;
        void TakeHit(int damage);
        void Explode();
    }
}