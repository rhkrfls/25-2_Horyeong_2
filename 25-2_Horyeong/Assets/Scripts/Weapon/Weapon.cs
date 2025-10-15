using UnityEngine;

public enum WEAPONTYPE
{
    WOODENSTICK, FIFE, GUN
}
public class Weapon : MonoBehaviour
{
    [Header("무기")]
    public PlayerController player;
    public WEAPONTYPE   weaponType;
    public int          damage;
    public int          skillDamage;
    public int          durability;
    public bool         isBringing = false;
    public bool         isSkill = false;
    public BoxCollider2D weaponColl;

    private void Awake()
    {
        if (weaponType == WEAPONTYPE.WOODENSTICK)
        {
            damage = 5;
            durability = 15;
        }

        else if (weaponType == WEAPONTYPE.FIFE)
        {
            damage = 10;
            durability = 25;
            isSkill = true;
        }

        weaponColl = GetComponent<BoxCollider2D>();
    }

    public virtual int takeDamage()
    {
        durability -= 1;
        return damage;
    }

    public virtual int takeSkill()
    {
        if (!isSkill) return 0;
        //skill 내용 정리

        return 1;
    }
}
