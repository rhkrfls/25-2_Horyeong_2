using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    PlayerController playerController;
    public int maxHp = 100;
    public int currentHp;

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
        currentHp = maxHp;
    }

    public void TakeDamage(int damage)
    {
        if (playerController.isKnockedBack) return;
        playerController.ApplyKnockback(playerController.transform);

        playerController.animator.SetTrigger("isHit");

        currentHp -= damage;
        Debug.Log($"현재 체력 : {currentHp}");

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
        playerController.animator.SetTrigger("isDeath");

        Debug.Log("Player Died");
        // Implement death logic here (e.g., respawn, game over screen)
    }
}
