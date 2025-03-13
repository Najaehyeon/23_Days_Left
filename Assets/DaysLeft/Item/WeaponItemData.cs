namespace DaysLeft.Item
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    [CreateAssetMenu(fileName = "Item_Weapon", menuName = "ScriptableObject/ItemData/Weapon")]
    public class WeaponItemData : ItemData
    {
        [Header("Status")]
        public float AttackPower;
    }
}