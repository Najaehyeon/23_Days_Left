namespace DaysLeft.Menu
{
    using DaysLeft.Inventory;
    using DaysLeft.Item;
    using System.Collections.Generic;
    using UnityEngine;

    public class ViewUIInventory : UIScreen
    {

        protected PlayerInventory _inventory;

        protected override void Start()
        {
            base.Start();

            _inventory = Global.Instance.Player.inventory;

            if (_inventory == null)
            {
                Debug.LogError($"There is no Player in this scene");
                return;
            }

            UpdateSlots();

            _inventory.OnContentsChanged += UpdateSlots;
        }

        public void UpdateSlots()
        {
            for (int i = 0; i < Plugins.Count; ++i)
            {
                if (Plugins[i] is not ScreenPluginItemSlot slot)
                    continue;

                ItemInstance data = _inventory.Get(i);
                if (data != null)
                    slot.Set(data);
                else
                    slot.Clear();
            }
        }

        public void OnPressedCraftButton()
        {
            Controller.Show<ViewUICrafting>();
        }
    }
}