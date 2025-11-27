using JetBrains.Annotations;
using UnityEngine;

public class WoodStick : Weapon 
{
    private void Awake()
    {
        player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        weaponType = WEAPONTYPE.WOODENSTICK;
        damage = 5;
        durability = 15;
        isBringing = false;
        isSkill = false;
    }

    public void Attack()
    {
        player.animator.SetTrigger("isAttackWoodStick");
        //애니메이션 재생 > 히트박스 활성화 > 히트박스에 트리거 있을 시 데미지 주기
    }
}
