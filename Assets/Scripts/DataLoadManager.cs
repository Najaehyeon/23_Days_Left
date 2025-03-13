using DaysLeft.Item;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class DataLoadManager
{
    private List<ItemData> itemDatas;

    void Awake()
    {
        itemDatas = Resources.LoadAll<ItemData>("Resource/Item").ToList();
    }

    /// <summary>
    /// Get item by idx
    /// </summary>
    /// <param name="idx"> idx of item for searching</param>
    public ItemData Query(int idx)
        => (from item in itemDatas where item.ID == idx select item).FirstOrDefault();

    /// <summary>
    /// Get item by name (case-insensitive)
    /// </summary>
    /// <param name="name"> name of item for searching</param>
    public ItemData Query(string name)
        => (from item in itemDatas where item.Name.Equals(name, System.StringComparison.OrdinalIgnoreCase) select item).FirstOrDefault();
}
