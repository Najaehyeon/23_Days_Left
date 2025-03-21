namespace DaysLeft.Inventory
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

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
        public void AddNew(int itemID)
        {
            ItemInstance newItem ;

            if (itemID >= 0 && itemID < 1000)
            {
                newItem = new ItemInstance(itemID);
            }
            else if (itemID >= 1000 && itemID < 2000)
            {
                newItem = new WeaponInstance(itemID);
            }
            else if (itemID >= 2000 && itemID < 3000)
            {
                newItem = new ConsumeInstance(itemID);
            }
            else if (itemID >= 3000 && itemID < 4000)
            {
                newItem = new ArmorInstance(itemID);
            }
            else
                newItem = null;

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
                int index = CurEmptyIndex;
                _items[index] = newItem;
                _items[index].Quantity++;
                OnContentsChanged?.Invoke();
            }
        }

        public bool TrySubtract(int id, int consumeQuantity)
        {
            int remainQuantity = 0;
            List<ItemInstance> targetItems = QueryAll(id);

            foreach (var item in targetItems)
                remainQuantity += item.Quantity;

            if (remainQuantity < consumeQuantity)
                return false;

            while (targetItems[0].MaxQuantity < consumeQuantity)
            {
                consumeQuantity -= targetItems[0].MaxQuantity;
                targetItems[0] = null;
                targetItems.RemoveAt(0);
            }

            targetItems[0].Quantity -= consumeQuantity;
            if (targetItems[0].Quantity <= 0)
                targetItems[0] = null;

            OnContentsChanged?.Invoke();
            return true;
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
        public List<ItemInstance> QueryAll(int id)
        {
            List<ItemInstance> result = new();

            foreach (var item in _items)
                if (item?.ID == id)
                    result.Add(item);

            return result;
        }
    }
}