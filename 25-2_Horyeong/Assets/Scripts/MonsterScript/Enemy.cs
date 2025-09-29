using System.Data.Common;
using UnityEngine;
using static UnityEditor.Progress;
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
    protected int monster_damage;           // 공격 데미지
    [SerializeField]
    protected float attackDelay;            // 공격 딜레이
    [SerializeField]
    protected float attackDistance;         // 사정거리

    [Header("# Monster_LayerMask")]
    public LayerMask layer;         // 타겟 마스크

    [Header("# Sound")]
    [SerializeField]
    protected AudioClip[] sound_Normal;
    [SerializeField]
    protected AudioClip sound_Hurt;
    [SerializeField]
    protected AudioClip sound_Dead;

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
    public bool monster_detected = false;
    public bool monster_attacking = false;
    public bool monster_chasing = false;

    private Transform player;

    public Collider2D[] coll;
    public Collider2D Short_player;
    private SpriteRenderer spriteRenderer;

    [Header("# Monster_NonAttack")]
    [SerializeField]
    protected float ChaseTime;          // 추격 시간
    protected float currentChaseTime;   // 계산
    [SerializeField]
    protected float ChaseDelayTime;     // 추격 딜레이

    [SerializeField]
    public string animalName;             // 동물의 이름
    [SerializeField]
    protected int hp;                     // 동물의 hp

    [SerializeField]
    private Item item_Prefab;             // 아이템
    [SerializeField]
    public int itemNumber;             // 아이템 획득 개수

    [SerializeField]
    protected float walkSpeed;            // 걷기 스피드
    [SerializeField]
    protected float runSpeed;             // 뛰기 스피드

    protected Vector3 destination;        // 방향

    // 상태변수
    protected bool isAction;              // 행동중인지 아닌지 판별
    protected bool isWalking;             // 걷는지 안 걷는지 판별
    protected bool isRunning;             // 뛰는지 안 뛰는지 판별
    protected bool isChasing;             // 추격중인지 판별
    protected bool isAttacking;           // 공격중인 판별
    protected bool isSitting;             // 앉는 중인지 판별
    public bool isDead;                   // 죽었는지 판별

    [SerializeField]
    protected float walkTime;             // 걷기 시간
    [SerializeField]
    protected float waitTime;             // 대기 시간
    [SerializeField]
    protected float runTime;              // 뛰기 시간
    protected float currentTIme;

    // 필요한 컴포넌트
    [SerializeField]
    protected Animator animator;
    [SerializeField]
    protected Rigidbody rigid;
    [SerializeField]
    protected BoxCollider boxCol;
    protected AudioSource theAudio;
    protected NavMeshAgent nav;

    public bool isStunned { get; private set; } = false;

    private void Start()
    {
        theAudio = GetComponent<AudioSource>();
        currentTIme = waitTime;
        isAction = true;

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
            Move();
            ElapseTime();
        }

        MonsterSightRange();
        MonsterAttack();

        switch(Race_enumType)
        {
            case race.ANIMAL:
                
                break;
            case race.PLANT:

                break;
            case race.HUMAN:

                break;
        }
        switch(Attack_enumType)
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

    private void Rat()
    {
        Debug.Log("rat");
    }

    private void Cat()
    {
        Debug.Log("cat");
    }

    private void Dog()
    {


        Debug.Log("dog");
    }

    private void Crow()
    {
        Debug.Log("crow");
    }

    private void Tree()
    {
        Debug.Log("tree");
    }

    private void Grass()
    {
        Debug.Log("grass");
    }

    public void Stun(float duration)
    {
        if (!isStunned)
            StartCoroutine(StunRoutine(duration));
    }

    private System.Collections.IEnumerator StunRoutine(float duration)
    {
        isStunned = true;
        Debug.Log("플레이어 제압 상태 (이동 불가, 공격 가능)");
        yield return new WaitForSeconds(duration);
        isStunned = false;
        Debug.Log("제압 해제");
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
                // 방향 구하기
                Vector3 direction = Short_player.transform.position - transform.position;
                direction.Normalize();

                // 이동 (X축만 이동)
                transform.position += new Vector3(direction.x, 0, 0) * monster_speed * Time.deltaTime;

                // 좌우 반전 (플레이어 위치에 따라 flip)
                if (direction.x < 0)
                    spriteRenderer.flipX = false; // 왼쪽 바라봄
                else if (direction.x > 0)
                    spriteRenderer.flipX = true;  // 오른쪽 바라봄
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


    // 공격 스크립트
    protected IEnumerator ChaseTargetCoroutine()
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
                // 방향 구하기
                Vector3 direction = Short_player.transform.position - transform.position;
                direction.Normalize();

                // 이동 (X축만 이동)
                transform.position += new Vector3(direction.x, 0, 0) * monster_speed * Time.deltaTime;

                // 좌우 반전 (플레이어 위치에 따라 flip)
                if (direction.x < 0)
                    spriteRenderer.flipX = false; // 왼쪽 바라봄
                else if (direction.x > 0)
                    spriteRenderer.flipX = true;  // 오른쪽 바라봄
            }
        }
        yield return null;
    }

    protected IEnumerator AttackCoroutine()
    {
        isAttacking = true;
        nav.ResetPath();
        currentChaseTime = ChaseTime;   // 공격 중에 다시 추격하는걸 막기 .IEnumerator ChaseTargetCoroutine()이 도는것을 막기
        yield return new WaitForSeconds(0.5f);
        yield return new WaitForSeconds(0.5f);
        RaycastHit _hit;
        if (Physics.Raycast(transform.position + Vector3.up, transform.forward, out _hit, attackDistance, targetMask))
        {
            animator.SetTrigger("Attack");
            //GameManager.damage = true;
        }

        yield return new WaitForSeconds(attackDelay);
        isAttacking = false;

        StartCoroutine(ChaseTargetCoroutine());     // 공격이 끝나면 다시 추적시도
    }

    // 비공격 스크립트
    protected void Move()
    {
        if (isWalking || isRunning)
            nav.SetDestination(transform.position + destination * 6f);
    }

    protected void ElapseTime()
    {
        if (isAction)
        {
            currentTIme -= Time.deltaTime;
            if (currentTIme <= 0 && !isChasing && !isAttacking)
                ReSet();
        }
    }

    protected virtual void ReSet()
    {
        isSitting = false;
        isWalking = false;
        isAction = true;
        isRunning = false;
        nav.speed = walkSpeed;
        nav.ResetPath();
        animator.SetBool("Sit", isSitting);
        animator.SetBool("Walking", isWalking);
        animator.SetBool("Running", isRunning);
        destination.Set(Random.Range(-5f, 5f), 0f, Random.Range(-5f, 5f));
    }

    protected void TryWalk()
    {
        isSitting = false;
        isWalking = true;
        animator.SetBool("Sit", isSitting);
        animator.SetBool("Walking", isWalking);
        currentTIme = walkTime;
        nav.speed = walkSpeed;
    }

    public virtual void Damage(int _dmg, Vector3 _targetPos)
    {
        if (!isDead)
        {
            hp -= _dmg;

            if (hp <= 0)
            {
                Dead();
                return;
            }

            PlaySE(sound_Hurt);
            animator.SetTrigger("Hit");
        }
    }

    protected void Dead()
    {
        PlaySE(sound_Dead);
        isSitting = false;
        isWalking = false;
        isRunning = false;
        isChasing = false;
        isAttacking = false;
        isDead = true;
        nav.ResetPath();
        animator.SetTrigger("Dead");
    }

    protected void RandomSound()
    {
        int _random = Random.Range(0, 3); // 일상 사운드 3개.
        PlaySE(sound_Normal[_random]);
    }

    protected void PlaySE(AudioClip _clip)
    {
        theAudio.clip = _clip;
        theAudio.Play();
    }

}
