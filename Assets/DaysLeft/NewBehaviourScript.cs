using DaysLeft.Inventory;
using DaysLeft.Item;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    private PlayerInventory _playerInventory;

    private void Start()
    {
        _playerInventory = Global.Instance.Player.inventory;    
    }

    public void OnPressedAddRandomItem()
    {
       
        _playerInventory.AddNew(Random.Range(0, 2));
    }
}
