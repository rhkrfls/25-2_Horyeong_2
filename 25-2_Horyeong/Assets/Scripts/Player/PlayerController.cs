using UnityEngine;
using UnityEngine.InputSystem;

public enum PLAYERSTATE
{
    IDLE, WALK, RUN, 
}

public class PlayerController : Player
{
    public PLAYERSTATE playerstate;
    public Rigidbody2D rb;
    public Animator animator;
    public SpriteRenderer spriteRenderer;
    public Vector2 moveInput;

    [Header("�̵�")]
    public float moveSpeed = 5f;
    public bool isMoving = false;

    [Header("����")]
    public float jumpForce = 8f;        // ���� �� ���� ���� ũ��
    public LayerMask groundLayer;       // �ٴ����� �ν��� ���̾�
    public bool isGrounded = true;      // ���� �ٴڿ� ����ִ��� ����
    public BoxCollider2D coll;          // �ٴ� üũ�� ���� Collider2D �߰�

    [Header("����")]
    public Weapon currentWeapon;
    public Gun yuseongWeapon;
    public bool isAttacking = false;

    [Header("Managers")]
    public Gamemanager gameManager;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        coll = GetComponent<BoxCollider2D>();

        yuseongWeapon = GetComponentInChildren<Gun>();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (isMoving == false && context.started)
        {
            animator.SetBool("isWalkEnd", false);
            animator.SetTrigger("isWalkStart");
        }
        moveInput = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (isGrounded)
            {
                animator.SetTrigger("isJump");
                rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                Debug.Log("����!");
            }
        }
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            isAttacking = true;
            animator.SetTrigger("isAttack");
        }
    }

    public void OnSwap(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (PN == PLAYERNAME.YUSEONG)
           {
                PN = PLAYERNAME.SEOLHAN;
           }

           else
           {
                PN = PLAYERNAME.YUSEONG;
           }

            Debug.Log("ĳ����:" + PN);
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

    private void FixedUpdate()
    {
        if (!gameManager.isGroggy)
        {
            // �̵�
            if (moveInput.x != 0)
            {
                isMoving = true;
                rb.linearVelocity = new Vector2(moveInput.x * moveSpeed, rb.linearVelocity.y);
                float horizontalVelocity = Mathf.Abs(rb.linearVelocity.x);

                if (isGrounded)
                    animator.SetFloat("isWalk", horizontalVelocity);
            }

            else
            {
                isMoving = false;
                rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
                animator.SetFloat("isWalk", 0);
                animator.SetBool("isWalkEnd", true);
            }
        }

        // �¿����
        FlipCharacter(rb.linearVelocityX);
        
        // �ٴ� üũ
        CheckGround();
    }
}
