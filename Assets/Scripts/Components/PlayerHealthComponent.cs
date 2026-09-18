using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthComponent : MonoBehaviour
{
    private float currentHealth;
    private const float MAX_HEALTH = 100.0f;

    [SerializeField]
    private Slider healthSlider;

    void Awake()
    {
        currentHealth = MAX_HEALTH;
    }
    public void TakeDamage()
    {
        currentHealth -= 7.5f;
        Debug.Log(currentHealth);
        if(currentHealth <= 0.0f)
        {
            GameManager.Instance.EndGame(EndEvent.FAIL);
            Destroy(gameObject);
        }

        healthSlider.value = currentHealth/MAX_HEALTH;
    }
}
