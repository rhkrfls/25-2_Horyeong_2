using UnityEngine;

public class Gun : Weapon
{
    public GameObject bulletPrefab;      // 총알 프리팹을 Inspector에 드래그하여 연결
    public Transform firePoint;          // 총알이 나갈 위치 (Empty GameObject로 지정)

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

    public void Attack()
    {
        if (player.PN != PLAYERNAME.YUSEONG) return;

        GameObject bulletGO = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        Bullet bullet = bulletGO.GetComponent<Bullet>();

        Vector2 shootDirection = player.spriteRenderer.flipX ? Vector2.left : Vector2.right;

        if (bullet != null)
        {
            bullet.Launch(shootDirection);
        }
    }
}
