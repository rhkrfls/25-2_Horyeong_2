using UnityEngine;

public class Map_Interaction : MonoBehaviour
{
    public InteractionData interactionData;

    public DialogueManager dm;
    public string dialogueCSVFileName;

    public void SetIsIntrecting(bool isInteracting) { this.interactionData.isInteracted = isInteracting; }

    private void Start()
    {
        dm = FindAnyObjectByType<DialogueManager>();
        interactionData.interactionPrompt.transform.position = new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z);
        interactionData.interactionPrompt.SetActive(false);

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            interactionData.isTriggered = true;
            interactionData.interactionPrompt.SetActive(true);

            collision.GetComponent<PlayerController>().SetCurrentInteractable(this);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            interactionData.isTriggered = false;
            interactionData.interactionPrompt.SetActive(false);

            collision.GetComponent<PlayerController>().SetCurrentInteractable(null);
        }
    }

    private void DialogueRead()
    {
        // 플레이어가 범위 안에 있을 때만 대화 시작
        if (interactionData.isTriggered)
        {
            if (!interactionData.isInteracted)
            {
                // DialogueManager의 LoadAndStartDialogue 함수 호출
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

    private void SavePoint(PlayerController player)
    {
        // Save Point 상호작용 로직 추가
        Debug.Log("Save Point에 상호작용했습니다.");

        PlayerStatus playerStatus = FindAnyObjectByType<PlayerStatus>();
        playerStatus.Heal(playerStatus.GetmaxHp());

        DataManager.Instance.UpdateAndSavePlayerPosition(player);
    }

    public void Interact(PlayerController player)
    {
        if (interactionData.interactionType == InteractionType.Dialogue) DialogueRead();

        if (interactionData.interactionType == InteractionType.SavePoint) SavePoint(player);

    }
}
