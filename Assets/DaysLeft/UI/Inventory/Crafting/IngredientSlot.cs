using DaysLeft.Item;
using DaysLeft.Menu;
using System.Collections;
using System.Collections.Generic;
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
        text.text = ingredientQuantity.ToString();                
    }
}
