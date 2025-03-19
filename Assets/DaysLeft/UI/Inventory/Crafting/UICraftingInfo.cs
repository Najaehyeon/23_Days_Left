using DaysLeft.Menu;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DaysLeft.Inventory
{
    public class UICraftingInfo : UIScreen
    {
        [SerializeField]
        private Image       Icon;
        [SerializeField]
        private Transform  Ingrdients;
        [SerializeField]
        private TMP_Text    Description;
        [SerializeField]
        private GameObject  IngredientSlot;
        [SerializeField]
        private Button      CraftingButton;

        protected override void Start()
        {
            base.Start();

            CraftingButton.onClick.AddListener(Craft);
        }

        public void ShowItemInfo(Recipe recipe)
        {
            Plugins.Clear();
            Icon.sprite = recipe.target.Icon;

            foreach (Transform child in Ingrdients)
                Destroy(child.gameObject);

            foreach (var rec in recipe.ingredients)
            {
                GameObject slot = Instantiate(IngredientSlot, transform);

                if (slot.TryGetComponent(out IngredientSlot ingSlot))
                {
                    ingSlot.Set(recipe.target.ID, rec.Key, rec.Value);
                    slot.transform.SetParent(Ingrdients, false);
                    Plugins.Add(ingSlot);
                }
            }

            Description.text = $"<color=yellow>{recipe.target.Name}</color>\n\n{recipe.target.Description}";
        }

        private void Craft()
        {
            PlayerInventory inven = Global.Instance.Player.inventory;


            foreach (var plugin in Plugins)
                if (plugin.TryGetComponent(out IngredientSlot ingSlot))
                    if (inven.TrySubtract(ingSlot.itemId, ingSlot.itemQuantity))
                        inven.AddNew(ingSlot.targetId);
        }
    }
}