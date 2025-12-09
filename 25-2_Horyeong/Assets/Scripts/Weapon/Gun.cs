using UnityEngine;

public class Gun : MonoBehaviour
{
    public WeaponData gunData;

    public PlayerController player;
    public GameObject bulletPrefab;      // 총알 프리팹을 Inspector에 드래그하여 연결
    public Transform firePoint;          // 총알이 나갈 위치 (Empty GameObject로 지정)

    private void Awake()
    {
        player = GetComponent<PlayerController>();
        firePoint = GameObject.Find("FirePoint").transform;
    }

    public void Attack()
    {
        if (player.currentData.currentPlayerCharachter != PLAYERNAME.YUSEONG) return;
        Debug.Log("Gun Attack!");   
        GameObject bulletGO = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        Bullet bullet = bulletGO.GetComponent<Bullet>();

        Vector2 shootDirection = player.spriteRenderer.flipX ? Vector2.left : Vector2.right;

        if (bullet != null)
        {
            bullet.Launch(shootDirection, gunData.damage);
        }
    }
}
