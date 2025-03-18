using DaysLeft.Inventory;
using DaysLeft.Item;
using DaysLeft.Menu;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    private PlayerInventory _playerInventory;
    public InventoryUIController mainUI;

    private void Start()
    {
        _playerInventory = Global.Instance.Player.inventory;
        mainUI.ShowMain();
    }

    public void OnPressedAddRandomItem()
    {       
        _playerInventory.AddNew(Random.Range(1000, 1002));
    }

}
