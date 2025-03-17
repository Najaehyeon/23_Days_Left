using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �׸��忡 ������Ʈ�� ������ �ڸ� ���¸� ����
/// </summary>
public class GridData
{
    Dictionary<Vector3Int, PlacementData> placedObjects = new Dictionary<Vector3Int, PlacementData>(); // c# 9.0������ new();�� �Է��ص� �ȴ�
    public void AddObjectAt(Vector3Int gridPosition, Vector2Int objectSize, int ID, int placedObjIdx)
    {
        List<Vector3Int> positionToOccupy = CalculatePositions(gridPosition, objectSize);
        PlacementData data = new PlacementData(positionToOccupy, ID, placedObjIdx);
        foreach (var pos in positionToOccupy)
        {
            // ���� �̹� ������ �ڸ����
            if (placedObjects.ContainsKey(pos))
                throw new Exception($"Dictionary already contains this cell position {pos}");
            placedObjects[pos] = data;
        }
    }

    private List<Vector3Int> CalculatePositions(Vector3Int gridPosition, Vector2Int objectSize)
    {
        List<Vector3Int> returnVal = new List<Vector3Int>();
        // object�� ���� �Ʒ� ����
        /// ȸ���� �ݿ��ϰ� �ʹٸ� ȸ������ ������� ������ �� �� ������ �Ѵ�
        /// ������ ��ȸ��
        for (int x = 0; x < objectSize.x; x++)
        {
            for (int y = 0; y < objectSize.y; y++)
            {
                returnVal.Add(gridPosition + new Vector3Int(x, 0, y));
            }
        }
        return returnVal;
    }
    public bool CanPlaceObjAt(Vector3Int gridPosition, Vector2Int objectSize)
    {
        List<Vector3Int> positionToOccupy = CalculatePositions(gridPosition, objectSize);
        foreach (var pos in positionToOccupy)
        {
            if (placedObjects.ContainsKey(pos))
            {
                /// ���⼭ ���� �ٲܰŴ�?
                return false;
            }
        }
        return true;
    }
    // ���� ���� ������ ���� ����
    // �����ϴ� ������ ������, ������ ���� �������� ���� ��ȹ
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