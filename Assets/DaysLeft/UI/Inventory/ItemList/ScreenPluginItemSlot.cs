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
        [Header("Components", order = 1)]
        [Header("Information", order = 2)]
        [SerializeField]
        private TMP_Text    ItemQuantity;
        [SerializeField]
        private Image       ItemIcon;
        [Header("Controlled", order = 2)]
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
            _data = null;
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
            if (_data == null)
                return;

            ItemIconCanvas.sortingOrder = 1000;
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (_data == null)
                return;
            ItemTransform.position = eventData.position;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (_data == null)
                return;
            ItemTransform.position = gameObject.transform.position;

            GameObject obj = eventData.pointerCurrentRaycast.gameObject;

            if(obj.TryGetComponent(out ScreenPluginItemSlot slot))
            {
                if(slot._data == null)
                {
                    slot.Set(_data);
                    this.Clear();
                }
                else
                {
                    ItemInstance targetData = new ItemInstance(slot._data);
                    slot.Set(_data);
                    this.Set(targetData);
                }
            }

            ItemIconCanvas.sortingOrder = 10;
        }
    }
}
