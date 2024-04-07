using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public string thisScene;
    public UIManager uiManager;

    [SerializeField]
    private Stopwatch _stopwatch;

    private void Awake()
    {
        thisScene = SceneManager.GetActiveScene().name;
    }

    private void OnEnable()
    {
        PlayerController.OnPlayerDeath += PlayerDied;
    }

    private void OnDisable()
    {
        PlayerController.OnPlayerDeath -= PlayerDied;
    }

    private void Start()
    {
        _stopwatch.StartStopwatch();
    }

    public void PauseOrContinue()
    {
        Time.timeScale = (Time.timeScale == 1) ? 0f : 1f;
        uiManager.ActivePausePanel();
    }

    public void ReStart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(thisScene);

        // Time reset
        //_stopwatch.StopStopwatch();
        //_stopwatch.ResetStopwatch();
    }

    public void PlayerDied()
    {
        PauseOrContinue();
    }

}
