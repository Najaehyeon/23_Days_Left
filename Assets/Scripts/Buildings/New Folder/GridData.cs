using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 그리드에 오브젝트를 놓을때 자리 상태를 저장
/// </summary>
public class GridData
{
    Dictionary<Vector3Int, PlacementData> placedObjects = new Dictionary<Vector3Int, PlacementData>(); // c# 9.0에서는 new();만 입력해도 된다
    public void AddObjectAt(Vector3Int gridPosition, Vector2Int objectSize, int ID, int placedObjIdx)
    {
        List<Vector3Int> positionToOccupy = CalculatePositions(gridPosition, objectSize);
        PlacementData data = new PlacementData(positionToOccupy, ID, placedObjIdx);
        foreach (var pos in positionToOccupy)
        {
            // 누가 이미 점유한 자리라면
            if (placedObjects.ContainsKey(pos))
                throw new Exception($"Dictionary already contains this cell position {pos}");
            placedObjects[pos] = data;
        }
    }

    private List<Vector3Int> CalculatePositions(Vector3Int gridPosition, Vector2Int objectSize)
    {
        List<Vector3Int> returnVal = new List<Vector3Int>();
        // object의 왼쪽 아래 기준
        /// 회전을 반영하고 싶다면 회전값을 기반으로 루프를 몇 개 만들어야 한다(?)
        /// 지금은 무회전
        for (int x = 0; x < objectSize.x; x++)
        {
            for (int y = 0; y < objectSize.y; y++)
            {
                returnVal.Add(gridPosition + new Vector3Int(x, 0, y));
            }
        }
        return returnVal;
    }

    /// <summary>
    /// 회전된 오브젝트가 차지하는 영역을 계산
    /// </summary>
    /// <param name="gridPosition"></param>
    /// <param name="objectSize"></param>
    /// <param name="rotationAngle"></param>
    /// <returns></returns>
    private List<Vector3Int> CalculateRotatedPositions(Vector3Int gridPosition, Vector2Int objectSize, float rotationAngle)
    {
        List<Vector3Int> returnVal = new List<Vector3Int>();

        // 중심점을 기준으로 회전하도록 하기 위한 pivot 계산
        Vector3 pivot = new Vector3(gridPosition.x + objectSize.x / 2f, 0, gridPosition.z + objectSize.y / 2f);

        // 회전된 위치를 계산
        for (int x = 0; x < objectSize.x; x++)
        {
            for (int y = 0; y < objectSize.y; y++)
            {
                // 원래의 좌표 계산
                Vector3Int position = gridPosition + new Vector3Int(x, 0, y);

                // 회전된 좌표로 변환
                Vector3 rotatedPosition = RotatePosition(position, pivot, rotationAngle);
                returnVal.Add(new Vector3Int(Mathf.RoundToInt(rotatedPosition.x), 0, Mathf.RoundToInt(rotatedPosition.z)));
            }
        }

        return returnVal;
    }
    /// <summary>
    /// 회전된 좌표 계산
    /// </summary>
    /// <param name="position"></param>
    /// <param name="pivot"></param>
    /// <param name="angle"></param>
    /// <returns></returns>
    private Vector3 RotatePosition(Vector3 position, Vector3 pivot, float angle)
    {
        Vector3 dir = position - pivot;  // 중심을 기준으로 방향 계산
        Quaternion rotation = Quaternion.Euler(0, angle, 0);  // Y축 기준 회전
        Vector3 rotatedDir = rotation * dir;  // 회전 적용
        return pivot + rotatedDir;  // 회전된 위치
    }

    public bool CanPlaceObjAt(Vector3Int gridPosition, Vector2Int objectSize)
    {
        List<Vector3Int> positionToOccupy = CalculatePositions(gridPosition, objectSize);
        foreach (var pos in positionToOccupy)
        {
            if (placedObjects.ContainsKey(pos))
            {
                /// 여기서 색깔 바꿀거다?
                return false;
            }
        }
        return true;
    }
    // 쓰는 곳이 없으면 지울 예정
    // 삭제하는 곳에서 쓰던데, 삭제는 따로 구현하지 않을 계획
    internal int GetRepresentationIndex(Vector3Int gridPosition)
    {
        if (placedObjects.ContainsKey(gridPosition) == false)
            return -1;
        return placedObjects[gridPosition].PlacedObjectIndex;
    }
}

public class PlacementData
{
    public List<Vector3Int> occupiedPositions;
    public int ID { get; private set; }
    public int PlacedObjectIndex { get; private set; }
    public PlacementData(List<Vector3Int> occupiedPositions, int iD, int placedObjectIndex)
    {
        this.occupiedPositions = occupiedPositions;
        ID = iD;
        PlacedObjectIndex = placedObjectIndex;
    }
}