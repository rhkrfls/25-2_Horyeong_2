using UnityEngine;
using UnityEngine.InputSystem;

enum PLAYERSTATE
{
    IDLE, WALK, RUN, 
}

public class PlayerController : MonoBehaviour
{
    private PLAYERSTATE playerstate;
    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private Vector2 moveInput;

    [Header("�̵�")]
    public float moveSpeed = 5f;
    public bool isMoving = false;

    [Header("����")]
    public float jumpForce = 8f;        // ���� �� ���� ���� ũ��
    public LayerMask groundLayer;       // �ٴ����� �ν��� ���̾�
    public bool isGrounded = true;      // ���� �ٴڿ� ����ִ��� ����
    public BoxCollider2D coll;          // �ٴ� üũ�� ���� Collider2D �߰�


    public Gamemanager gameManager;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        coll = GetComponent<BoxCollider2D>();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (isMoving == false && context.started)
            animator.SetTrigger("isWalkStart");

        moveInput = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (isGrounded)
            {
                rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                Debug.Log("����!");
            }
        }
    }

    private void FlipCharacter(float horizontalVelocity)
    {
        if (horizontalVelocity > 0.01f) 
        {
            spriteRenderer.flipX = false;
        }
        else if (horizontalVelocity < -0.01f)
        {
            spriteRenderer.flipX = true; 
        }
    }

    private void CheckGround()
    {
        isGrounded = Physics2D.BoxCast(
            coll.bounds.center, // �ڽ�ĳ��Ʈ ������
            coll.bounds.size,   // �ڽ�ĳ��Ʈ ũ�� (ĳ���� �ݶ��̴��� ����)
            0f,                 // ȸ�� ����
            Vector2.down,       // �Ʒ� �������� ĳ����
            0.1f,               // ĳ���� �Ÿ� (���� ª��)
            groundLayer         // �ٴ� ���̾� ����ũ
        );
    }

    void FixedUpdate()
    {
        if (!gameManager.isGroggy)
        {
            // �̵�
            if (moveInput.x != 0)
            {
                isMoving = true;
                rb.linearVelocity = new Vector2(moveInput.x * moveSpeed, rb.linearVelocity.y);
                float horizontalVelocity = Mathf.Abs(rb.linearVelocity.x);
                animator.SetFloat("isWalk", horizontalVelocity);
            }

            else
            {
                isMoving = false;
                rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
                animator.SetFloat("isWalk", 0);
                animator.SetTrigger("isWalkEnd");
            }
        }

        // �¿����
        FlipCharacter(rb.linearVelocityX);
        
        // �ٴ� üũ
        CheckGround();
    }


}
