using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 에디터 사용
[CreateAssetMenu]
public class ObjectsDatabaseSO : ScriptableObject
{
    public List<ObjectData> objectsData;

}


[Serializable]
public class ObjectData
{
    [field: SerializeField]
    public string Name { get; private set; }
    [field: SerializeField]
    public int ID { get; private set; }
    [field: SerializeField]
    public Vector2Int Size { get; private set; } = Vector2Int.one;  // 기본크기
    [field: SerializeField]
    public GameObject Prefab { get; private set; }
}