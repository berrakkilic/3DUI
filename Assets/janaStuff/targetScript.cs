using UnityEngine;
using UnityEngine.UI;

public class targetScript : MonoBehaviour
{
    [Header("Health")]
    public float maxHealth = 100f;
    public float health;

    [Header("UI")]
    public Slider healthBar;

    [Header("Drop")]
    public GameObject fluteDrop;

    private void Start()
    {
        health = maxHealth;

        if (healthBar != null)
        {
            healthBar.maxValue = maxHealth;
            healthBar.value = health;
            healthBar.gameObject.SetActive(false);
        }

        if (fluteDrop != null)
        {
            fluteDrop.SetActive(false);
        }
    }

    public void takeDamage(float damageAmount)
    {
        health -= damageAmount;
        health = Mathf.Clamp(health, 0, maxHealth);

        if (healthBar != null)
        {
            healthBar.gameObject.SetActive(true);
            healthBar.value = health;
        }

        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        if (healthBar != null)
        {
            healthBar.gameObject.SetActive(false);
        }

        if (fluteDrop != null)
        {
            fluteDrop.transform.position = transform.position + Vector3.up * 0.7f;
            fluteDrop.SetActive(true);
        }

        Destroy(gameObject);
    }
}
