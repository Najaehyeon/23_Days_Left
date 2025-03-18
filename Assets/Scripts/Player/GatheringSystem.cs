using DaysLeft.Inventory;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class GatheringSystem : MonoBehaviour
{
    PlayerController playerController;
    PlayerInventory inventory;

    private void Start()
    {
        playerController = CharacterManager.Instance.Player.controller;
        inventory = CharacterManager.Instance.Player.inventory;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Resource"))
        {
            inventory.AddNew(other.GetComponent<ItemComponent>().ID);
            Destroy(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Resource"))
        {
            playerController.itemId = -1;
        }
    }
}
