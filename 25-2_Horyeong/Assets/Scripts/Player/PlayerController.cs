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

    [Header("�̵�")]
    public float moveSpeed = 5f;

    [Header("����")]
    public float jumpForce = 8f;        // ���� �� ���� ���� ũ��
    public LayerMask groundLayer;       // �ٴ����� �ν��� ���̾�
    public bool isGrounded = true;            // ���� �ٴڿ� ����ִ��� ����
    public BoxCollider2D coll;         // �ٴ� üũ�� ���� Collider2D �߰�


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
                Debug.Log("����!");
            }
        }
    }

    // 2. �ٴ� üũ �Լ�
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
        rb.linearVelocity = new Vector2(moveInput.x * moveSpeed, rb.linearVelocity.y);
        CheckGround();
    }


}
