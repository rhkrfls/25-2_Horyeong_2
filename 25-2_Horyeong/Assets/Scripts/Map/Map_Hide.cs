using UnityEngine;

public class Map_Hide : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            gameObject.SetActive(false);
    }
}
