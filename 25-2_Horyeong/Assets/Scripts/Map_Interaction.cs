using UnityEngine;

public class Map_Interaction : MonoBehaviour
{
    public InteractionData interactionData;
    public GameObject interactionPrompt;    //상호작용 가능 오브젝트임을 표시

    public string dialogueCSVFileName;

    public void SetIsIntrecting(bool isInteracting) { this.interactionData.isInteracted = isInteracting; }

    private void Start()
    {
        interactionData.isInteracted = false;
        interactionData.isTriggered = false;
        interactionData.interactedCooldown = 0f;
        interactionPrompt.transform.position = new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z);
        interactionPrompt.SetActive(false);

    }

    private void Update()
    {
        if(interactionData.interactedCooldown > 0f)
            interactionData.interactedCooldown -= Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            interactionData.isTriggered = true;
            interactionPrompt.SetActive(true);

            collision.GetComponent<PlayerController>().SetCurrentInteractable(this);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            interactionData.isTriggered = false;
            interactionPrompt.SetActive(false);

            collision.GetComponent<PlayerController>().SetCurrentInteractable(null);
        }
    }

    private void DialogueRead()
    {
        if (interactionData.isTriggered)
        {
            if (!interactionData.isInteracted)
            {
                DialogueManager.Instance.LoadAndStartDialogue(dialogueCSVFileName, this.name);
            }

            else
            {
                DialogueManager.Instance.CheckDialogueType();
            }
        }
    }
    private void SavePoint(PlayerController player)
    {
        // Save Point 상호작용 로직 추가
        Debug.Log("Save Point에 상호작용했습니다.");

        DialogueManager.Instance.LoadAndStartDialogue(dialogueCSVFileName, this.name);

        PlayerStatus playerStatus = FindAnyObjectByType<PlayerStatus>();
        playerStatus.Heal(playerStatus.GetmaxHp());

        DataManager.Instance.UpdateAndSavePlayerPosition(player);
    }

    public void Interact(PlayerController player)
    {
        if (interactionData.interactedCooldown > 0f) return;
        interactionData.interactedCooldown = 0.25f;

        if (interactionData.interactionType == InteractionType.Dialogue) DialogueRead();

        if (interactionData.interactionType == InteractionType.SavePoint) SavePoint(player);

    }
}
