namespace DaysLeft.Item
{
    using UnityEngine;

    public class ItemInstance
    {
        protected ItemData _itemData;
        public ItemInstance(int id)
        {
            DataLoadManager dataLoad = Global.Instance.DataLoadManager;

            if (dataLoad == null)
                Debug.LogError("DataLoadManager is not there in Global");
            else
                _itemData = dataLoad.Query(id);
        }
        public ItemInstance(ItemData data)
        {
            if (data == null)
                Debug.LogError("Wrong data");
            else
                _itemData = data;
        }

        public int ID               => _itemData.ID;
        public string Name          => _itemData.Name;
        public string Description   => _itemData.Description;
        public ItemRarity Rarity    => _itemData.Rarity;
        public float Weight         => _itemData.weight;
        public bool Stackable       => _itemData.Stackable;
        public Sprite Icon          => _itemData.Icon;
        public GameObject Prefab    => _itemData.Prefab;

        public int MaxQuantity = 99;
        private int _quantity = 0;
        public int Quantity
        {
            get { return _quantity; }
            set
            {
                if (_quantity < MaxQuantity)
                {
                    _quantity = value;
                }
            }
        }
        public bool HasData
            => _itemData != null;
    }
}