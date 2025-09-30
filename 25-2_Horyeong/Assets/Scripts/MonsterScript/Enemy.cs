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
    private int monster_damage;           // 공격 데미지
    [SerializeField]
    private float attackDelay;            // 공격 딜레이
    [SerializeField]
    private float attackDistance;         // 사정거리
    [SerializeField]
    public float stunDuration = 2f;       // 제압 지속 시간

    [Header("# Monster_LayerMask")]
    public LayerMask layer;         // 타겟 마스크

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
    private Item item_Prefab;             // 아이템*/
    [SerializeField]
    public int itemNumber;             // 아이템 획득 개수

    // 상태변수
    private bool isChasing;             // 추격중인지 판별
    private bool isAttacking;           // 공격중인 판별
    private bool isDead;                   // 죽었는지 판별

    // 필요한 컴포넌트
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
                        if (Random.value < 0.1f) // Random.value는 0.0 ~ 1.0 사이의 float
                        {
                            StartCoroutine(DogSpecialAttack(/*player*/));
                            Debug.Log("10% 확률로 실행!");
                        }
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

    // 행동제약
    public void Stun(float duration)        // 플레이어 쪽에서 stun구현후 삭제
    {
        if (!isStunned)
            StartCoroutine(StunRoutine(duration));
    }

    private IEnumerator StunRoutine(float duration)     // 플레이어 쪽에서 stun구현후 삭제
    {
        isStunned = true;
        Debug.Log("플레이어 제압 상태 (이동 불가, 공격 가능)");
        yield return new WaitForSeconds(duration);
        isStunned = false;
        monster_attacking = false;
        Debug.Log("제압 해제");
    }

    private IEnumerator DogSpecialAttack(/*Player player*/)
    {
        float damage = monster_damage * 0.5f;

        // 먼저 플레이어 제압
        Stun(stunDuration);
        // player.Stun(stunDuration);

        // 0.5배 공격 3번 실행
        for (int i = 0; i < 3; i++)
        {
            Debug.Log($"특수 공격 {i + 1}회차: {damage}");
            // player.TakeDamage(damage);

            yield return new WaitForSeconds(attackDelay);
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
    // 비공격 스크립트

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
        int _random = Random.Range(0, 3); // 일상 사운드 3개.
        PlaySE(sound_Normal[_random]);
    }

    protected void PlaySE(AudioClip _clip)
    {
        theAudio.clip = _clip;
        theAudio.Play();
    }
}
