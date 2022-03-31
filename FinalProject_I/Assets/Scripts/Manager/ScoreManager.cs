using System;
using UnityEngine;

namespace Manager
{
    public class ScoreManager : MonoBehaviour
    {
        public event Action OnScoreUpdated;

        private int playerScore;

        public static ScoreManager Instance { get; private set; }
    
        private void Awake()
        {
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
        
        private void Start()
        {
            playerScore = 0;
            GameManager.Instance.OnRestarted += OnRestarted;
        }

        public int GetScore()
        {
            return playerScore;
        }
        
        public void UpdateScore(int score)
        {
            playerScore += score;
            OnScoreUpdated?.Invoke();
        }
        
        public void ResetScore()
        {
            playerScore = 0;
        }
        
        private void OnRestarted()
        {
            ResetScore();
        }
    }
}


