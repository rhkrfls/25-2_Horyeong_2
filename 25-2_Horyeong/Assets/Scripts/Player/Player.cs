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

    [Header("�̵�")]
    public float moveSpeed = 5f;

    private void Awake()
    {
        playerName = PLAYERNAME.YUSEONG;
        playerstate = PLAYERSTATE.IDLE;

        rb = GetComponent<Rigidbody2D>();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        // �¿� �̵��� ���� Vector2 �Է� ��(X�ุ -1 �Ǵ� 1)�� �о�ɴϴ�.
        // Y���� �̹� ���ε��� �����߱� ������ 0���� ���ɴϴ�.
        moveInput = context.ReadValue<Vector2>();
    }

    public void OnRun(InputAction.CallbackContext context)
    {

    }

    void FixedUpdate()
    {
        // �¿� �Է� �� (moveInput.x)�� ����ϰ�, Y�� �ӵ�(linearVelocity.y)�� �״�� �����մϴ�.
        rb.linearVelocity = new Vector2(
            moveInput.x * moveSpeed,
            rb.linearVelocity.y
        );
    }
}
