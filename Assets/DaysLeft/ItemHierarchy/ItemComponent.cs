using UnityEngine;

public class ItemComponent : MonoBehaviour
{
    [SerializeField]
    private ItemData        ItemData;

    private ItemInstance    itemInstance;

    public  int             ID          => itemInstance.ID;
    public  ItemInstance    Instance    => itemInstance;

    public void Awake()
    {
        itemInstance = new(ItemData);
    }
}
