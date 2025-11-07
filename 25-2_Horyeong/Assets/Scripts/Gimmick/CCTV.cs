using UnityEngine;

public class CCTV : MonoBehaviour
{
    public PlayerStatus playerStatus;
    public int damage;

    public BoxCollider2D boxColl;
    public BoxCollider2D detect_boxColl;
    public Rigidbody2D rigid;

    private bool isGround = false;
    public float groundCheckDistance = 0.5f;
    public LayerMask groundLayer;


    void Start()
    {
        rigid.gravityScale = 0;

        if (playerStatus == null)
        {
            playerStatus = FindAnyObjectByType<PlayerStatus>();
        }
    }


    void Update()
    {
        if (!isGround)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, groundLayer);
            if (hit.collider != null)
            {
                isGround = true;
                rigid.gravityScale = 0; // ¶³¾îÁø ÈÄ ¸ØÃã
            }
        }
        if(isGround)
            damage = 0;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision == detect_boxColl)
            return;

        if (collision.CompareTag("Player") && !isGround)
        {
            if (detect_boxColl.IsTouching(collision))
            {
                rigid.gravityScale = 3;
            }
        }

        if (!isGround && collision.CompareTag("Player") && boxColl.IsTouching(collision))
        {
            if (playerStatus != null)
            {
                playerStatus.TakeDamage(damage, this.transform);
                isGround = true;
            }
        }
    }
}
