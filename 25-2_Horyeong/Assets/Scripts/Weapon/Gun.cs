using UnityEngine;

public class Gun : Weapon
{
    public GameObject bulletPrefab;      // �Ѿ� �������� Inspector�� �巡���Ͽ� ����
    public Transform firePoint;          // �Ѿ��� ���� ��ġ (Empty GameObject�� ����)

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
