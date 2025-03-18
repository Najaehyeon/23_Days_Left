using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item_Consume", menuName = "ScriptableObject/ItemData/Consume")]
public class ConsumeItemData : ItemData
{
    public Action ConsumeEffect;

    public void Awake()
    {
        ConsumeEffect += ExampleAddHealth;
        ConsumeEffect += ExampleAddHungry;
    }

    public void ExampleAddHungry()
    {

    }

    public void ExampleAddHealth()
    {

    }
}