using UnityEngine;

public class WeaponInstance : ItemInstance
{
    public float currentDurability; // 현재 내구도 

    private WeaponItemData _weaponItemData;
    public WeaponInstance(int id) : base(id)
    {
        DataLoadManager dataLoad = Global.Instance.DataLoadManager;

        if (dataLoad == null)
            Debug.LogError("DataLoadManager is not there in Global");
        else
            _weaponItemData = dataLoad.Query(id) as WeaponItemData;
    }

    public WeaponInstance(WeaponInstance instance) : base(instance)
    { 
        currentDurability = instance.currentDurability;
        _weaponItemData = instance._weaponItemData;
    }

    public EquippedWeaponType WeaponType => _weaponItemData.type;
    public float AttackDamage   => _weaponItemData.attackDamage;
    public float DigWoodDamage  => _weaponItemData.digWoodDamage;
    public float MineOreDamage  => _weaponItemData.mineOreDamage; // (Ore = 광석)
    public float durability     => _weaponItemData.durability; // 총 내구도 (durability = 내구도)
}
