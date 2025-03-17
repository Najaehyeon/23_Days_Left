using DaysLeft.Inventory;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerController controller;
    public PlayerInventory  inventory;

    private void Awake()
    {
        CharacterManager.Instance.Player = this;
        controller =  GetComponent<PlayerController>();
        Global.Instance.Player = this;
        inventory = new();
    }
}
