using System.Data.Common;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine.UI;
using System.Collections;
using Unity.VisualScripting;

public class Enemy : MonoBehaviour
{
    protected enum monster_attack_type { MELEE, MID_RANGED, RANGED };

    protected enum monster_id { RAT, CAT, STRAYDOG, CROW, PINE, DANDELION };

    protected enum race { HUMAN, ANIMAL, PLANT };

    [Header("# Monster_Enum_type")]
    [SerializeField]
    protected monster_attack_type Attack_enumType;
    [SerializeField]
    protected monster_id Id_enumType;
    [SerializeField]
    protected race Race_enumType;

    // Monster_Attack
    protected int monster_damage;           // ���� ������

    public PlayerStatus playerStatus;


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
    [SerializeField]
    protected AudioClip sound_Attack;

    // Monster_Hp
    protected int monster_maxHp = 1;
    protected int monster_curHp = 1;

    // Monster_Range
    protected float monster_sight_range = 0f;
    protected float monster_attack_range = 0f;

    // Monster_Speed
    protected float monster_speed = 5f;
    protected float monster_rotationSpeed = 5f;

    // Monster_Bool
    protected bool isAction;                        // �ൿ������ �ƴ��� �Ǻ�
    protected bool monster_detected = false;        // �÷��̾� ����
    [SerializeField]
    protected bool monster_attacking = false;       // �������� �Ǻ�
    protected bool isStunned = false;
    protected bool isChasing;                       // �߰������� �Ǻ�
    protected bool isDead;                          // �׾����� �Ǻ�
    protected bool stopAction = false;

    // Monster_Patrol
    protected bool usePatrol = true;        // ���� ��� �ѱ�
    protected float patrolRange = 3f;       // �պ� �Ÿ�
    protected float patrolSpeed = 2f;       // ���� �ӵ�
    protected float patrolWaitTime = 2f;    // �������� ��� �ð�
    private Vector3 startPos;               // ���� ����
    private bool movingRight = true;        // �̵� ���� �Ǻ�
    private bool isWaiting = false;         // ��� ������
    private bool isReturning = false;       // ���� ������

    protected float attack_delay;
    protected float stun_delay;

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

    protected virtual void Start()
    {
        theAudio = GetComponent<AudioSource>();

        player = GameObject.FindGameObjectWithTag("Player").transform;
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (playerStatus == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            GameObject short_player = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                playerStatus = playerObj.GetComponent<PlayerStatus>();
                if (playerStatus == null)
                    Debug.LogWarning("Player ������Ʈ�� PlayerStatus ������Ʈ�� �����ϴ�!");
            }
            if (Short_player != null)
            {
                GameObject player = GameObject.Find("Player");
                Short_player = player.GetComponent<CapsuleCollider2D>();
            }
            else
            {
                Debug.LogWarning("'Player' �±װ� ������ ������Ʈ�� ã�� �� �����ϴ�!");
            }
        }

        // GameManager �ڵ� ����
        if (gameManager == null)
        {
            gameManager = GetComponent<Gamemanager>();
            if (gameManager == null)
                Debug.LogWarning("������ Gamemanager�� ã�� �� �����ϴ�!");
        }

        startPos = transform.position;
        StartCoroutine(PatrolRoutine());
    }

    protected virtual void Update()
    {
        if (isDead || isStunned) return;

        monster_detected = MonsterSightRange();
        bool inAttackRange = MonsterInAttackRange();

        if (monster_detected)
        {
            // ���� ���� ���̸� �߰� ����, ���ڸ����� ����
            if (inAttackRange)
            {
                if (isChasing)
                {
                    isChasing = false;
                    SetAnimationState(true, false, false); // run �� idle ��ȯ
                }

                // ����
                if (!monster_attacking)
                {
                    StartCoroutine(MonsterAttackCoroutine(attack_delay, stun_delay));
                }
            }
            else
            {
                // ���� ���� ���̸� �߰�
                isChasing = true;
                SetAnimationState(false, false, true); // Idle���� run����
                ChasePlayer();
            }
        }
        else if (isChasing)
        {
            // �÷��̾� ������ �� ���� �ڸ��� ����
            StartCoroutine(ReturnToStart());
        }
    }


    protected bool MonsterSightRange()
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
                isChasing = true;
                return true;
            }
        }

        isChasing = false;
        return false;
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

    // ���� �Լ�
    protected IEnumerator PatrolRoutine()
    {
        startPos = transform.position; // ���� ��ġ

        while (!isDead && usePatrol)
        {
            if (monster_detected || monster_attacking || isStunned)
            {
                yield return null;
                continue;
            }

            if (!isWaiting)
            {
                SetAnimationState(false, true, false);

                // �̵�
                float moveDir = movingRight ? 1f : -1f;
                transform.Translate(Vector2.right * moveDir * patrolSpeed * Time.deltaTime);

                // ���� üũ
                if (movingRight && transform.position.x >= startPos.x + patrolRange)
                {
                    movingRight = false;
                    StartCoroutine(PatrolWait());
                }
                else if (!movingRight && transform.position.x <= startPos.x - patrolRange)
                {
                    movingRight = true;
                    StartCoroutine(PatrolWait());
                }

                // �̹��� �ø�
                spriteRenderer.flipX = movingRight;
            }
            else
            {
                // ��� �߿��� Idle �ִϸ��̼�
                SetAnimationState(true, false, false);
            }

            yield return null;
        }
    }

    private IEnumerator PatrolWait()
    {
        isWaiting = true;
        yield return new WaitForSeconds(patrolWaitTime);
        isWaiting = false;
    }


    // �߰� �Լ�
    protected void ChasePlayer()
    {
        if (Short_player == null) return;

        SetAnimationState(false, false, true);

        Vector3 direction = Short_player.transform.position - transform.position;
        direction.Normalize();

        transform.position += new Vector3(direction.x, 0, 0) * monster_speed * Time.deltaTime;

        // �¿� ����
        spriteRenderer.flipX = direction.x > 0; // �������̸� true, �����̸� false

    }

    // ���� �Լ�
    protected IEnumerator ReturnToStart()
    {
        if (isReturning) yield break; // �ߺ� ����
        isReturning = true;

        SetAnimationState(false, true, false);

        while (Vector3.Distance(transform.position, startPos) > 0.1f)
        {
            Vector3 dir = (startPos - transform.position).normalized;
            transform.position += dir * monster_speed * Time.deltaTime;

            // �¿� ����
            if (dir.x < 0)
                spriteRenderer.flipX = false;
            else if (dir.x > 0)
                spriteRenderer.flipX = true;

            yield return null;
        }

        transform.position = startPos;
        isChasing = false;
        isReturning = false;

        // �ٽ� ���� �簳
        SetAnimationState(true, false, false);
        if (usePatrol && !isDead)
            StartCoroutine(PatrolRoutine());
    }


    // ����
    protected IEnumerator MonsterAttackCoroutine(float AttackDelay, float StunDelay)
    {
        monster_attacking = true;
        stopAction = true;

        // ���� �� �̵� ����
        Vector3 originalPos = transform.position;

        switch (Id_enumType)
        {
            case monster_id.STRAYDOG:
                if (Random.value < 0.3f && !isStunned)
                {
                    yield return StartCoroutine(DogSpecialAttack(StunDelay));
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

        // ���� �� ��� ���
        yield return new WaitForSeconds(AttackDelay);
        monster_attacking = false;
        stopAction = false;
    }

    protected bool MonsterInAttackRange()
    {
        coll = Physics2D.OverlapCircleAll((Vector2)transform.position, monster_attack_range, layer);

        if (coll.Length > 0)
        {
            foreach (Collider2D col in coll)
            {
                if (col == null) continue;
                Short_player = col;
                return true;
            }
        }
        return false;
    }

    // ����
    protected void Attack()
    {
        PlaySE(sound_Attack);
        Debug.Log("�Ϲݰ���");
        animator.SetTrigger("Attack");
        playerStatus.TakeDamage(monster_damage);
    }

    // ����
    protected IEnumerator DogSpecialAttack(float StunDelay)
    {
        Debug.Log("10% Ȯ���� ����!");
        PlaySE(sound_Attack);
        gameManager.isGroggy = true;
        isStunned = true;
        // 0.5�� ���� 3�� ����
        for (int i = 0; i < 3; i++)
        {
            float damage = monster_damage * 0.5f;
            playerStatus.TakeDamage(monster_damage);
            Debug.Log($"Ư�� ���� {i + 1}ȸ��: {damage}");
            yield return new WaitForSeconds(StunDelay);
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
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet"))
        {
            Bullet bullet = collision.GetComponent<Bullet>();
            if (bullet != null)
            {
                Damage(bullet.damage);
            }

            // �Ѿ� �ı�
            Destroy(collision.gameObject);
        }
    }

    // ����� ��ũ��Ʈ
    protected virtual void Damage(int _dmg)
    {
        if (!isDead)
        {
            Debug.Log($"���� HP : {monster_curHp}");
            monster_curHp -= _dmg;
            Debug.Log($"�Ѿ� ������ : {_dmg}");
            Debug.Log($"���� HP : {monster_curHp}");

            if (monster_curHp <= 0)
            {
                Dead();
                return;
            }

            PlaySE(sound_Hurt);
            animator.SetTrigger("Hit");
        }
    }

    protected void SetAnimationState(bool idle, bool walk, bool run)
    {
        animator.SetBool("isIdle", idle);
        animator.SetBool("isWalk", walk);
        animator.SetBool("isRun", run);
    }

    protected void Dead()
    {
        animator.SetTrigger("Dead");
        PlaySE(sound_Dead);
        Destroy(this.gameObject);
        monster_attacking = false;
        isDead = true;
    }

    protected void RandomSound()
    {
        switch (Id_enumType)
        {
            case monster_id.STRAYDOG:
                int _random = Random.Range(0, 3); // �ϻ� ���� 2��.
                PlaySE(sound_Normal[_random]);
                break;
            case monster_id.CAT:
                int random = Random.Range(0, 2); // �ϻ� ���� 2��.
                PlaySE(sound_Normal[random]);
                break;
            case monster_id.RAT:
                PlaySE(sound_Normal[0]);
                break;
        }
    }

    protected void PlaySE(AudioClip _clip)
    {
        theAudio.clip = _clip;
        theAudio.Play();
    }
}