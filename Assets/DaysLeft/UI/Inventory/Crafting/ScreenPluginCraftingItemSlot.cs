using DaysLeft.Menu;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.UI;

public class ScreenPluginCraftingItemSlot : ScreenPlugin
{
    [Header("Component")]
    [SerializeField]
    private Button  Slot;
    private int     id;

    private Recipe _recipe;

    private InventoryUIController _controller;

    private void Awake()
    {
    }

    public void Set(Recipe recipe)
    {
        _recipe = recipe;
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
