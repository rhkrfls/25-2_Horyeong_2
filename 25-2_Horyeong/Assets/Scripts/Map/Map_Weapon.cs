using UnityEngine;

public class Map_Weapon : MonoBehaviour
{
    public bool isUsed = false;
    public WeaponData usedWeaponData;

    public void SetWeaponData(WeaponData weaponData)
    {
        usedWeaponData = weaponData;
    }

    public WeaponData GetUsedWeapon()
    {
        return usedWeaponData;
    }
}
