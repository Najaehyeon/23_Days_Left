namespace DaysLeft.Menu
{
    using DaysLeft.Inventory;
    using UnityEngine;

    public class InventoryUIController : UIController
    {
        public UIItemInfo       ItemInfoPopupHandler;
        public UICraftingInfo   CraftingInfoPopupHandler;

        [SerializeField]
        private UIScreen MainScreen;
        
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

        public bool Toggle()
        {
            if(!MainScreen.gameObject.activeSelf)
            {
                Show<ViewUIInventory>();
                return true;
            }
            else
            {
                Hide();
                return false;
            }
        }
    }
}