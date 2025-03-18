namespace DaysLeft.Item
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;

    [CreateAssetMenu(fileName = "Item_Weapon", menuName = "ScriptableObject/ItemData/Weapon")]
    public class WeaponItemData : ItemData
    {
        [Header("Info")]
        public string weaponName;
        public string weaponDescription;
        public Image weaponIcon;

        [Header("Damage")]
        public float attackDamage;
        public float digWoodDamage;
        public float mineOreDamage; // (Ore = 광석)

        [Header("Status")]
        public float durability; // 총 내구도 (durability = 내구도)
        public float currentDurability; // 현재 내구도
    }
}
