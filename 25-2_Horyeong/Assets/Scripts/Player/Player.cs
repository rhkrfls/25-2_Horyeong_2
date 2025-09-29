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

public class Player : MonoBehaviour
{
    private PLAYERNAME playerName;
    private PLAYERSTATE playerstate;
    private Rigidbody2D rb;
    private Vector2 moveInput;

    [Header("이동")]
    public float moveSpeed = 5f;

    private void Awake()
    {
        playerName = PLAYERNAME.YUSEONG;
        playerstate = PLAYERSTATE.IDLE;

        rb = GetComponent<Rigidbody2D>();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        // 좌우 이동에 대한 Vector2 입력 값(X축만 -1 또는 1)을 읽어옵니다.
        // Y축은 이미 바인딩을 제거했기 때문에 0으로 들어옵니다.
        moveInput = context.ReadValue<Vector2>();
    }

    public void OnRun(InputAction.CallbackContext context)
    {

    }

    void FixedUpdate()
    {
        // 좌우 입력 값 (moveInput.x)만 사용하고, Y축 속도(linearVelocity.y)는 그대로 유지합니다.
        rb.linearVelocity = new Vector2(
            moveInput.x * moveSpeed,
            rb.linearVelocity.y
        );
    }
}
