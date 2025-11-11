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
    protected int monster_damage;           // 공격 데미지

    public PlayerStatus playerStatus;


    [Header("# Monster_LayerMask")]
    [SerializeField]
    protected LayerMask layer;         // 타겟 마스크

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
    protected bool isAction;                        // 행동중인지 아닌지 판별
    protected bool monster_detected = false;        // 플레이어 감지
    [SerializeField]
    protected bool monster_attacking = false;       // 공격중인 판별
    protected bool isStunned = false;
    protected bool isChasing;                       // 추격중인지 판별
    protected bool isDead;                          // 죽었는지 판별
    protected bool stopAction = false;

    // Monster_Patrol
    protected bool usePatrol = true;        // 순찰 기능 켜기
    protected float patrolRange = 3f;       // 왕복 거리
    protected float patrolSpeed = 2f;       // 순찰 속도
    protected float patrolWaitTime = 2f;    // 끝점에서 대기 시간
    private Vector3 startPos;               // 시작 지점
    private bool movingRight = true;        // 이동 방향 판별
    private bool isWaiting = false;         // 대기 중인지
    private bool isReturning = false;       // 복귀 중인지

    protected float attack_delay;
    protected float stun_delay;

    protected Transform player;
    [SerializeField]
    protected GameManager gameManager;
    [SerializeField]
    protected Collider2D[] coll;
    [SerializeField]
    protected Collider2D Short_player;
    [SerializeField]
    protected SpriteRenderer spriteRenderer;

    /*[SerializeField]
    private Item item_Prefab;             // 아이템*/
    [SerializeField]
    protected int itemNumber;             // 아이템 획득 개수

    // 필요한 컴포넌트
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
            playerStatus = FindAnyObjectByType<PlayerStatus>();

            if (playerObj != null)
            {
                playerStatus = playerStatus.GetComponent<PlayerStatus>();
                Short_player = player.GetComponent<CapsuleCollider2D>();

                if (playerStatus == null)
                    Debug.LogWarning("Player 오브젝트에 PlayerStatus 컴포넌트가 없습니다!");
            }
            else
            {
                Debug.LogWarning("'Player' 태그가 지정된 오브젝트를 찾을 수 없습니다!");
            }
        }

        // GameManager 자동 연결
        GameObject gamemanager = GameObject.FindGameObjectWithTag("GameManager");

        if (gameManager == null)
        {
            gameManager = gamemanager.GetComponent<GameManager>();
            if (gameManager == null)
                Debug.LogWarning("씬에서 Gamemanager를 찾을 수 없습니다!");
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
            // 공격 범위 안이면 추격 중지, 제자리에서 공격
            if (inAttackRange)
            {
                if (isChasing)
                {
                    isChasing = false;
                    SetAnimationState(true, false, false); // run → idle 전환
                }

                // 공격
                if (!monster_attacking)
                {
                    StartCoroutine(MonsterAttackCoroutine(attack_delay, stun_delay));
                }
            }
            else
            {
                // 공격 범위 밖이면 추격
                isChasing = true;
                SetAnimationState(false, false, true); // Idle에서 run으로
                ChasePlayer();
            }
        }
        else if (isChasing)
        {
            // 플레이어 놓쳤을 때 원래 자리로 복귀
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

    // 순찰 함수
    protected IEnumerator PatrolRoutine()
    {
        startPos = transform.position; // 시작 위치

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

                // 이동
                float moveDir = movingRight ? 1f : -1f;
                transform.Translate(Vector2.right * moveDir * patrolSpeed * Time.deltaTime);

                // 끝점 체크
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

                // 이미지 플립
                spriteRenderer.flipX = movingRight;
            }
            else
            {
                // 대기 중에는 Idle 애니메이션
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


    // 추격 함수
    protected void ChasePlayer()
    {
        if (Short_player == null) return;

        SetAnimationState(false, false, true);

        Vector3 direction = Short_player.transform.position - transform.position;
        direction.Normalize();

        transform.position += new Vector3(direction.x, 0, 0) * monster_speed * Time.deltaTime;

        // 좌우 반전
        spriteRenderer.flipX = direction.x > 0; // 오른쪽이면 true, 왼쪽이면 false

    }

    // 복귀 함수
    protected IEnumerator ReturnToStart()
    {
        if (isReturning) yield break; // 중복 방지
        isReturning = true;

        SetAnimationState(false, true, false);

        while (Vector3.Distance(transform.position, startPos) > 0.1f)
        {
            Vector3 dir = (startPos - transform.position).normalized;
            transform.position += dir * monster_speed * Time.deltaTime;

            // 좌우 반전
            if (dir.x < 0)
                spriteRenderer.flipX = false;
            else if (dir.x > 0)
                spriteRenderer.flipX = true;

            yield return null;
        }

        transform.position = startPos;
        isChasing = false;
        isReturning = false;

        // 다시 순찰 재개
        SetAnimationState(true, false, false);
        if (usePatrol && !isDead)
            StartCoroutine(PatrolRoutine());
    }


    // 공격
    protected IEnumerator MonsterAttackCoroutine(float AttackDelay, float StunDelay)
    {
        monster_attacking = true;
        stopAction = true;

        // 공격 중 이동 정지
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

        // 공격 후 잠깐 대기
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

    // 공격
    protected void Attack()
    {
        PlaySE(sound_Attack);
        Debug.Log("일반공격");
        animator.SetTrigger("Attack");
        playerStatus.TakeDamage(monster_damage, this.transform);
    }

    // 스턴
    protected IEnumerator DogSpecialAttack(float StunDelay)
    {
        Debug.Log("10% 확률로 실행!");
        PlaySE(sound_Attack);
        animator.SetTrigger("Skill");
        gameManager.isGroggy = true;
        isStunned = true;
        // 0.5배 공격 3번 실행
        for (int i = 0; i < 3; i++)
        {
            float damage = monster_damage * 0.5f;
            playerStatus.TakeDamage(monster_damage, this.transform);
            Debug.Log($"특수 공격 {i + 1}회차: {damage}");
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

            // 총알 파괴
            Destroy(collision.gameObject);
        }
    }

    // 비공격 스크립트
    protected virtual void Damage(int _dmg)
    {
        if (!isDead)
        {
            Debug.Log($"몬스터 HP : {monster_curHp}");
            monster_curHp -= _dmg;
            Debug.Log($"총알 데미지 : {_dmg}");
            Debug.Log($"몬스터 HP : {monster_curHp}");

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
                int _random = Random.Range(0, 3); // 일상 사운드 2개.
                PlaySE(sound_Normal[_random]);
                break;
            case monster_id.CAT:
                int random = Random.Range(0, 2); // 일상 사운드 2개.
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