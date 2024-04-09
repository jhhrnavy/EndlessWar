using UnityEngine;
using UnityEngine.SceneManagement;
using static Cinemachine.DocumentationSortingAttribute;

public class GameManager : Singleton<GameManager>
{
    public string thisScene;
    private string _mainMenu = "Main Menu";

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
        Time.timeScale = 1f;
        UIManager.Instance.Stopwatch.StartStopwatch();
    }

    public void PauseOrContinue()
    {
        Time.timeScale = (Time.timeScale == 1) ? 0f : 1f;
    }

    public void ReStart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(thisScene, LoadSceneMode.Single);
        SceneManager.LoadSceneAsync(1, LoadSceneMode.Additive); // UI

    }
    public void LoadMainMenu()
    {
        SceneManager.LoadScene(_mainMenu);
    }

    // Mission Failed
    public void PlayerDied()
    {
        PauseOrContinue();
        UIManager.Instance?.ShowMissionFailedPopup();
    }
}
