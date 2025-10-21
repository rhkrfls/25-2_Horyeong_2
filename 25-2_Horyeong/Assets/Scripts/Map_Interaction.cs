using UnityEngine;

public class Map_Interaction : MonoBehaviour
{
    public DialogueManager dm;
    public string dialogueCSVFileName;

    public bool isTriggered = false;
    public bool isInteracting = false;
    public void SetIsIntrecting(bool isInteracting) { this.isInteracting = isInteracting; }
    public GameObject interactionPrompt;

    private void Start()
    {
        dm = FindObjectOfType<DialogueManager>();
        interactionPrompt.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isTriggered = true;
            interactionPrompt.SetActive(true);

            collision.GetComponent<PlayerController>().SetCurrentInteractable(this);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isTriggered = false;
            interactionPrompt.SetActive(false);

            collision.GetComponent<PlayerController>().SetCurrentInteractable(null);
        }
    }

    public void Interact()
    {
        // �÷��̾ ���� �ȿ� ���� ���� ��ȭ ����
        if (isTriggered)
        {
            if (!isInteracting)
            {
                // DialogueManager�� LoadAndStartDialogue �Լ� ȣ��
                if (dm != null)
                {
                    dm.LoadAndStartDialogue(dialogueCSVFileName);
                }
            }

            else
            {
                if (dm != null)
                {
                    dm.DisplayNextLine();
                }
            }
        }
    }
}
