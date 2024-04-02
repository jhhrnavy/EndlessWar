using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    public float health, maxHealth;

    [SerializeField]
    private Slider _healthBar;

    public void SetMaxHealth(float maxHealth)
    {
        this.maxHealth = maxHealth;
    }

    public void SetHealth(float health)
    {
        this.health = health;
        _healthBar.value = this.health / maxHealth;
    }
}
