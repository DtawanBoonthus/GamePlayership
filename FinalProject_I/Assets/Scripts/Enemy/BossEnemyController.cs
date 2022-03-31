using System;
using Spaceship;
using UnityEngine;

namespace Enemy
{
    public class BossEnemyController : MonoBehaviour
    {
        [SerializeField] private BossEnemyShip bossEnemyShip;

        private void Update()
        {
            bossEnemyShip.Fire();
        }
    }
}