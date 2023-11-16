using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[DefaultExecutionOrder(-1)]
public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; set; }

    [Header("Buttons")]
    [SerializeField] private Button playButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button creditsButton;
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button returnToMenuButton;
    [SerializeField] private Button quitButton;
    [SerializeField] private Button backButton;

    [Header("Panel Components")]
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] public GameObject gameOverPanel;
    [SerializeField] private GameObject creditsPanel;
    [SerializeField] public GameObject scorePanel;

    [Header("Audio Components")]
    [SerializeField] private AudioSource backgroundMusic;

    private bool isPaused;
    private bool isActive;
    [HideInInspector] public bool isDead;

    // Start is called before the first frame update
    void Start()
    {
        if (playButton)
        {
            playButton.onClick.AddListener(() => Invoke("StartGame", 1f));
            playButton.onClick.AddListener(() => AudioManager.Instance.Play("Select"));
        }

        if (creditsButton)
            creditsButton.onClick.AddListener(Credits);

        if (resumeButton)
            resumeButton.onClick.AddListener(PauseGame);

        if (returnToMenuButton)
            returnToMenuButton.onClick.AddListener(GameQuit);

        if (settingsButton)
            settingsButton.onClick.AddListener(GameSettings);

        if (quitButton)
            quitButton.onClick.AddListener(GameQuit);

        if (backButton)
            backButton.onClick.AddListener(BackToPauseMenu);

        if (!backgroundMusic && SceneManager.GetActiveScene().buildIndex == 1)
            Debug.Log("Please set Background music file 1");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            AudioManager.Instance.Play("Select");
            PauseGame();
        }

        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            UpdateScoreDisplay();
        }
    }

    public void StartGame()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void LoadGame()
    {
        SceneManager.LoadScene("Level");
    }

    private void GameSettings()
    {
        AudioManager.Instance.Play("Select");
        settingsPanel.SetActive(true);
    }

    private void BackToPauseMenu()
    {
        AudioManager.Instance.Play("Select");
        settingsPanel.SetActive(false);
    }

    public void BackToMainMenu()
    {
        AudioManager.Instance.Play("Select");
        SceneManager.LoadScene(0);
    }
    public void GameQuit()
    {
        SceneManager.LoadScene(0);
        //UnityEditor.EditorApplication.isPlaying = false;
    }

    public void GameOver()
    {
        SceneManager.LoadScene("GameOver");
    }

    public void Credits()
    {
        AudioManager.Instance.Play("Select");
        creditsPanel.SetActive(true);
        isActive = true;
    }


    public void GoBack()
    {
        if (isActive)
        {
            AudioManager.Instance.Play("Select");
            creditsPanel.SetActive(false);
            isActive = false;
        }
    }

    public void PauseGame()
    {
        isPaused = !isPaused;
        pausePanel.SetActive(isPaused);
        scorePanel.SetActive(false);

        if (isPaused)
        {
            Time.timeScale = 0;
            PauseBackgorundMusic();
        }

        else
        {
            AudioManager.Instance.Play("Select");
            Time.timeScale = 1;
            scorePanel.SetActive(true);
            UnpauseBackgorundMusic();
        }
    }

    public void PauseBackgorundMusic()
    {
        backgroundMusic.Pause();
    }

    public void UnpauseBackgorundMusic()
    {
        backgroundMusic.UnPause();
    }

    public void SetInactive()
    {
        GoBack();
    }

    public void UpdateScoreDisplay()
    {
    
    }
}
