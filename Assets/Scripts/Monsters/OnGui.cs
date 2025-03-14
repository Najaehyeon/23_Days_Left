using System;
using System.Collections;
using System.Collections.Generic;
using _23DaysLeft.Managers;
using _23DaysLeft.Monsters;
using UnityEngine;

public class OnGui : MonoBehaviour
{
    public bool isDebug = true;
    private List<GameObject> spawnObjects = new();
    [SerializeField] private CreatureSpawner spawner;
    [SerializeField] private CreatureController creature;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }

    public void OnGUI()
    {
        if (!isDebug) return;
        
        GUIStyle buttonStyle = new GUIStyle(GUI.skin.button);
        buttonStyle.fontSize = 25;
        
        // 스폰 테스트
        if (GUI.Button(new Rect(10, 10, 200, 50), "Spawn", buttonStyle))
        {
            var test = PoolManager.Instance.Spawn("Kitty_001");
            spawnObjects.Add(test);
        }
        
        // 디스폰 테스트
        if (GUI.Button(new Rect(10, 70, 200, 50), "All Despawn", buttonStyle))
        {
            for (int i = 0; i < spawnObjects.Count; i++)
            {
                PoolManager.Instance.Despawn(spawnObjects[i]);
            }
        }
        
        // Creature Hit 테스트
        if (GUI.Button(new Rect(10, 130, 200, 50), "Creature Hit", buttonStyle))
        {
            creature.OnHit(20f);
        }
        
        // Creature 스폰 테스트
        if (GUI.Button(new Rect(10, 190, 200, 50), "Creature Spawn", buttonStyle))
        {
            spawner.Spawn();
        }
    }
}