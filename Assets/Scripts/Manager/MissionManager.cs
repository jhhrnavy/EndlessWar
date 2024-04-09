using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MissionManager : MonoBehaviour
{
    public static MissionManager Instance;

    private void Awake()
    {
        if(null == Instance)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public int level;
    public string clearTime;

    private void Start()
    {
        level = -1;
    }
    public void SelectMission(int level)
    {
        this.level = level;
    }

    public void LoadLevel()
    {
        if(level <= 1)
        {
            Debug.Log("Select Mission");
            return;
        }
        SceneManager.LoadScene(level, LoadSceneMode.Single);
        SceneManager.LoadSceneAsync(1,LoadSceneMode.Additive); // UI
    }
}
