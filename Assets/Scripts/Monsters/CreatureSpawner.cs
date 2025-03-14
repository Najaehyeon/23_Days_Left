using System;
using System.Collections;
using System.Collections.Generic;
using _23DaysLeft.Managers;
using UnityEngine;
using Random = UnityEngine.Random;

public class CreatureSpawner : MonoBehaviour
{
    [SerializeField] private float radius = 10f;
    [SerializeField] private int spawnY = 3;
    [SerializeField] private int maxCount = 10;
    [SerializeField] private Creatures[] creatureIds;
    private int spawnCount = 0;

    public void Spawn()
    {
        // PoolManager.Instance.spawn(data.name)
        var creature = PoolManager.Instance.Spawn<Creature>("Colobus", transform);
        creature.Init(GetSpawnPos());
    }

    private Vector3 GetSpawnPos()
    {
        Vector3 spawnPos = transform.position + Random.onUnitSphere * radius;
        spawnPos.y = spawnY;
        return spawnPos;
        // var randomIndex = Random.Range(0, creatureIds.Length);
        // var creatureId = (int)creatureIds[randomIndex];
        // var creatureName = GetSpawnCreature(creatureId);
    }

    private string GetSpawnCreature(int id)
    {
        // enum -> id
        // var data = DataManager.Instance.GetCreatureData(id)
        // return data.name;
        return "";
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }

    public enum Creatures
    {
        Colobus = 3000,
    }
}
