using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    // 이동 속도를 Inspector에서 설정할 수 있도록 public 변수 선언
    public float moveSpeed = 5f;

    // Rigidbody2D 컴포넌트를 저장할 변수
    private Rigidbody2D rb;

    // Input System으로부터 받은 입력 값을 저장할 Vector2 변수
    private Vector2 moveInput;

    // 스크립트가 처음 활성화될 때 한 번 호출
    void Awake()
    {
        // Rigidbody2D 컴포넌트를 가져와서 rb 변수에 할당
        rb = GetComponent<Rigidbody2D>();
    }

    // FixedUpdate는 물리 계산을 위해 일정한 시간 간격으로 호출됩니다.
    void FixedUpdate()
    {
        // 입력 값(moveInput)을 기반으로 Rigidbody2D의 선형 속도(linearVelocity)를 설정
        // y축 속도는 중력에 의해 자연스럽게 변하도록 유지 (rb.linearVelocity.y)
        rb.linearVelocity = new Vector2(moveInput.x * moveSpeed, rb.linearVelocity.y);
    }

    // New Input System에서 Move Action이 호출될 때 자동으로 실행되는 콜백 함수
    // Player Input 컴포넌트에서 Move Action의 Type을 Value로 설정하고, 
    // Behavior를 Invoke Unity Events로 설정해야 합니다.
    public void OnMove(InputAction.CallbackContext context)
    {
        // 입력 값(Vector2)을 읽어서 moveInput 변수에 저장
        moveInput = context.ReadValue<Vector2>();
    }
}