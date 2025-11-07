using UnityEngine;
using UnityEngine.InputSystem;

public class ImageChange : MonoBehaviour
{
    public int currentImageIndex = 0;
    public Sprite[] images;
    public SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {

    }

    public void OnImageChange(InputAction.CallbackContext context)
    {
        currentImageIndex++;
        if (currentImageIndex >= images.Length)
        {
            currentImageIndex = 0;
        }
        ChangeImage();
    }
    public void ChangeImage()
    {
        if (images.Length == 0) return;

        //currentImageIndex = (currentImageIndex + 1) % images.Length;
        spriteRenderer.sprite = images[currentImageIndex];
    }
}
