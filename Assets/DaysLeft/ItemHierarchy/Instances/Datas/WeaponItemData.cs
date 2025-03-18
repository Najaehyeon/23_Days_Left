using UnityEngine;

[CreateAssetMenu(fileName = "Item_Weapon", menuName = "ScriptableObject/ItemData/Weapon")]
public class WeaponItemData : ItemData
{
    [Header("Type")]
    public EquippedWeaponType type;

    [Header("Damage")]
    public float attackDamage;
    public float digWoodDamage;
    public float mineOreDamage; // (Ore = 광석)

    [Header("Status")]
    public float durability; // 총 내구도 (durability = 내구도)
}