using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[DefaultExecutionOrder(-1)]
public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

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
    [SerializeField] private TextMeshProUGUI scoreText;

    [Header("Audio Components")]
    [SerializeField] private AudioSource backgroundMusic;

    private bool isPaused;
    private bool isActive;
    [HideInInspector] public bool isDead;

    public float score = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if (playButton)
        {
            playButton.onClick.AddListener(StartGame);
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
    }

    public void StartGame()
    {
        score = 0;
        if(scoreText != null)
        {
            scoreText.text = "Score: " + score.ToString();
        }
        
        SceneManager.LoadScene(1);
    }

    private void GameSettings()
    {
        AudioManager.Instance.Play("Select");
        settingsPanel.SetActive(true);
        isActive = true;
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
        gameOverPanel.SetActive(true);
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
            settingsPanel.SetActive(false);
            isActive = false;
        }
    }

    public void PauseGame()
    {
        isPaused = !isPaused;
        pausePanel.SetActive(isPaused);
        //scorePanel.SetActive(false);

        if (isPaused)
        {
            Time.timeScale = 0;
            PauseBackgorundMusic();
        }

        else
        {
            AudioManager.Instance.Play("Select");
            Time.timeScale = 1;
            //scorePanel.SetActive(true);
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
        score += 5;
        scoreText.text = "Score: " +  score.ToString();
    }
}
