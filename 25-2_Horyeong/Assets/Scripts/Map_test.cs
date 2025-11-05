using UnityEngine;

public class Map_test : MonoBehaviour
{
    public PlayerStatus PlayerStatus;

    private void Start()
    {
        PlayerStatus = FindObjectOfType<PlayerStatus>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerStatus.TakeDamage(5);
        }
    }
}
