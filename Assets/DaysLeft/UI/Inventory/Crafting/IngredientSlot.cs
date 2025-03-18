using DaysLeft.Menu;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IngredientSlot : ScreenPlugin
{
    [SerializeField]
    private Image       icon;
    [SerializeField]
    private TMP_Text    text;

    public int targetId;
    public int itemId;
    public int itemQuantity;
    
    public void Set(int _targetId, ItemData ingredient, int ingredientQuantity)
    {
        targetId = _targetId;
        itemId = ingredient.ID;
        itemQuantity = ingredientQuantity;

        icon.sprite = ingredient.Icon;

        int remainNum = 0;

        foreach(var item in Global.Instance.Player.inventory.QueryAll(itemId))
        {
            remainNum += item.Quantity;
        }

        text.text = $"{remainNum} / {ingredientQuantity}";
        if(remainNum > ingredientQuantity)
        {
            text.color = Color.white;
        }
        else
        {
            text.color = Color.red;
        }
    }
}
