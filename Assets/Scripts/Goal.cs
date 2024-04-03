using UnityEngine;

public class Goal : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Congratulations! You win!");
            GameManager.Instance.ReStart();
        }
    }
}
