using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 건물 오브젝트를 맵에 추가
/// </summary>
public class ObjectPlacer : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> placedGameObjects = new List<GameObject>(); // 생성된 게임오브젝트 저장

    public int PlaceObject(GameObject prefab, Vector3 position)
    {
        GameObject newObject = Instantiate(prefab);
        newObject.transform.position = position; // 격자 오브젝트를 그 좌표로 이동
        placedGameObjects.Add(newObject);   // 생성한 게임오브젝트 저장
        return placedGameObjects.Count - 1;
    }
}
