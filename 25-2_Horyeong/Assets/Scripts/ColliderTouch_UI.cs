using UnityEngine;

public class ColliderTouch_UI : MonoBehaviour
{
    public GameObject UI_Image;

    private void Start()
    {
        UI_Image.gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ( collision.CompareTag("Player"))
        {
            UI_Image.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            UI_Image.gameObject.SetActive(false);
        }
    }
}
