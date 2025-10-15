using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;

public class Cat : Enemy
{
    [Header("# Cat Settings")]
    [SerializeField] private int catHp;                // 체력

    [Header("# Cat Speed")]
    [SerializeField] private float catSpeed;           // 추격 속도

    [Header("# Cat Range")]
    [SerializeField] private float catSightRange;      // 시야 범위
    [SerializeField] private float catAttackRange;     // 공격 범위

    [Header("# Cat Attack")]
    [SerializeField] private int catDamage;            // 공격력
    [SerializeField] private float catAttackDelay;     // 공격 간격

    [Header("# Cat Patrol")]
    [SerializeField] private float catPatrolRange;     // 순찰 범위
    [SerializeField] private float catPatrolSpeed;     // 순찰 속도

    protected override void Start()
    {
        base.Start();

        // 고양이 세팅 초기화
        monster_maxHp = catHp;

        monster_damage = catDamage;

        monster_speed = catSpeed;

        patrolSpeed = catPatrolSpeed;
        patrolRange = catPatrolRange;

        stun_delay = 0;

        monster_sight_range = catSightRange;
        monster_attack_range = catAttackRange;

        Id_enumType = monster_id.CAT;
        Attack_enumType = monster_attack_type.MELEE;
        Race_enumType = race.ANIMAL;
    }
    protected override void Update()
    {
        base.Update();

        // 고양이 세팅 초기화
        monster_maxHp = catHp;

        monster_damage = catDamage;

        monster_speed = catSpeed;

        stun_delay = 0;

        patrolSpeed = catPatrolSpeed;
        patrolRange = catPatrolRange;

        monster_sight_range = catSightRange;
        monster_attack_range = catAttackRange;

        Id_enumType = monster_id.CAT;
        Attack_enumType = monster_attack_type.MELEE;
        Race_enumType = race.ANIMAL;

        MonsterSightRange();
        if (MonsterView() && !monster_attacking)
        {
            stopAction = true;
            StartCoroutine(MonsterAttackCoroutine(catAttackDelay, 0));
        }

        if (!stopAction && !monster_attacking)
            StartCoroutine(Wait());
    }

    private IEnumerator Wait()
    {
        stopAction = true;
        RandomSound();
        yield return new WaitForSeconds(8f);
        stopAction = false;
    }
}
