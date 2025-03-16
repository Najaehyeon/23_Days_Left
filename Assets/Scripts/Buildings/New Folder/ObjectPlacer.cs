using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �ǹ� ������Ʈ�� �ʿ� �߰�
/// </summary>
public class ObjectPlacer : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> placedGameObjects = new List<GameObject>(); // ������ ���ӿ�����Ʈ ����

    public int PlaceObject(GameObject prefab, Vector3 position)
    {
        GameObject newObject = Instantiate(prefab);
        newObject.transform.position = position; // ���� ������Ʈ�� �� ��ǥ�� �̵�
        placedGameObjects.Add(newObject);   // ������ ���ӿ�����Ʈ ����
        return placedGameObjects.Count - 1;
    }
}
