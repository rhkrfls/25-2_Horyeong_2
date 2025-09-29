using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    // �̵� �ӵ��� Inspector���� ������ �� �ֵ��� public ���� ����
    public float moveSpeed = 5f;

    // Rigidbody2D ������Ʈ�� ������ ����
    private Rigidbody2D rb;

    // Input System���κ��� ���� �Է� ���� ������ Vector2 ����
    private Vector2 moveInput;

    // ��ũ��Ʈ�� ó�� Ȱ��ȭ�� �� �� �� ȣ��
    void Awake()
    {
        // Rigidbody2D ������Ʈ�� �����ͼ� rb ������ �Ҵ�
        rb = GetComponent<Rigidbody2D>();
    }

    // FixedUpdate�� ���� ����� ���� ������ �ð� �������� ȣ��˴ϴ�.
    void FixedUpdate()
    {
        // �Է� ��(moveInput)�� ������� Rigidbody2D�� ���� �ӵ�(linearVelocity)�� ����
        // y�� �ӵ��� �߷¿� ���� �ڿ������� ���ϵ��� ���� (rb.linearVelocity.y)
        rb.linearVelocity = new Vector2(moveInput.x * moveSpeed, rb.linearVelocity.y);
    }

    // New Input System���� Move Action�� ȣ��� �� �ڵ����� ����Ǵ� �ݹ� �Լ�
    // Player Input ������Ʈ���� Move Action�� Type�� Value�� �����ϰ�, 
    // Behavior�� Invoke Unity Events�� �����ؾ� �մϴ�.
    public void OnMove(InputAction.CallbackContext context)
    {
        // �Է� ��(Vector2)�� �о moveInput ������ ����
        moveInput = context.ReadValue<Vector2>();
    }
}