using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 3.5f;
    public float lifetime = 2f; // 총알의 수명 (2초 후 자동 비활성화)
    private Rigidbody2D rb;
    public Vector2 direction;
    private float timeElapsed;

    public int damage;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // 총알을 재활용할 때 호출되는 초기화 함수
    public void Launch(Vector2 shootDirection, int bulletDamage)
    {
        direction = shootDirection.normalized; // 방향 벡터 정규화
        damage = bulletDamage;
        timeElapsed = 0f;
        gameObject.SetActive(true);
        // Rigidbody를 이용하여 총알 발사
        rb.linearVelocity = direction * speed;
    }

    private void Update()
    {
        timeElapsed += Time.deltaTime;
        if (timeElapsed >= lifetime)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
    }
}