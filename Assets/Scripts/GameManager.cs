using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("UI Panels")]
    public GameObject startPanel;
    public GameObject pausePanel;
    public GameObject gameOverPanel;

    [Header("UI Text")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI hpText;
    public TextMeshProUGUI narrativeText;

    [Header("Station Settings")]
    public int stationMaxHP = 5;
    private int _stationHP;
    private int _score;
    
    private bool _gameStarted = false;
    private bool _isPaused = false;
    private bool _gameOver = false;

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;

        _stationHP = stationMaxHP;
        ShowNarrative("");

        Time.timeScale = 0f;
        startPanel.SetActive(true);
        pausePanel.SetActive(false);
        gameOverPanel.SetActive(false);

        UpdateUI();
    }

    void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            QuitGame();
        }

        if (_gameStarted && !_gameOver)
        {
            if (Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                if (_isPaused) ResumeGame();
                else PauseGame();
            }
        }
    }


    public void StartGame()
    {
        _gameStarted = true;
        startPanel.SetActive(false);
        Time.timeScale = 1f;
        
        ShowNarrative("Метеоритная атака началась! Защити корабль!");
    }

    public void PauseGame()
    {
        _isPaused = true;
        Time.timeScale = 0f;
        pausePanel.SetActive(true);
    }

    public void ResumeGame()
    {
        _isPaused = false;
        Time.timeScale = 1f;
        pausePanel.SetActive(false);
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitGame()
    {
        Debug.Log("Выход...");
        Application.Quit();
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }

    void GameOver()
    {
        _gameOver = true;
        Time.timeScale = 0f;
        gameOverPanel.SetActive(true);
    }

    public void AddScore(int val)
    {
        _score += val;
        UpdateUI();
        if (_score == 50)  ShowNarrative("Отличная работа! Продолжай!");
    }

    public void DamageStation(int dmg)
    {
        if (_gameOver) return;
        _stationHP -= dmg;
        UpdateUI();
        CameraShake.Instance?.Shake(3f, 0.3f);
        if (_stationHP <= 0) GameOver();
    }

    void UpdateUI()
    {
        if (hpText) hpText.text = "HP: " + _stationHP + " / " + stationMaxHP;
        if (scoreText) scoreText.text = "Счёт: " + _score;
    }

    void LockCursor(bool locked)
    {
        Cursor.lockState = locked ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !locked;
    }

    public void ShowNarrative(string msg)
    {
        StopAllCoroutines();
        StartCoroutine(NarrativeRoutine(msg));
    }

    System.Collections.IEnumerator NarrativeRoutine(string msg)
    {
        narrativeText.gameObject.SetActive(true);
        narrativeText.text = msg;
        yield return new WaitForSeconds(3.5f);
        narrativeText.gameObject.SetActive(false);
    }
}