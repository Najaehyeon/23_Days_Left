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
        ItemInstance itemInstance = new(Random.Range(1,2));
        
        _playerInventory.Add(itemInstance);
    }
}
