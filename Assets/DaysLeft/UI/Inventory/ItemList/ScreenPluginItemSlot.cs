namespace DaysLeft.Inventory
{
    using TMPro;
    using DaysLeft.Item;
    using UnityEngine;
    using UnityEngine.UI;
    using DaysLeft.Menu;
    using UnityEngine.EventSystems;

    public class ScreenPluginItemSlot : ScreenPlugin, IPointerEnterHandler, IPointerExitHandler
    {
        [Header("Components")]
        public TMP_Text         ItemQuantity;
        public Image            ItemIcon;

        public ItemInstance data;
        private InventoryUIController _controller;
        private bool _onMouse = false;
        private Vector3 _onMousePoison = Vector3.zero;

        public void Set()
        {
            if (data.Stackable)
                ItemQuantity.text = $"{data.Quantity}";
            else
                ItemQuantity.text = "";
            ItemIcon.sprite = data.Icon;
        }

        public void Clear()
        {
            ItemQuantity.text = null;
            ItemIcon.sprite  = null;
        }

        public override void Show(UIScreen screen)
        {
            base.Show(screen);

            _controller = (InventoryUIController)screen.Controller;
        }

        public override void Hide(UIScreen screen)
        {
            base.Hide(screen);

            _controller = null;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (data != null)
                _controller.ShowItemInfo(eventData.position, data);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (data != null)
                _controller.HideItemInfo();
        }
    }
}
