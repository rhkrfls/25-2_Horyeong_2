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
                // 방향 구하기
                Vector3 direction = Short_player.transform.position - transform.position;
                direction.Normalize();

                // 이동 (X축만 이동)
                transform.position += new Vector3(direction.x, 0, 0) * speed * Time.deltaTime;

                // 좌우 반전 (플레이어 위치에 따라 flip)
                if (direction.x < 0)
                    spriteRenderer.flipX = false; // 왼쪽 바라봄
                else if (direction.x > 0)
                    spriteRenderer.flipX = true;  // 오른쪽 바라봄
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
