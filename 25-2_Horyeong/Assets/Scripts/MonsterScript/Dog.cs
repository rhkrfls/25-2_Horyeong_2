using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;

public class Dog : Enemy
{
    [Header("# Dog Settings")]
    [SerializeField] private int dogHp;                // ü��

    [Header("# Dog Speed")]
    [SerializeField] private float dogSpeed;           // �߰� �ӵ�

    [Header("# Dog Range")]
    [SerializeField] private float dogSightRange;      // �þ� ����
    [SerializeField] private float dogAttackRange;     // ���� ����

    [Header("# Dog Attack")]
    [SerializeField] private int dogDamage;            // ���ݷ�
    [SerializeField] private float dogAttackDelay;     // ���� ����

    [Header("# Dog Stun")]
    [SerializeField] private float dogStunDelay;       // ���� ����

    [Header("# Dog Patrol")]
    [SerializeField] private float dogPatrolRange;     // ���� ����
    [SerializeField] private float dogPatrolSpeed;     // ���� �ӵ�

    protected override void Start()
    {
        base.Start();

        // �� ���� �ʱ�ȭ
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
