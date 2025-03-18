using System.Collections;
using System.Collections.Generic;
using _23DaysLeft.Managers;
using _23DaysLeft.Utils;
using System;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace _23DaysLeft.Monsters
{
    public class CreatureSpawner : MonoBehaviour
    {
        [Header("Spawn Creatures")]
        [SerializeField] private Creatures[] spawnList;

        [Header("Spawn Settings")]
        [SerializeField] private float radius = 30f;
        [SerializeField] private int spawnY = 3;
        [SerializeField] private int maxCount = 10;
        [SerializeField] private float spawnDelay = 20f;

        private WaitForSeconds waitTime;
        private bool isSceneInit;
        private int spawnCount = 0;
        
        private void Start()
        {
            MainSceneBase.Instance.OnMainSceneInitComplete += () => isSceneInit = true;
            waitTime = new WaitForSeconds(spawnDelay);
            StartCoroutine(SpawnCoroutine());
        }

        private void OnDestroy()
        {
            StopAllCoroutines();
        }

        private IEnumerator SpawnCoroutine()
        {
            while (!isSceneInit) yield return null;
            while (true)
            {
                Spawn();
                yield return waitTime;
            }
        }

        private void Spawn()
        {
            if (spawnCount >= maxCount) return;

            var spawnPos = GetSpawnPos();
            var creature = Global.Instance.PoolManager.Spawn<Creature>(GetSpawnCreature());
            if (NavMesh.SamplePosition(spawnPos, out var hit, 5f, NavMesh.AllAreas))
            {
                spawnPos = hit.position;
            }

            creature.Init(spawnPos);
            creature.Controller.CreatureSpawned(this);
            creature.transform.parent = transform;
            spawnCount++;
        }

        private string GetSpawnCreature()
        {
            var index = Random.Range(0, spawnList.Length);
            var key = spawnList[index].ToName();
            var data = Global.Instance.DataLoadManager.GetCreatureData(key);
            return data.name;
        }

        private Vector3 GetSpawnPos()
        {
            Vector3 spawnPos = transform.position + Random.onUnitSphere * radius;
            spawnPos.y = spawnY;
            return spawnPos;
        }
        
        public void CreatureDied()
        {
            spawnCount--;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, radius);
        }
    }
}