namespace DaysLeft.Menu
{
    using DaysLeft.Inventory;
    using DaysLeft.Item;
    using DeayLeft.Inventory;
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;

    public class InventoryUIController : UIController
    {
        public UIItemInfo       ItemInfoPopupHandler;
        public UICraftingInfo   CraftingInfoPopupHandler;

        public void ShowItemInfo(Vector3 position, ItemInstance itemInstance)
        {
            ItemInfoPopupHandler.ShowItemInfo(position, itemInstance);
        }

        public void HideItemInfo()
        {
            ItemInfoPopupHandler.Hide();
        }

        public void ShowCraftingInfo(Recipe recipe)
        {
            CraftingInfoPopupHandler.ShowItemInfo(recipe);
        }
    }
}