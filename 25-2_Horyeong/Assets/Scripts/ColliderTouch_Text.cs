using UnityEngine;

public class ColliderTouch_Text : MonoBehaviour

{
    public string dialogueCSVFileName;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            DialogueManager.Instance.LoadAndStartDialogue(dialogueCSVFileName, this.name);
        }
    }
}
