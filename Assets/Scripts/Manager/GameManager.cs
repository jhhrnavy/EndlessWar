using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public string thisScene;
    public UIManager uiManager;

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

    public void PauseOrContinue()
    {
        Time.timeScale = (Time.timeScale == 1) ? 0f : 1f;
        uiManager.ActivePausePanel();
    }

    public void ReStart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(thisScene);
    }

    public void PlayerDied()
    {
        PauseOrContinue();
    }

}
