using System;
using Spaceship;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Manager
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private PlayerSpaceship playerSpaceship;
        [SerializeField] private EnemySpaceship enemySpaceship;
        [SerializeField] private BossEnemyShip bossEnemyShip;
        [SerializeField] private int playerSpaceshipHp;
        [SerializeField] private int playerSpaceshipMoveSpeed;
        [SerializeField] private int enemySpaceshipHp;
        [SerializeField] private int enemySpaceshipMoveSpeed;
        [SerializeField] private int bossEnemyShipHp;

        [HideInInspector] public PlayerSpaceship spawnedPlayerShip;

        public event Action OnRestarted;
        public event Action OnSpawnBoss;
        public event Action OnBossEnemyExploded;

        public static GameManager Instance { get; private set; }

        private void Awake()
        {
            Debug.Assert(playerSpaceship != null, "playerSpaceship cannot be null");
            Debug.Assert(enemySpaceship != null, "enemySpaceship cannot be null");
            Debug.Assert(bossEnemyShip != null, "bossEnemyShip cannot be null");
            Debug.Assert(playerSpaceshipHp > 0, "playerSpaceship hp has to be more than zero");
            Debug.Assert(playerSpaceshipMoveSpeed > 0, "playerSpaceshipMoveSpeed has to be more than zero");
            Debug.Assert(enemySpaceshipHp > 0, "enemySpaceshipHp has to be more than zero");
            Debug.Assert(enemySpaceshipMoveSpeed > 0, "enemySpaceshipMoveSpeed has to be more than zero");
            Debug.Assert(bossEnemyShipHp > 0, "bossEnemyShipHp hp has to be more than zero");

            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void StartGame()
        {
            SpawnPlayerSpaceship();
            SpawnEnemySpaceship();
        }

        private void SpawnPlayerSpaceship()
        {
            spawnedPlayerShip = Instantiate(playerSpaceship);
            spawnedPlayerShip.Init(playerSpaceshipHp, playerSpaceshipMoveSpeed);
            spawnedPlayerShip.OnExploded += OnPlayerSpaceshipExploded;
        }

        private void OnPlayerSpaceshipExploded()
        {
            Restart();
        }

        private void SpawnEnemySpaceship()
        {
            var spawnedEnemyShip = Instantiate(enemySpaceship);
            spawnedEnemyShip.Init(enemySpaceshipHp, enemySpaceshipMoveSpeed);
            spawnedEnemyShip.OnExploded += OnEnemySpaceshipExploded;
        }
        
        private void SpawnBossEnemySpaceship()
        {
            var spawnedBossEnemyShip = Instantiate(bossEnemyShip);
            spawnedBossEnemyShip.Init(bossEnemyShipHp);
            spawnedBossEnemyShip.OnExploded += OnBossEnemySpaceshipExploded;
        }
        
        private void OnEnemySpaceshipExploded()
        {
            ScoreManager.Instance.UpdateScore(1);
            SpawnBossEnemySpaceship();
            OnSpawnBoss?.Invoke();
        }
        
        private void OnBossEnemySpaceshipExploded()
        {
            ScoreManager.Instance.UpdateScore(5);
            OnBossEnemyExploded?.Invoke();
            Restart();
        }

        private void Restart()
        {
            OnRestarted?.Invoke();
            DestroyRemainingShips();
        }

        private void DestroyRemainingShips()
        {
            var remainingEnemy = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (var enemy in remainingEnemy)
            {
                Destroy(enemy);
            }

            var remainingPlayer = GameObject.FindGameObjectsWithTag("Player");
            foreach (var player in remainingPlayer)
            {
                Destroy(player);
            }
        }
    }
}