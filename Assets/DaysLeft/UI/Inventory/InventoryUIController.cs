namespace DaysLeft.Menu
{
    using DaysLeft.Inventory;
    using UnityEngine;
    using UnityEngine.UIElements;

    public class InventoryUIController : UIController
    {
        public UIItemInfo       ItemInfoPopupHandler;
        public UICraftingInfo   CraftingInfoPopupHandler;

        [SerializeField]
        private UIScreen MainScreen;
        [SerializeField]
        private GameObject Background;
        
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
                Background.SetActive(true);
                Show<ViewUIInventory>();
                return true;
            }
            else
            {
                Background.SetActive(false);
                Hide();
                return false;
            }
        }
    }
}