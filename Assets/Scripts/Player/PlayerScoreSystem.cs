using TMPro;
using UnityEngine;

public class PlayerScoreSystem : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI scoreText;

    [SerializeField]
    private TextMeshProUGUI highScoreText;

    private PlayerSaveSystem playerSaveSystem;
    

    private int score;
    private int highScore;

    private void Start()
    {

        playerSaveSystem = GetComponent<PlayerSaveSystem>();
        //playerSaveSystem.SaveKillCount(0);
        
        highScore = playerSaveSystem.LoadKillCount();

        UpdateUI();

        GameManager.Instance.OnGameEnd += OnGameEnd;
    }

    private void OnDestroy()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.OnGameEnd -= OnGameEnd;
    }

    public void AddScore(int amount)
    {
        score += amount;

        if (score > highScore)
            highScore = score;

        UpdateUI();
    }

    public int GetScore()
    {
        return score;
    }

    public int GetHighScore()
    {
        return highScore;
    }

    private void OnGameEnd()
    {
        playerSaveSystem.SaveKillCount(highScore);
    }

    private void UpdateUI()
    {
        if (scoreText != null)
            scoreText.text = "Kills: " + score;

        if (highScoreText != null)
            highScoreText.text = "Highscore: " + highScore;
    }
}