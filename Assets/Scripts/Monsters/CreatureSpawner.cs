using System.Collections;
using System.Collections.Generic;
using _23DaysLeft.Managers;
using UnityEngine;
using Random = UnityEngine.Random;

public class CreatureSpawner : MonoBehaviour
{
    [Header("Spawn Creatures")]
    [SerializeField] private Creatures[] spawnList;
    
    [Header("Spawn Settings")]
    [SerializeField] private float radius = 30f;
    [SerializeField] private int spawnY = 3;
    [SerializeField] private int maxCount = 10;
    [SerializeField] private float spawnDelay = 20f;
    
    private List<Creature> spawnedCreatures = new();
    private WaitForSeconds waitTime;
    private int spawnCount = 0;

    private void Start()
    {
        waitTime = new WaitForSeconds(spawnDelay);
        StartCoroutine(SpawnCoroutine());
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }

    private IEnumerator SpawnCoroutine()
    {
        while (true)
        {
            yield return waitTime;
            Spawn();
        }
    }
    
    public void Spawn()
    {
        if (spawnCount >= maxCount) return;

        var creature = PoolManager.Instance.Spawn<Creature>(GetSpawnCreature());
        creature.Init(GetSpawnPos());
        spawnCount++;
    }

    private string GetSpawnCreature()
    {
        // var index = Random.Range(0, spawnList.Length);
        // var key = spawnList[index].ToKey();
        // var data = Global.Instance.DataLoadManager.GetCreatureData(key);
        // return data.name;
        
        return "Colobus";
    }
    
    private Vector3 GetSpawnPos()
    {
        Vector3 spawnPos = transform.position + Random.onUnitSphere * radius;
        spawnPos.y = spawnY;
        return spawnPos;
    }

    public void DespawnCreature(int count)
    {
        if (spawnedCreatures.Count == 0) return;

        if (count > spawnedCreatures.Count)
        {
            count = spawnedCreatures.Count;
        }
        
        for (int i = 0; i < count; i++)
        {
            var creature = spawnedCreatures[i];
            spawnedCreatures.RemoveAt(i);
            PoolManager.Instance.Despawn(creature.gameObject);
        }
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}

public enum Creatures
{
    Colobus = 3000,
}
