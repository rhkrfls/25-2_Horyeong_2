using UnityEngine;
using UnityEngine.InputSystem;

public class ImageChange : MonoBehaviour
{
    public int currentDialogueIndex = 0;
    public int currentImageIndex = 0;
    public int changeCount = 0;
    public bool isImageChanged = false;
    public Sprite[] images;
    public SpriteRenderer spriteRenderer;
    public string dialogueCSVFileName;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        DialogueManager.Instance.LoadAndStartDialogue(dialogueCSVFileName, this.name);
    }

    private void Update()
    {
        if (isImageChanged)
        {
            if (DialogueManager.Instance.currentLineIndex != currentDialogueIndex)
            {
                isImageChanged = false;
            }
        }

        if (DialogueManager.Instance.currentDialogue.lines[DialogueManager.Instance.currentLineIndex].type == "end")
        {
            SceneController.instance.LoadScene("CharacterScene");
        }
    }

    public void OnNextText(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            DialogueManager.Instance.CheckDialogueType();

            if (DialogueManager.Instance.currentDialogue.lines[DialogueManager.Instance.currentLineIndex].isChangeBG)
            {
                currentDialogueIndex = DialogueManager.Instance.currentLineIndex;

                if (isImageChanged == false && currentDialogueIndex == DialogueManager.Instance.currentLineIndex)
                {
                    isImageChanged = true;
                    ChangeImage(changeCount++);
                }
            }
        }
    }
    public void ChangeImage(int imageCount)
    {
        if (images.Length == 0) return;

        currentImageIndex = imageCount;
        if (currentImageIndex >= images.Length)
        {
            currentImageIndex = 0;
        }

        spriteRenderer.sprite = images[currentImageIndex];
    }
}
