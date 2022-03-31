using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Manager
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private Button startButton;
        [SerializeField] private RectTransform startDialog;
        [SerializeField] private TextMeshProUGUI scoreText;
        [SerializeField] private TextMeshProUGUI finalScoreText;
        [SerializeField] private Button restartButton;
        [SerializeField] private RectTransform endDialog;
        [SerializeField] private GameObject background;
        [SerializeField] private GameObject backgroundNextStage;
        [SerializeField] private RectTransform winnerUI;
        [SerializeField] private TextMeshProUGUI stageUI;

        private void Awake()
        {
            Debug.Assert(startButton != null, "startButton cannot be null");
            Debug.Assert(startDialog != null, "startDialog cannot be null");
            Debug.Assert(scoreText != null, "scoreText cannot null");
            Debug.Assert(finalScoreText != null, "finalScoreText cannot null");
            Debug.Assert(restartButton != null, "restartButton cannot be null");
            Debug.Assert(endDialog != null, "endDialog cannot be null");
            Debug.Assert(background != null, "background cannot be null");
            Debug.Assert(backgroundNextStage != null, "backgroundNextStage cannot be null");
            Debug.Assert(winnerUI != null, "winnerUI cannot be null");
            Debug.Assert(stageUI != null, "stageUI cannot be null");

            startButton.onClick.AddListener(OnStartButtonClicked);
            restartButton.onClick.AddListener(OnRestartButtonClicked);
        }

        private void Start()
        {
            background.gameObject.SetActive(true);
            ScoreManager.Instance.ResetScore();
            GameManager.Instance.OnRestarted += RestartUI;
            ScoreManager.Instance.OnScoreUpdated += UpdateScoreUI;
            GameManager.Instance.OnSpawnBoss += NextStage;
            GameManager.Instance.OnBossEnemyExploded += Winner;
            ShowEndDialog(false);
            ShowScore(false);
            UpdateScoreUI();
            SetTextStage(1);
            ShowStage(false);
            ShowWinnerUI(false);
        }

        private void Winner()
        {
            ShowStage(false);
            ShowWinnerUI(true);
        }

        private void OnStartButtonClicked()
        {
            ShowStartDialog(false);
            ShowScore(true);
            ShowStage(true);
            GameManager.Instance.StartGame();
        }

        private void OnRestartButtonClicked()
        {
            SetTextStage(1);
            ShowWinnerUI(false);
            ShowEndDialog(false);
            UpdateScoreUI();
            ShowScore(true);
            ShowStage(true);
            GameManager.Instance.StartGame();
            backgroundNextStage.gameObject.SetActive(false);
            background.gameObject.SetActive(true);
        }

        private void UpdateScoreUI()
        {
            scoreText.text = $"Score : {ScoreManager.Instance.GetScore()}";
            finalScoreText.text = $"Player Score : {ScoreManager.Instance.GetScore()}";
        }

        private void RestartUI()
        {
            ShowStage(false);
            ShowScore(false);
            ShowEndDialog(true);
        }

        private void NextStage()
        {
            SetTextStage(2);
            background.gameObject.SetActive(false);
            backgroundNextStage.gameObject.SetActive(true);
        }

        private void ShowScore(bool showScore)
        {
            scoreText.gameObject.SetActive(showScore);
        }

        private void ShowStartDialog(bool showDialog)
        {
            startDialog.gameObject.SetActive(showDialog);
        }

        private void ShowEndDialog(bool showDialog)
        {
            endDialog.gameObject.SetActive(showDialog);
        }

        private void ShowStage(bool showStage)
        {
            stageUI.gameObject.SetActive(showStage);
        }

        private void SetTextStage(int stage)
        {
            stageUI.text = $"Stage:{stage}";
        }

        private void ShowWinnerUI(bool show)
        {
            winnerUI.gameObject.SetActive(show);
        }

        private void OnDestroy()
        {
            GameManager.Instance.OnRestarted -= RestartUI;
            ScoreManager.Instance.OnScoreUpdated -= UpdateScoreUI;
        }
    }
}