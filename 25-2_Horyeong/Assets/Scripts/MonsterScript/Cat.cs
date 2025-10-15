using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;

public class Cat : Enemy
{
    [Header("# Cat Settings")]
    [SerializeField] private int catHp;                // ü��

    [Header("# Cat Speed")]
    [SerializeField] private float catSpeed;           // �߰� �ӵ�

    [Header("# Cat Range")]
    [SerializeField] private float catSightRange;      // �þ� ����
    [SerializeField] private float catAttackRange;     // ���� ����

    [Header("# Cat Attack")]
    [SerializeField] private int catDamage;            // ���ݷ�
    [SerializeField] private float catAttackDelay;     // ���� ����

    [Header("# Cat Patrol")]
    [SerializeField] private float catPatrolRange;     // ���� ����
    [SerializeField] private float catPatrolSpeed;     // ���� �ӵ�

    protected override void Start()
    {
        base.Start();

        // ����� ���� �ʱ�ȭ
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

        // ����� ���� �ʱ�ȭ
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
