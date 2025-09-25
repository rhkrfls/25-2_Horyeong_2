using UnityEngine;

public class Enemy : MonoBehaviour
{
    public enum monster_attack_type
    {
        MELEE,
        MID_RANGED,
        RANGED
    };

    public enum monster_id
    {
        STRAYDOG,
        CROW,
        PINE,
        DANDELION
    };

    public enum race
    {
        HUMAN,
        ANIMAL,
        PLANT
    };

    [Header("# Monster_Enum_type")]
    public monster_attack_type Attack_enumType;
    public monster_id Id_enumType;
    public race Race_enumType;

    [Header("# Monster_Int")]
    public int monster_damage = 1;
    public int monster_hp = 1;
    public int monster_maxHp = 1;

    [Header("# Monster_Range")]
    public float monster_sight_range = 0f;
    public float monster_attack_range = 0f;
    public float monster_chase_ranges = 0f;

    [Header("# Monster_Speed")]
    public float monster_speed = 5f;
    public float monster_rotationSpeed = 5f;

    [Header("# Monster_Bool")]
    public bool monster_detected = false;
    public bool monster_attacking = false;
    public bool monster_chasing = false;


    [Header("# Monster_LayerMask")]
    public LayerMask layer;

    private Transform player;

    public Collider2D[] coll;
    public Collider2D Short_player;
    private SpriteRenderer spriteRenderer;


    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        foreach(Collider2D collider2D in coll)
        {
            if(collider2D != null)
            {
                collider2D.enabled = true;
            }
        }
        monster_hp = monster_maxHp;
    }

    private void Update()
    {
        MonsterSightRange();
        MonsterAttack();
    }

    private void MonsterSightRange()
    {
        coll = Physics2D.OverlapCircleAll((Vector2)transform.position, monster_sight_range, layer);

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
                transform.position += new Vector3(direction.x, 0, 0) * monster_speed * Time.deltaTime;

                // �¿� ���� (�÷��̾� ��ġ�� ���� flip)
                if (direction.x < 0)
                    spriteRenderer.flipX = false; // ���� �ٶ�
                else if (direction.x > 0)
                    spriteRenderer.flipX = true;  // ������ �ٶ�
            }
        }
    }

    private void MonsterAttack()
    {
        coll = Physics2D.OverlapCircleAll((Vector2)transform.position, monster_attack_range, layer);

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
                monster_attacking = true;
            }
        }
        else
        {
            monster_attacking = false;
        }
    }

    private void OnDestroy()
    {
        Short_player = null;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, monster_sight_range);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, monster_attack_range);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, monster_chase_ranges);
    }
}
