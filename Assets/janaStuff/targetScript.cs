using UnityEngine;

public class targetScript : MonoBehaviour
{
    public float health = 50f;
    // also from brackeys tutorial
    public void takeDamage(float damageAmount)
    {
        health -= damageAmount;
        if(health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
