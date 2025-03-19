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

    public TempInputManager tempInputManager;  // angle을 가져온다

    public int PlaceObject(GameObject prefab, Vector3 position)
    {
        /// tempInputManager에서 회전각을 가져온다
        /// 이건 preview오브젝트가 회전한 각이기도 하지만, 
        /// 회전축이 회전한 각이기도 하다
        float angle = tempInputManager.angle; 

        GameObject newObject = Instantiate(prefab);
        newObject.transform.position = position; // 오브젝트를 그리드 좌표를 월드 좌표로 환산한 값으로 이동
        newObject.transform.rotation = Quaternion.Euler(0, angle, 0); // Y축 기준으로 회전

        placedGameObjects.Add(newObject);   // 생성한 게임오브젝트 저장
        return placedGameObjects.Count - 1;
    }
}
