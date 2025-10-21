using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 3.5f;
    public float lifetime = 2f; // �Ѿ��� ���� (2�� �� �ڵ� ��Ȱ��ȭ)
    private Rigidbody2D rb;
    public Vector2 direction;
    private float timeElapsed;

    public int damage;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // �Ѿ��� ��Ȱ���� �� ȣ��Ǵ� �ʱ�ȭ �Լ�
    public void Launch(Vector2 shootDirection, int bulletDamage)
    {
        direction = shootDirection.normalized; // ���� ���� ����ȭ
        damage = bulletDamage;
        timeElapsed = 0f;
        gameObject.SetActive(true);
        // Rigidbody�� �̿��Ͽ� �Ѿ� �߻�
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