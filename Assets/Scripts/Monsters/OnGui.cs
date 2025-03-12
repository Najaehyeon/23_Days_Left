using System;
using System.Collections;
using System.Collections.Generic;
using _23DaysLeft.Managers;
using UnityEngine;

public class OnGui : MonoBehaviour
{
    public bool isDebug = true;
    private List<GameObject> spawnObjects = new();

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
    }
}