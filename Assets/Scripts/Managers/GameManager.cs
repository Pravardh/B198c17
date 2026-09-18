using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI timeText;

    [SerializeField]
    private GameObject winScreen;

    [SerializeField]
    private PlayerScoreSystem playerScoreSystem;

    [SerializeField]
    private GameObject loseScreen;

    [SerializeField]
    private GameObject parentUI;
    private float elapsedTime;

    private static GameManager instance;
    public static GameManager Instance {get {return instance; }}

    [SerializeField]
    private float totalGameTime = 180.0f;

    public event Action OnGameEnd;
    private bool isRunning = false;

    [SerializeField]
    private TextMeshProUGUI highScore;

    void Awake()
    {
        if(instance == null)
            instance = this;
    }
    void Start()
    {
        elapsedTime = 0.0f;
        isRunning = true;
    }

    void Update()
    {
        timeText.text = Mathf.RoundToInt(elapsedTime) + "/" + totalGameTime;
        if(!isRunning) return;
        elapsedTime += Time.deltaTime;

        if(elapsedTime >= totalGameTime)
        {
            EndGame(EndEvent.SUCCESS);
        }
    }

    public void EndGame(EndEvent endEvent)
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        elapsedTime = totalGameTime;
        isRunning = false;
        ShowEndUI(endEvent);
        OnGameEnd?.Invoke();
        Debug.Log("Game ended");
    }

    private void ShowEndUI(EndEvent endEvent)
    {
        parentUI.gameObject.SetActive(true);
        highScore.text = "Highscore: " + playerScoreSystem.GetHighScore();

        switch (endEvent)
        {
            case EndEvent.SUCCESS:
                winScreen.gameObject.SetActive(true);
                break;
            case EndEvent.FAIL:
                loseScreen.gameObject.SetActive(true);
                break;
        }
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}

public enum EndEvent
{
    SUCCESS,
    FAIL
}