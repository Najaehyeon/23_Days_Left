using UnityEngine;

public enum ArmorType
{
    Helmet,
    Chest,
    Panth,
    Boots,
    Glove,
    Shoulders
}

public enum MaterialType
{
    Leather,
    Plate
}

[CreateAssetMenu(fileName = "Item_Armor", menuName = "ScriptableObject/ItemData/Armor")]
public class ArmorItemData : ItemData
{
    public ArmorType Type;
    public MaterialType Material;

    [Header("Status")]
    public float DefencePower;
    public float Durability; // 총 내구도 (durability = 내구도)
}