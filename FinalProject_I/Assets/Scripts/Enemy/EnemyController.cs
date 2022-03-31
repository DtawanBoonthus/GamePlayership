using Manager;
using UnityEngine;
using Spaceship;

namespace Enemy
{
    public class EnemyController : MonoBehaviour
    {
        [SerializeField] private EnemySpaceship enemySpaceship;
        [SerializeField] private float chasingThresholdDistance;

        private PlayerSpaceship spawnedPlayerShip;

        private void Start()
        {
            spawnedPlayerShip = GameManager.Instance.spawnedPlayerShip;
        }

        private void Update()
        {
            MoveToPlayer();
            enemySpaceship.Fire();
        }

        private void MoveToPlayer()
        {
            if (spawnedPlayerShip == null)
            {
                return;
            }

            var distanceToPlayer = Vector2.Distance(spawnedPlayerShip.transform.position, transform.position);
            if (distanceToPlayer < chasingThresholdDistance)
            {
                var direction = (Vector2) (spawnedPlayerShip.transform.position - transform.position);
                direction.Normalize();
                var distance = direction * enemySpaceship.Speed * Time.deltaTime;
                gameObject.transform.Translate(distance);
            }
        }
    }
}