using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;


public class PlayerController : MonoBehaviour
{
    public CharacterData currentData;
    public Rigidbody2D rb;
    public Animator animator;
    public SpriteRenderer spriteRenderer;
    public Vector2 moveInput;

    [Header("이동")]
    public bool isMoving = false;


    [Header("점프")]
    public LayerMask groundLayer;       
    public bool isGrounded = true;      
    public BoxCollider2D coll;          

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

    public Map_Interaction currentInteractable;

    [Header("Managers")]
    public Gamemanager gameManager;
    public Player      swapManager;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        coll = GetComponent<BoxCollider2D>();

        yuseongWeapon = GetComponentInChildren<Gun>();
    }

    public void ResetPlayer()
    {
        if (currentData.currentPlayerCharachter.ToString() != DataManager.Instance.gameData.activeCharacterName)
        {
            swapManager.SwapCharacter();
        }

        transform.position = new Vector3(
            DataManager.Instance.gameData.playerPositionX,
            DataManager.Instance.gameData.playerPositionY,
            transform.position.z
        );

        animator.SetBool("isDeath", false);
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
                rb.AddForce(Vector2.up * currentData.jumpForce, ForceMode2D.Impulse);
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
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.started && !isKnockedBack)
        {
            // 현재 상호작용 가능한 오브젝트가 있고,
            // (Interactable 스크립트 내부에서 isPlayerInRange를 다시 확인)
            if (currentInteractable != null)
            {
                currentInteractable.Interact(this);
                currentInteractable.SetIsIntrecting(true);
            }
        }
    }

    public void OnSwap(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            
            swapManager.SwapCharacter();
            Debug.Log("캐릭터 변경");
        }
    }

    public void LoadCharacter(CharacterData newData)
    {
        if (newData == null) return;

        currentData = newData;

        // 1. 애니메이터 컨트롤러 교체 (캐릭터 외형/애니메이션 변경)
        animator.runtimeAnimatorController = currentData.animatorController;

        rb.mass = currentData.mass;
        rb.gravityScale = currentData.gravityScale;

        Debug.Log($"캐릭터가 스왑되었습니다. 새 이동 속도: {currentData.maxMoveSpeed}");
    }

    public void callBackGameStop()
    {
        gameManager.SetGameStop();
    }

    public void ApplyKnockback(Transform attacker)
    {
        if (isKnockedBack) return;

        StartCoroutine(KnockbackRoutine(attacker));
    }

    IEnumerator KnockbackRoutine(Transform attacker)
    {
        isKnockedBack = true;

        spriteRenderer.color = Color.pink;

        Vector2 knockbackDirection;
        Debug.Log($"this: {transform.position.x}");

        Debug.Log($"attacker: {attacker.position.x}");

        float directionX = (transform.position.x > attacker.position.x) ? 1f : -1f;
        Debug.Log($"Knockback Direction X: {directionX}");
        knockbackDirection = new Vector2(directionX, 0.5f).normalized;

        rb.linearVelocity = Vector2.zero;
        rb.AddForce(knockbackDirection * knockbackPower, ForceMode2D.Impulse);

        yield return new WaitForSeconds(knockbackDuration);

        isKnockedBack = false;

        spriteRenderer.color = Color.white; // 넉백이 끝난 후 원래 색상으로 복원

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
                float targetSpeedX = moveInput.x * currentData.maxMoveSpeed;

                float newVelocityX = Mathf.Lerp(
                    rb.linearVelocity.x,                // 현재 X 속도
                    targetSpeedX,                       // 목표 X 속도
                    Time.deltaTime * currentData.accelerationRate   // 가속 비율 (deltaTime을 곱해 프레임에 독립적으로 만듦)
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
                    Time.deltaTime * currentData.accelerationRate * 2f
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
