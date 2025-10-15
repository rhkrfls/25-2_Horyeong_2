using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    public int maxHp = 100;
    public int currentHp;

    private void Awake()
    {
        currentHp = maxHp;
    }

    public void TakeDamage(int damage)
    {
        currentHp -= damage;

        if (currentHp <= 0)
        {
            currentHp = 0;
            Die();
        }
    }

    public void Heal(int amount)
    {
        currentHp += amount;

        if (currentHp > maxHp)
        {
            currentHp = maxHp;
        }
    }

    public void Die()
    {
        Debug.Log("Player Died");
        // Implement death logic here (e.g., respawn, game over screen)
    }
}
