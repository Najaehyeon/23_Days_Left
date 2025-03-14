using DaysLeft.Item;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using _23DaysLeft.Monsters;

public class DataLoadManager : MonoBehaviour 
{
    private List<ItemData> itemDatas;
    private Dictionary<string, CreatureData> creatureDatas;

    void Awake()
    {
        itemDatas = Resources.LoadAll<ItemData>("Item").ToList(); 
        LoadCreatureData();
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
    
    public void LoadCreatureData()
    {
        creatureDatas = new Dictionary<string, CreatureData>();
        var creatureData = Resources.LoadAll<CreatureData>("Creature");
        foreach (var data in creatureData)
        {
            creatureDatas.Add(data.name, data);
        }
    }

    public CreatureData GetCreatureData(string creatureName)
    {
        if (creatureDatas.TryGetValue(creatureName, out var data))
            return data;
        else
        {
            Debug.LogError($"CreatureData {creatureName} is not found");
            return null;
        }
    }
}
