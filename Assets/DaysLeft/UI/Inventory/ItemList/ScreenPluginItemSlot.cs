namespace DaysLeft.Inventory
{
    using TMPro;
    using DaysLeft.Item;
    using UnityEngine;
    using UnityEngine.UI;
    using DaysLeft.Menu;
    using UnityEngine.EventSystems;

    public class ScreenPluginItemSlot : ScreenPlugin, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [Header("Components")]
        [SerializeField]
        private TMP_Text    ItemQuantity;
        [SerializeField]
        private Image       ItemIcon;
        [SerializeField]
        private Transform   ItemTransform;
        [SerializeField]
        private Canvas      ItemIconCanvas;

        private InventoryUIController _controller;

        private ItemInstance _data;

        public void Set(ItemInstance data)
        {
            _data = data;

            if (_data.Stackable)
                ItemQuantity.text = $"{_data.Quantity}";
            else
                ItemQuantity.text = "";
            ItemIcon.sprite = _data.Icon;
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
            if (_data != null)
                _controller.ShowItemInfo(eventData.position, _data);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (_data != null)
                _controller.HideItemInfo();
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
        }

        public void OnDrag(PointerEventData eventData)
        {
            ItemTransform.position = eventData.position;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            ItemTransform.position = gameObject.transform.position;

            GameObject obj = eventData.pointerCurrentRaycast.gameObject;

            if(obj.TryGetComponent(out ScreenPluginItemSlot slot))
            {

            }
        }
    }
}
