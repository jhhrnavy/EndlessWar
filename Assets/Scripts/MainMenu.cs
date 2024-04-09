using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public CoinDisplay coinDisplay;
    public TextMeshProUGUI clearTimeText;
    public void OnClickBtnPlayGame()
    {
        MissionManager.Instance.LoadLevel();
    }
    public void OnClickBtnSelectMission(int level)
    {
        MissionManager.Instance.SelectMission(level);
        SetClearTimeText(MissionManager.Instance.clearTime);
    }

    public void SetClearTimeText(string time)
    {
        clearTimeText.text = $"ClearTime : {time}";
    }
}
