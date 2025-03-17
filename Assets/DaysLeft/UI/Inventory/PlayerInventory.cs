namespace DaysLeft.Inventory
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;
    using DaysLeft.Item;

    public class PlayerInventory
    {
        [Header("Limit")]
        public int      MaxCapacity = 25;
        public float    MaxWeight = 100;

        private ItemInstance[] _items;

        /// <summary>
        /// Invoked when the inventory contents change.
        /// </summary>
        public Action OnContentsChanged;
        public PlayerInventory()
        {
            _items = new ItemInstance[MaxCapacity];
        }

        public int CurCapacity
        {
            get 
            {
                int _capacity = 0;
                foreach (var item in _items)
                {
                    if (item != null)
                        _capacity++;
                }
                return _capacity;
            }
        }

        public float CurWeight
        {
            get
            {
                float _weight = 0;
                foreach (var item in _items)
                {
                    if (item != null)
                        _weight += item.Weight;
                }
                return _weight;
            }
        }

        public int CurEmptyIndex
        {
            get
            {
                for(int i = 0; i < _items.Length; ++i)
                {
                    if (_items[i] == null)
                        return i;
                }

                return -1;
            }
        }

        /// <summary>
        /// Add an item to inventory
        /// </summary>
        /// <param name="newItem">ItemInstance</param>
        public void Add(ItemInstance newItem)
        {
            if (!newItem.HasData)
                Debug.LogError("Try add new item that have no data.");

            foreach (var item in _items)
            {
                if (item == null)
                    continue;
                if (item.ID == newItem.ID)
                {
                    if (item.Stackable)
                    {
                        item.Quantity++;
                        OnContentsChanged?.Invoke();
                        return;
                    }
                }
            }

            if(CurCapacity < MaxCapacity && newItem.Weight <= MaxWeight - CurWeight)
            {
                _items[CurEmptyIndex] = newItem;
                OnContentsChanged?.Invoke();
            }
        }

        /// <summary>
        /// Get an item by index in inventory
        /// </summary>
        /// <param name="index">index of an item in inventory</param>
        /// <returns></returns>
        public ItemInstance Get(int index)
            => _items[index];

        /// <summary>
        /// Query all items by id
        /// </summary>
        /// <param name="id"> id of itemData</param>
        public List<ItemInstance> Query(int id)
            => (from item in _items where item.ID == id select item).ToList();
    }
}