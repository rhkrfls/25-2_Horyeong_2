using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;

public class Dog : Enemy
{
    [Header("# Dog Settings")]
    [SerializeField] private int dogHp;                // 체력

    [Header("# Dog Speed")]
    [SerializeField] private float dogSpeed;           // 추격 속도

    [Header("# Dog Range")]
    [SerializeField] private float dogSightRange;      // 시야 범위
    [SerializeField] private float dogAttackRange;     // 공격 범위

    [Header("# Dog Attack")]
    [SerializeField] private int dogDamage;            // 공격력
    [SerializeField] private float dogAttackDelay;     // 공격 간격

    [Header("# Dog Stun")]
    [SerializeField] private float dogStunDelay;       // 스턴 간격

    [Header("# Dog Patrol")]
    [SerializeField] private float dogPatrolRange;     // 순찰 범위
    [SerializeField] private float dogPatrolSpeed;     // 순찰 속도

    protected override void Start()
    {
        base.Start();

        // 개 세팅 초기화
        monster_maxHp = dogHp;
        monster_curHp = monster_maxHp;

        attack_delay = dogAttackDelay;
        stun_delay = dogStunDelay;

        monster_damage = dogDamage;

        monster_speed = dogSpeed;
        patrolSpeed = dogPatrolSpeed;
        patrolRange = dogPatrolRange;

        monster_sight_range = dogSightRange;
        monster_attack_range = dogAttackRange;

        Id_enumType = monster_id.STRAYDOG;
        Attack_enumType = monster_attack_type.MELEE;
        Race_enumType = race.ANIMAL;
    }

    protected override void Update()
    {
        base.Update();


        MonsterSightRange();
        if (MonsterView() && !monster_attacking)
        {
            stopAction = true;
            StartCoroutine(MonsterAttackCoroutine(dogAttackDelay, dogStunDelay));
        }

        if(!stopAction && !monster_attacking)
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
