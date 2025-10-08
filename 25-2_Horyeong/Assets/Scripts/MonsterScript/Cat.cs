using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;

public class Cat : Enemy
{
    [Header("# Monster_AttackDelay")]
    [SerializeField]
    protected float attackDelay_Cat;        // 냥이 공격 딜레이

    void Start()
    {
        
    }

    protected override void Update()
    {
        MonsterSightRange();
        if (MonsterView() && !monster_attacking)
        {
            StartCoroutine(MonsterAttackCoroutine(attackDelay_Cat, 0));
        }
    }
}
