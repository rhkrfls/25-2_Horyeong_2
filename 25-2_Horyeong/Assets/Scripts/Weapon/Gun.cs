using UnityEngine;

public class Gun : Weapon
{
    private void Awake()
    {
        weaponType = WEAPONTYPE.GUN;
        damage = 5;
        skillDamage = 5;
        durability = 100;
        isBringing = true;
        isSkill = false;
    }

    private void Update()
    {
        
    }

    private void Attack()
    {
        if (player.PN != PLAYERNAME.YUSEONG) return;
    }
}
