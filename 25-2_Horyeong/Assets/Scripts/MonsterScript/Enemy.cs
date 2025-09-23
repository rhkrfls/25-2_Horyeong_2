using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 5f;
    public float rotationSpeed = 5f;

    private Transform player;

    public float radius = 0f;

    public LayerMask layer;

    public Collider2D[] coll;
    public Collider2D Short_player;
    private SpriteRenderer spriteRenderer;


    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        coll = Physics2D.OverlapCircleAll((Vector2)transform.position, radius, layer);

        if (coll.Length > 0)
        {
            float short_distance = Vector3.Distance(transform.position, coll[0].transform.position);

            foreach (Collider2D col in coll)
            {
                if (col == null) continue;

                float short_distance2 = Vector3.Distance(transform.position, col.transform.position);
                if (short_distance > short_distance2)
                {
                    short_distance = short_distance2;
                    Short_player = col;
                }
            }

            if (Short_player != null)
            {
                // ���� ���ϱ�
                Vector3 direction = Short_player.transform.position - transform.position;
                direction.Normalize();

                // �̵� (X�ุ �̵�)
                transform.position += new Vector3(direction.x, 0, 0) * speed * Time.deltaTime;

                // �¿� ���� (�÷��̾� ��ġ�� ���� flip)
                if (direction.x < 0)
                    spriteRenderer.flipX = false; // ���� �ٶ�
                else if (direction.x > 0)
                    spriteRenderer.flipX = true;  // ������ �ٶ�
            }
        }
    }

    private void OnDestroy()
    {
        Short_player = null;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
