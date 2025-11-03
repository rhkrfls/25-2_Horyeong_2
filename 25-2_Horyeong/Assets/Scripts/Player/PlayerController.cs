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

    [Header("이동")]
    public bool isMoving = false;
    public float maxMoveSpeed = 7f;         // 이전에 사용하던 moveSpeed 대신 최대 속도로 사용
    public float accelerationRate = 10f;

    [Header("점프")]
    public float jumpForce = 8f;        // 점프 시 가할 힘의 크기
    public LayerMask groundLayer;       // 바닥으로 인식할 레이어
    public bool isGrounded = true;      // 현재 바닥에 닿아있는지 여부
    public BoxCollider2D coll;          // 바닥 체크를 위해 Collider2D 추가

    [Header("무기")]
    public Weapon currentWeapon;
    public Gun yuseongWeapon;
    public bool isAttacking = false;
    public void SetisAttacking() { Debug.Log($"공격 상태: {isAttacking}"); this.isAttacking = false; }

    [Header("Knockback Settings")]
    public float knockbackPower = 5f;   // 넉백의 강도 (수정 가능)
    public float knockbackDuration = 0.3f; // 넉백이 지속되는 시간 (초)

    // 플레이어 상태 플래그 (이동 및 공격 차단용)
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
            // 현재 상호작용 가능한 오브젝트가 있고,
            // (Interactable 스크립트 내부에서 isPlayerInRange를 다시 확인)
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

            Debug.Log("캐릭터:" + PN);
        }
    }

    public void ApplyKnockback(Transform attacker)
    {
        // 이미 넉백 중이거나 무적 상태라면 무시
        if (isKnockedBack) return;

        // 넉백 코루틴 시작
        StartCoroutine(KnockbackRoutine(attacker));
    }

    IEnumerator KnockbackRoutine(Transform attacker)
    {
        isKnockedBack = true;

        spriteRenderer.color = Color.pink; // 넉백 시작 시 색상 변경 (예: 빨간색)
        // 1. 넉백 방향 계산
        // 몬스터의 위치와 플레이어의 위치를 비교하여 밀려날 방향을 결정
        Vector2 knockbackDirection;

        // 몬스터가 플레이어의 왼쪽에 있으면 오른쪽(1), 오른쪽에 있으면 왼쪽(-1)으로 밀려남
        float directionX = (transform.position.x > attacker.position.x) ? 1f : -1f;

        // 2. Rigidbody에 순간적인 힘 적용 (Impulse)
        // Y축으로 약간 띄우는 힘을 추가하여 시각적인 효과를 높임
        knockbackDirection = new Vector2(directionX, 0.5f).normalized; // 대각선 위로 힘을 주기 위해 Y값 추가

        // 기존 속도를 초기화하고 넉백 힘을 가함
        rb.linearVelocity = Vector2.zero;
        rb.AddForce(knockbackDirection * knockbackPower, ForceMode2D.Impulse);

        // 3. 넉백 지속 시간 동안 대기
        yield return new WaitForSeconds(knockbackDuration);

        // 4. 넉백 상태 해제
        isKnockedBack = false;

        spriteRenderer.color = Color.white; // 넉백이 끝난 후 원래 색상으로 복원

        // 넉백이 끝난 후 Rigidbody 속도 정리 (만약 넉백 중 벽에 부딪히지 않았다면 속도가 남아있을 수 있음)
        if (rb.linearVelocity.y < 0.1f) // 점프 중이 아니라면
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
            coll.bounds.center, // 박스캐스트 시작점
            coll.bounds.size,   // 박스캐스트 크기 (캐릭터 콜라이더와 동일)
            0f,                 // 회전 각도
            Vector2.down,       // 아래 방향으로 캐스팅
            0.1f,               // 캐스팅 거리 (아주 짧게)
            groundLayer         // 바닥 레이어 마스크
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
            // 이동
            if (moveInput.x != 0)
            {
                isMoving = true;

                // 1. 목표 속도 계산
                float targetSpeedX = moveInput.x * maxMoveSpeed;

                float newVelocityX = Mathf.Lerp(
                    rb.linearVelocity.x,                // 현재 X 속도
                    targetSpeedX,                       // 목표 X 속도
                    Time.deltaTime * accelerationRate   // 가속 비율 (deltaTime을 곱해 프레임에 독립적으로 만듦)
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
        // 바닥에 닿았다면 IsFalling을 false로 설정 (착지 완료)
        else if (isGrounded)
        {
            animator.SetBool("isJumpEnd", false);
        }

        // 좌우반전
        FlipCharacter(rb.linearVelocityX);
        
        // 바닥 체크
        CheckGround();
    }
}
