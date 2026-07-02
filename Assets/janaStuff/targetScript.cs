using UnityEngine;
using UnityEngine.UI;

public class targetScript : MonoBehaviour
{
    [Header("Health")]
    public float maxHealth = 100f;
    public float health;

    [Header("UI")]
    public GameObject heartOne;
    public GameObject heartTwo;
    public GameObject heartThree;
    public GameObject heartFour;
    public GameObject heartFive;

    [Header("Drop")]
    public GameObject fluteDrop;
    public shootingScript shooting;

    private void Start()
    {
        health = maxHealth;

        heartOne.SetActive(true);
        heartTwo.SetActive(true);
        heartThree.SetActive(true);
        heartFour.SetActive(true);
        heartFive.SetActive(true);
            

        if (fluteDrop != null)
        {
            fluteDrop.SetActive(false);
        }
        shooting.monsterNotDead = true;
    }

    public void takeDamage(float damageAmount)
    {
        health -= damageAmount;
        health = Mathf.Clamp(health, 0, maxHealth);

        if (health < 100)
        {
            heartFive.SetActive(false);
        }
        if (health < 80)
        {
            heartFour.SetActive(false);
        }
        if (health < 60)
        {
            heartThree.SetActive(false);
        }
        if (health < 40)
        {
            heartTwo.SetActive(false);
        }

        if (health <= 0)
        {
            heartOne.SetActive(false);
            Die();
        }
    }

    private void Die()
    {
        if (fluteDrop != null)
        {
            fluteDrop.transform.position = transform.position + Vector3.up * 0.7f;
            fluteDrop.SetActive(true);
        }
        shooting.monsterNotDead = false;
        Destroy(gameObject);
    }
}
