namespace DaysLeft.Item
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public enum ItemRarity
    {
        Common, Uncommon, Legendary
    }

    public class ItemData : ScriptableObject
    {
        [Header("Identify")]
        public int              ID;
        public string           Name;
        public string           Description;

        [Header("Element")]
        public ItemRarity       Rarity;
        public float            weight;

        [Header("Economy")]
        public float            Price;
        public float            SellPrice;
        public float            Currency;

        [Header("Setting")]
        public bool             Stackable;
        public Sprite           Icon;
        public GameObject       Prefab;
    }

}
