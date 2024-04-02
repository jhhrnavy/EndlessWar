using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _pausePanel;

    public void ActivePausePanel()
    {
        _pausePanel.SetActive(!_pausePanel.activeInHierarchy);
    }
}
