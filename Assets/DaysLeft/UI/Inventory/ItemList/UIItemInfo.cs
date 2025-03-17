using DaysLeft.Item;
using DaysLeft.Menu;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DeayLeft.Inventory
{
    public class UIItemInfo : UIScreen
    {
        [SerializeField]
        private TMP_Text _itemName;
        [SerializeField]
        private TMP_Text _itemDescription;
        [SerializeField]
        private TMP_Text _itemType;
        [SerializeField]
        private TMP_Text _itemStatus;
        [SerializeField]
        private Image _itemIcon;

        public void ShowItemInfo(Vector3 position, ItemInstance itemInstance)
        {
            _itemName.text = itemInstance.Name;
            _itemDescription.text = itemInstance.Description;
            _itemIcon.sprite = itemInstance.Icon;

            Show();
        }
    }
}