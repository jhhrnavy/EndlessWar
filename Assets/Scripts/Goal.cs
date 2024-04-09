using JetBrains.Annotations;
using UnityEngine;

public class Goal : MonoBehaviour
{
    public int rewardAmount = 10;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Congratulations! You win!");
            UIManager.Instance.Stopwatch.StopStopwatch();
            GameManager.Instance.PauseOrContinue();
            CoinManager.Instance.AddCoins(rewardAmount);
            MissionManager.Instance.clearTime = UIManager.Instance.Stopwatch.TimerText;
            UIManager.Instance.ShowMissionCompletePopup(rewardAmount);
        }
    }
}
