using UnityEngine;
using UnityEngine.InputSystem;

enum PLAYERSTATE
{
    IDLE, WALK, RUN, 
}

enum PLAYERNAME
{
    YUSEONG, SEOLHAN
}

public class PlayerController : MonoBehaviour
{
    private PLAYERNAME playerName;
    private PLAYERSTATE playerstate;
    private Rigidbody2D rb;
    private Vector2 moveInput;

    [Header("이동")]
    public float moveSpeed = 5f;

    [Header("점프")]
    public float jumpForce = 8f;        // 점프 시 가할 힘의 크기
    public LayerMask groundLayer;       // 바닥으로 인식할 레이어
    public bool isGrounded = true;            // 현재 바닥에 닿아있는지 여부
    public BoxCollider2D coll;         // 바닥 체크를 위해 Collider2D 추가


    private void Awake()
    {
        playerName = PLAYERNAME.YUSEONG;
        playerstate = PLAYERSTATE.IDLE;

        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }
    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (isGrounded)
            {
                rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                Debug.Log("점프!");
            }
        }
    }

    // 2. 바닥 체크 함수
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
    void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(moveInput.x * moveSpeed, rb.linearVelocity.y);
        CheckGround();
    }


}
