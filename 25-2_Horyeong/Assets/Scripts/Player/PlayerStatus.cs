using System;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    static public PlayerStatus instance;

    PlayerController playerController;

    public event Action<float, float> OnHealthChanged;
    public int maxHp = 100;
    public int GetmaxHp() { return maxHp; }
    public int currentHp;
    public int GetCurrentHp() {  return currentHp; }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);  // DontDestroyOnLoad(); 로 파괴 안되도록 막음
        }
        else
            Destroy(this.gameObject);

        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        currentHp = maxHp;
    }

    public void TakeDamage(int damage, Transform position)
    {
        if (playerController.isKnockedBack) return;
        playerController.ApplyKnockback(position);
        playerController.animator.SetTrigger("isHit");

        currentHp -= damage;
        Debug.Log($"현재 체력 : {currentHp}");

        OnHealthChanged?.Invoke(currentHp, maxHp);

        if (currentHp <= 0)
        {
            currentHp = 0;
            Die();
        }
    }

    public void Heal(int amount)
    {
        if (currentHp + amount > maxHp)
        {
            currentHp = maxHp;
        }

        else
        {
            currentHp += amount;
        }

        OnHealthChanged?.Invoke(currentHp, maxHp);
    }

    public void Die()
    {
        playerController.animator.SetTrigger("isDeath");
        Debug.Log("Player Died");
    }
}
