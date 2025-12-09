using UnityEngine;

[CreateAssetMenu(fileName = "WeaponData", menuName = "Scriptable Objects/WeaponData")]
public class WeaponData : ScriptableObject
{
    public WEAPONTYPE weaponType;
    public int damage;
    public int skillDamage;
    public int durability;
    public bool isBringing = false;
    public bool isSkill = false;
}
