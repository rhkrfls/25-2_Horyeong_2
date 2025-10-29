using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

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
    public bool isMoving = false;
    public float maxMoveSpeed = 7f;         // ������ ����ϴ� moveSpeed ��� �ִ� �ӵ��� ���
    public float accelerationRate = 10f;

    [Header("����")]
    public float jumpForce = 8f;        // ���� �� ���� ���� ũ��
    public LayerMask groundLayer;       // �ٴ����� �ν��� ���̾�
    public bool isGrounded = true;      // ���� �ٴڿ� ����ִ��� ����
    public BoxCollider2D coll;          // �ٴ� üũ�� ���� Collider2D �߰�

    [Header("����")]
    public Weapon currentWeapon;
    public Gun yuseongWeapon;
    public bool isAttacking = false;
    public void SetisAttacking() { Debug.Log($"���� ����: {isAttacking}"); this.isAttacking = false; }

    [Header("Knockback Settings")]
    public float knockbackPower = 5f;   // �˹��� ���� (���� ����)
    public float knockbackDuration = 0.3f; // �˹��� ���ӵǴ� �ð� (��)

    // �÷��̾� ���� �÷��� (�̵� �� ���� ���ܿ�)
    public bool isKnockedBack = false;

    private Map_Interaction currentInteractable;

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
        if (isMoving == false && context.started && !isKnockedBack)
        {
            animator.SetBool("isWalkEnd", false);
            animator.SetBool("isWalkStart", true);
        }
        moveInput = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started && !isKnockedBack)
        {
            if (isGrounded)
            {
                animator.SetTrigger("isJump");
                rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            }
        }
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.started && !isAttacking && !isKnockedBack)
        {
            isAttacking = true;
            animator.SetTrigger("isAttack");
        }

        //SetisAttackingFalse();
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.started && !isKnockedBack)
        {
            // ���� ��ȣ�ۿ� ������ ������Ʈ�� �ְ�,
            // (Interactable ��ũ��Ʈ ���ο��� isPlayerInRange�� �ٽ� Ȯ��)
            if (currentInteractable != null)
            {
                currentInteractable.Interact();
                currentInteractable.SetIsIntrecting(true);
            }
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

    public void ApplyKnockback(Transform attacker)
    {
        // �̹� �˹� ���̰ų� ���� ���¶�� ����
        if (isKnockedBack) return;

        // �˹� �ڷ�ƾ ����
        StartCoroutine(KnockbackRoutine(attacker));
    }

    IEnumerator KnockbackRoutine(Transform attacker)
    {
        isKnockedBack = true;

        // 1. �˹� ���� ���
        // ������ ��ġ�� �÷��̾��� ��ġ�� ���Ͽ� �з��� ������ ����
        Vector2 knockbackDirection;

        // ���Ͱ� �÷��̾��� ���ʿ� ������ ������(1), �����ʿ� ������ ����(-1)���� �з���
        float directionX = (transform.position.x > attacker.position.x) ? 1f : -1f;

        // 2. Rigidbody�� �������� �� ���� (Impulse)
        // Y������ �ణ ���� ���� �߰��Ͽ� �ð����� ȿ���� ����
        knockbackDirection = new Vector2(directionX, 0.5f).normalized; // �밢�� ���� ���� �ֱ� ���� Y�� �߰�

        // ���� �ӵ��� �ʱ�ȭ�ϰ� �˹� ���� ����
        rb.linearVelocity = Vector2.zero;
        rb.AddForce(knockbackDirection * knockbackPower, ForceMode2D.Impulse);

        // 3. �˹� ���� �ð� ���� ���
        yield return new WaitForSeconds(knockbackDuration);

        // 4. �˹� ���� ����
        isKnockedBack = false;

        // �˹��� ���� �� Rigidbody �ӵ� ���� (���� �˹� �� ���� �ε����� �ʾҴٸ� �ӵ��� �������� �� ����)
        if (rb.linearVelocity.y < 0.1f) // ���� ���� �ƴ϶��
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
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

    public void SetCurrentInteractable(Map_Interaction interactable)
    {
        currentInteractable = interactable;
    }

    private void FixedUpdate()
    {
        if (isKnockedBack) return;

        if (!gameManager.isGroggy && !isAttacking)
        {
            // �̵�
            if (moveInput.x != 0)
            {
                isMoving = true;

                // 1. ��ǥ �ӵ� ���
                float targetSpeedX = moveInput.x * maxMoveSpeed;

                float newVelocityX = Mathf.Lerp(
                    rb.linearVelocity.x,                // ���� X �ӵ�
                    targetSpeedX,                       // ��ǥ X �ӵ�
                    Time.deltaTime * accelerationRate   // ���� ���� (deltaTime�� ���� �����ӿ� ���������� ����)
                );

                rb.linearVelocity = new Vector2(newVelocityX, rb.linearVelocity.y);

                float horizontalVelocity = Mathf.Abs(rb.linearVelocity.x);

                if (isGrounded)
                    animator.SetFloat("isWalk", horizontalVelocity);
            }

            else
            {
                isMoving = false;

                float targetSpeedX = 0f;

                float newVelocityX = Mathf.Lerp(
                    rb.linearVelocity.x,
                    targetSpeedX,
                    Time.deltaTime * accelerationRate * 2f
                );

                rb.linearVelocity = new Vector2(newVelocityX, rb.linearVelocity.y);
                animator.SetFloat("isWalk", 0);
                animator.SetBool("isWalkStart", false);
                animator.SetBool("isWalkEnd", true);
            }   
        }

        else
        {
            isMoving = false;

            rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
        }

        if (!isGrounded && rb.linearVelocity.y < -0.1f)
        {
            animator.SetBool("isJumpEnd", true);
        }
        // �ٴڿ� ��Ҵٸ� IsFalling�� false�� ���� (���� �Ϸ�)
        else if (isGrounded)
        {
            animator.SetBool("isJumpEnd", false);
        }

        // �¿����
        FlipCharacter(rb.linearVelocityX);
        
        // �ٴ� üũ
        CheckGround();
    }
}
