using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{

    public void OnPressedAddRandomItem()
    {
        Global.Instance.Player.inventory.AddNew(Random.Range(0, 2));
        Global.Instance.Player.inventory.AddNew(Random.Range(0, 2));
        Global.Instance.Player.inventory.AddNew(Random.Range(0, 2));
        Global.Instance.Player.inventory.AddNew(Random.Range(0, 2));
        Global.Instance.Player.inventory.AddNew(Random.Range(0, 2));
        Global.Instance.Player.inventory.AddNew(Random.Range(0, 2));
        Global.Instance.Player.inventory.AddNew(Random.Range(0, 2));
        Global.Instance.Player.inventory.AddNew(Random.Range(0, 2));
        Global.Instance.Player.inventory.AddNew(Random.Range(0, 2));
        Global.Instance.Player.inventory.AddNew(Random.Range(0, 2));
        Global.Instance.Player.inventory.AddNew(Random.Range(0, 2));
        Global.Instance.Player.inventory.AddNew(Random.Range(0, 2));
        Global.Instance.Player.inventory.AddNew(Random.Range(0, 2));
        Global.Instance.Player.inventory.AddNew(Random.Range(0, 2));
        Global.Instance.Player.inventory.AddNew(Random.Range(0, 2));
        Global.Instance.Player.inventory.AddNew(Random.Range(0, 2));
        Global.Instance.Player.inventory.AddNew(Random.Range(0, 2));
        Global.Instance.Player.inventory.AddNew(Random.Range(0, 2));
    }

}
