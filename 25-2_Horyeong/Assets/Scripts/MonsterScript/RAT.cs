using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;

public class RAT : Enemy
{
    [SerializeField] private int contactDamage = 3; // 닿았을 때 입히는 피해량

    protected override void Start()
    {
        base.Start();

        Id_enumType = monster_id.RAT;
        Attack_enumType = monster_attack_type.MELEE;
        Race_enumType = race.ANIMAL;

        // 쥐는 순찰만 함
        usePatrol = true;

        // 쥐는 플레이어 탐지, 추격, 공격 안함
        monster_sight_range = 0f;
        monster_attack_range = 0f;
    }

    protected override void Update()
    {
        if (isDead || isStunned) return;

        if (!stopAction)
            StartCoroutine(Wait());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerStatus.TakeDamage(5);
            PlaySE(sound_Attack);
        }
    }

    private IEnumerator Wait()
    {
        stopAction = true;
        RandomSound();
        yield return new WaitForSeconds(5f);
        stopAction = false;
    }
}
