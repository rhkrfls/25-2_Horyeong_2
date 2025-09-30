using System.Data.Common;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine.UI;
using System.Collections; 

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

    [Header("# Monster_Attack")]
    [SerializeField]
    private int monster_damage;           // ���� ������
    [SerializeField]
    private float attackDelay;            // ���� ������
    [SerializeField]
    private float attackDistance;         // �����Ÿ�

    [Header("# Monster_LayerMask")]
    public LayerMask layer;         // Ÿ�� ����ũ

    [Header("# Sound")]
    [SerializeField]
    private AudioClip[] sound_Normal;
    [SerializeField]
    private AudioClip sound_Hurt;
    [SerializeField]
    private AudioClip sound_Dead;

    [Header("# Monster_Hp")]
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
    private bool monster_detected = false;
    private bool monster_attacking = false;
    private bool monster_chasing = false;
    private bool isStunned = false;

    private Transform player;

    public Collider2D[] coll;
    public Collider2D Short_player;
    private SpriteRenderer spriteRenderer;

    /*[SerializeField]
    private Item item_Prefab;             // ������*/
    [SerializeField]
    public int itemNumber;             // ������ ȹ�� ����

    // ���º���
    private bool isChasing;             // �߰������� �Ǻ�
    private bool isAttacking;           // �������� �Ǻ�
    private bool isDead;                   // �׾����� �Ǻ�

    // �ʿ��� ������Ʈ
    [SerializeField]
    public Animator animator;
    [SerializeField]
    public Rigidbody rigid;
    [SerializeField]
    public BoxCollider boxCol;
    public AudioSource theAudio;

    private void Start()
    {
        theAudio = GetComponent<AudioSource>();

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

    protected virtual void Update()
    {
        if (!isDead)
        {
            switch (Race_enumType)
            {
                case race.ANIMAL:

                    break;
                case race.PLANT:

                    break;
                case race.HUMAN:

                    break;
            }
            switch (Attack_enumType)
            {
                case monster_attack_type.MELEE:

                    break;
                case monster_attack_type.MID_RANGED:

                    break;
                case monster_attack_type.RANGED:

                    break;
            }
            switch (Id_enumType)
            {
                case monster_id.CROW:
                    Crow();
                    break;
                case monster_id.STRAYDOG:
                    Dog();
                    break;
                case monster_id.PINE:
                    Tree();
                    break;
                case monster_id.DANDELION:
                    Grass();
                    break;
            }
        }
    }


    // �ൿ����
    public void Stun(float duration)
    {
        if (!isStunned)
            StartCoroutine(StunRoutine(duration));
    }

    private IEnumerator StunRoutine(float duration)
    {
        isStunned = true;
        Debug.Log("�÷��̾� ���� ���� (�̵� �Ұ�, ���� ����)");
        yield return new WaitForSeconds(duration);
        isStunned = false;
        monster_attacking = false;
        Debug.Log("���� ����");
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

            if(!monster_attacking)
            {
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
                switch (Id_enumType)
                {
                    case monster_id.CROW:
                        
                        break;
                    case monster_id.STRAYDOG:
                        Stun(1f);
                        break;
                    case monster_id.PINE:
                        
                        break;
                    case monster_id.DANDELION:
                        
                        break;
                }
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
    // ����� ��ũ��Ʈ

    public virtual void Damage(int _dmg)
    {
        if (!isDead)
        {
            monster_hp -= _dmg;

            if (monster_hp <= 0)
            {
                Dead();
                return;
            }

            PlaySE(sound_Hurt);
            //animator.SetTrigger("Hit");
        }
    }

    private void Rat()
    {

    }

    private void Cat()
    {

    }

    private void Dog()
    {
        MonsterSightRange();
        MonsterAttack();
    }

    private void Crow()
    {

    }

    private void Tree()
    {

    }

    private void Grass()
    {

    }

    protected void Dead()
    {
        PlaySE(sound_Dead);
        isAttacking = false;
        isDead = true;
        animator.SetTrigger("Dead");
    }

    protected void RandomSound()
    {
        int _random = Random.Range(0, 3); // �ϻ� ���� 3��.
        PlaySE(sound_Normal[_random]);
    }

    protected void PlaySE(AudioClip _clip)
    {
        theAudio.clip = _clip;
        theAudio.Play();
    }
}
