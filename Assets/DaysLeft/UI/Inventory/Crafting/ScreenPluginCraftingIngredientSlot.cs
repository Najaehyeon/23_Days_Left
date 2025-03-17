using DaysLeft.Menu;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.UI;

public class ScreenPluginCraftingIngredientSlot : ScreenPlugin
{
    [Header("Component")]
    [SerializeField]
    private Image       Image;
    [SerializeField]
    private TMP_Text    Quantity;

    private Recipe _recipe;

    private InventoryUIController _controller;

    private void Awake()
    {
    }

    public void Set(Recipe recipe)
    {
        _recipe = recipe;
        Image.sprite = _recipe.target.Icon;
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
}
