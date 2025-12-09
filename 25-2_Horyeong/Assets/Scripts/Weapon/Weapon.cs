using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;
public enum WEAPONTYPE
{
    NONE, WOODSTICK, PIPE, GUN
}
public class Weapon : MonoBehaviour
{
    [Header("무기")]
    [SerializeField]
    private WeaponData currentWeaponData;

    [SerializeField]
    private WeaponData woodStickData;
    [SerializeField]
    private WeaponData pipeData;

    public PlayerController player;
    [SerializeField]
    private GameObject weapon_hitbox1;
    [SerializeField]
    private GameObject weapon_hitbox2;

    //무기를 가진 상태에서 다른 무기를 주울 경우 본래 무기를 떨어뜨리기 위한 프리팹
    public GameObject woodStickPrefab;
    public GameObject pipePrefab;

    public bool isEquipped = false;
    public bool isAttacked = false;
    public bool isSecondAttack = false;
    public float secondAttackTime = 0.0f;

    private void Awake()
    {
        player = GetComponent<PlayerController>();
        weapon_hitbox1 = GameObject.Find("WeaponHitBox1");
        weapon_hitbox2 = GameObject.Find("WeaponHitBox2");

        weapon_hitbox1.SetActive(false);
        weapon_hitbox2.SetActive(false);
    }

    private void Start()
    {
        BreakWeapon();
    }

    private void Update()
    {
        if (isAttacked)
        {
            secondAttackTime += Time.deltaTime;

            if (secondAttackTime > 5.0f)
            {
                isAttacked = false;
            }
        }

        else
        {
            secondAttackTime = 0.0f;
        }
    }

    public void GetWeapon(WeaponData data)
    {
        if (isEquipped) dropWeapon();

        currentWeaponData = data;
        isEquipped = true;
    }

    public void GetWeapon(WEAPONTYPE type)
    {
        if (isEquipped) dropWeapon();

        if (type == WEAPONTYPE.WOODSTICK)
        {
            currentWeaponData.weaponType    = woodStickData.weaponType;
            currentWeaponData.damage        = woodStickData.damage;
            currentWeaponData.durability    = woodStickData.durability;
            currentWeaponData.isBringing    = woodStickData.isBringing;
            currentWeaponData.isSkill       = woodStickData.isSkill;
            isEquipped = true;
        }

        else if (type == WEAPONTYPE.PIPE)
        {
            currentWeaponData.weaponType    = pipeData.weaponType;
            currentWeaponData.damage        = pipeData.damage;
            currentWeaponData.durability    = pipeData.durability;
            currentWeaponData.isBringing    = pipeData.isBringing;
            currentWeaponData.isSkill       = pipeData.isSkill;
            isEquipped = true;
        }
    }   
    
    public void dropWeapon()
    {
        if (currentWeaponData.weaponType == WEAPONTYPE.WOODSTICK)
        {
            GameObject dropWoodStick = Instantiate(woodStickPrefab, player.transform.position, Quaternion.identity);
            dropWoodStick.GetComponent<Map_Weapon>().SetWeaponData(currentWeaponData);
            dropWoodStick.GetComponent<Map_Weapon>().isUsed = true;
        }

        else if (currentWeaponData.weaponType == WEAPONTYPE.PIPE)
        {
            GameObject dropPipe = Instantiate(pipePrefab, player.transform.position, Quaternion.identity);
            dropPipe.GetComponent<Map_Weapon>().SetWeaponData(currentWeaponData);
            dropPipe.GetComponent<Map_Weapon>().isUsed = true;
        }

        isEquipped = false;
    }

    public virtual void Attack()
    {
        if (currentWeaponData.weaponType == WEAPONTYPE.NONE) return;
        if (isSecondAttack) return;

        if (isAttacked)
        {
            isSecondAttack = true;
            player.animator.SetTrigger("isAttackSecond");
        }

        else
        {
            if (currentWeaponData.weaponType == WEAPONTYPE.WOODSTICK)
            {
                isAttacked = true;
                player.animator.SetTrigger("isAttackWood");
            }

            else if (currentWeaponData.weaponType == WEAPONTYPE.PIPE)
            {
                isAttacked = true;
                player.animator.SetTrigger("isAttackPipe");
            }
        }

        currentWeaponData.durability -= 1;
        Debug.Log($"무기 내구도: {currentWeaponData.durability}");

        if (currentWeaponData.durability <= 0)
        {
            BreakWeapon();
        }
    }

    public void firstAttackStart()
    {
        weapon_hitbox1.SetActive(true);
    }

    public void firstAttackEnd()
    {
        weapon_hitbox1.SetActive(false);
        player.SetisAttacking();
    }

    public void secondAttackStart()
    {
        weapon_hitbox2.SetActive(true);
    }

    public void secondAttackEnd()
    {
        weapon_hitbox2.SetActive(false);
        player.SetisAttacking();
        isSecondAttack = false;
    }

    public int takeDamage()
    {
        return currentWeaponData.damage;
    }
    public int takeSkill()
    {
        if (!currentWeaponData.isSkill) return 0;
        //skill 내용 정리

        return 1;  
    }

    public  void BreakWeapon()
    {
        currentWeaponData.weaponType = WEAPONTYPE.NONE;
        currentWeaponData.damage = 0;
        currentWeaponData.durability = 0;
        currentWeaponData.isBringing = false;
        currentWeaponData.isSkill = false;
        isEquipped = false;
    }
}
