using System.Data.Common;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine.UI;
using System.Collections;

public class Enemy : MonoBehaviour
{
    protected enum monster_attack_type
    {
        MELEE,
        MID_RANGED,
        RANGED
    };

    protected enum monster_id
    {
        RAT,
        CAT,
        STRAYDOG,
        CROW,
        PINE,
        DANDELION
    };

    protected enum race
    {
        HUMAN,
        ANIMAL,
        PLANT
    };

    [Header("# Monster_Enum_type")]
    [SerializeField]
    protected monster_attack_type Attack_enumType;
    [SerializeField]
    protected monster_id Id_enumType;
    [SerializeField]
    protected race Race_enumType;

    [Header("# Monster_Attack")]
    [SerializeField]
    protected int monster_damage;           // ���� ������
    [SerializeField]
    protected float attackDistance;         // �����Ÿ�


    [Header("# Monster_LayerMask")]
    [SerializeField]
    protected LayerMask layer;         // Ÿ�� ����ũ

    [Header("# Sound")]
    [SerializeField]
    protected AudioClip[] sound_Normal;
    [SerializeField]
    protected AudioClip sound_Hurt;
    [SerializeField]
    protected AudioClip sound_Dead;

    [Header("# Monster_Hp")]
    [SerializeField]
    protected int monster_maxHp = 1;
    protected int monster_hp = 1;

    [Header("# Monster_Range")]
    [SerializeField]
    protected float monster_sight_range = 0f;
    [SerializeField]
    protected float monster_attack_range = 0f;
    [SerializeField]
    protected float monster_chase_ranges = 0f;

    [Header("# Monster_Speed")]
    [SerializeField]
    protected float monster_speed = 5f;
    [SerializeField]
    protected float monster_rotationSpeed = 5f;

    [Header("# Monster_Bool")]
    protected bool monster_detected = false;
    protected bool monster_attacking = false;        // �������� �Ǻ�
    protected bool monster_chasing = false;
    protected bool isStunned = false;
    protected bool isChasing;                   // �߰������� �Ǻ�
    protected bool isDead;                      // �׾����� �Ǻ�

    protected Transform player;
    [SerializeField]
    protected Gamemanager gameManager;
    [SerializeField]
    protected Collider2D[] coll;
    [SerializeField]
    protected Collider2D Short_player;
    [SerializeField]
    protected SpriteRenderer spriteRenderer;

    /*[SerializeField]
    private Item item_Prefab;             // ������*/
    [SerializeField]
    protected int itemNumber;             // ������ ȹ�� ����

    // �ʿ��� ������Ʈ
    [SerializeField]
    protected Animator animator;
    [SerializeField]
    protected Rigidbody rigid;
    [SerializeField]
    protected BoxCollider boxCol;
    [SerializeField]
    protected AudioSource theAudio;

    private void Start()
    {
        theAudio = GetComponent<AudioSource>();

        player = GameObject.FindGameObjectWithTag("Player").transform;
        spriteRenderer = GetComponent<SpriteRenderer>();
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
        }
    }

    protected void MonsterSightRange()
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

    protected bool MonsterView()
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
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }


    // ����
    protected IEnumerator MonsterAttackCoroutine(float AttackDelay, float StunDelay)
    {
        monster_attacking = true;
        switch (Id_enumType)
        {
            case monster_id.STRAYDOG:
                if (Random.value < 0.3f && !isStunned)
                {
                    StartCoroutine(DogSpecialAttack(StunDelay));
                }
                else
                {
                    Attack();
                }
                break;
            case monster_id.CAT:
                Attack();
                break;
        }
        yield return new WaitForSeconds(AttackDelay);
        monster_attacking = false;
    }

    // ����
    protected void Attack()
    {
        Debug.Log("�Ϲݰ���");
        //animator.SetTrigger("Attack");
        //thePlayerStatus.DecreaseHP(attackDamage);
        //GameManager.damage = true;
    }

    // ����
    protected IEnumerator DogSpecialAttack(float StunDelay)
    {
        Debug.Log("10% Ȯ���� ����!");
        gameManager.isGroggy = true;
        isStunned = true;
        // 0.5�� ���� 3�� ����
        for (int i = 0; i < 3; i++)
        {
            float damage = monster_damage * 0.5f;
            Debug.Log($"Ư�� ���� {i + 1}ȸ��: {damage}");
            yield return new WaitForSeconds(StunDelay);
            // player.TakeDamage(damage);
        }
        isStunned = false;
    }

    protected void OnDestroy()
    {
        Short_player = null;
    }

    protected void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, monster_sight_range);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, monster_attack_range);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, monster_chase_ranges);
    }
    // ����� ��ũ��Ʈ

    protected virtual void Damage(int _dmg)
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

    protected void Dead()
    {
        PlaySE(sound_Dead);
        monster_attacking = false;
        isDead = true;
        animator.SetTrigger("Dead");
    }

    protected virtual void ReSet()
    {
        //isWalking = false;
        //animator.SetBool("Running", isRunning);
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