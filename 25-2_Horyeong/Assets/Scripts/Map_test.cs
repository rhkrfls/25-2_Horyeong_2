using UnityEngine;

public class Map_test : MonoBehaviour
{
    public PlayerStatus PlayerStatus;

    private void Start()
    {
        PlayerStatus = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStatus>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerStatus.TakeDamage(5);
        }
    }
}
