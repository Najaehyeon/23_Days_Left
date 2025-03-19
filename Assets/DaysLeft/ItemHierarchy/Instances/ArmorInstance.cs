using Unity.VisualScripting;
using UnityEngine;

public class ArmorInstance : ItemInstance
{
    public float currentDurability; // 현재 내구도 

    private ArmorItemData _armorItemData;
    public ArmorInstance(int id) : base(id)
    {
        DataLoadManager dataLoad = Global.Instance.DataLoadManager;

        if (dataLoad == null)
            Debug.LogError("DataLoadManager is not there in Global");
        else
            _armorItemData = dataLoad.Query(id) as ArmorItemData;
    }

    public ArmorInstance(ArmorInstance instance) : base(instance)
    {
        currentDurability = instance.currentDurability;
        _armorItemData = instance._armorItemData;
    }
    public float durability => _armorItemData.Durability; // 총 내구도 (durability = 내구도)
    public ArmorType armorType => _armorItemData.Type;
    public MaterialType materialType => _armorItemData.Material;
}
