using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;

public class Dog : Enemy
{
    void Start()
    {
        
    }

    protected override void Update()
    {
        MonsterSightRange();
        if (MonsterView() && !monster_attacking)
        {
            StartCoroutine(MonsterAttackCoroutine());
        }
    }
}
