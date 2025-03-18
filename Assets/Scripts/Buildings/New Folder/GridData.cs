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

    /// <summary>
    /// 매개변수로 들어오는 float angle은 TempInputManager.angle
    /// 따라서 변수 선언해서 참조할 필요 없다
    /// </summary>
    /// <param name="gridPosition"></param>
    /// <param name="objectSize"></param>
    /// <param name="ID"></param>
    /// <param name="placedObjIdx"></param>
    /// <param name="rotation"></param>
    /// <exception cref="Exception"></exception>
    //public void AddObjectAt(Vector3Int gridPosition, Vector2Int objectSize, int ID, int placedObjIdx, float angle, Transform parent)
    //{
    //    // 현재 저장하려는 preview 오브젝트의 그리드 좌표, 회전상태 
    //    List<Vector3Int> positionToOccupy = CalculatePositions(gridPosition, objectSize);
    //    PlacementData data = new PlacementData(positionToOccupy, ID, placedObjIdx, angle, gridPosition, objectSize);

    //    foreach (var pos in positionToOccupy)
    //    {
    //        // 누가 이미 점유한 자리라면
    //        if (placedObjects.ContainsKey(pos))
    //        {
    //            // 매개변수로 들어온 rotation과 placedObjects에 저장된 rotation과 같은지 비교하고,
    //            // 저장된 rotation - 매개변수로 들어온 rotation의 값 만큼 pos들을 회전한다
    //            // 기존에 저장된 오브젝트의 회전 값 가져오기
    //            float storedAngle = placedObjects[pos].angle;
    //            // 저장된 회전값 - 현재 회전각의 차이를 구함
    //            float angleDiff = storedAngle - angle;
    //            /// 현재 pos를 회전 차이에 따라 변환
    //            Vector3Int rotatedPos = RotatePosition(pos, gridPosition, angleDiff, parent);

    //            // 변환된 좌표가 기존 좌표 중 하나와 겹친다면 충돌
    //            if (placedObjects.ContainsKey(Vector3Int.RoundToInt(rotatedPos)))
    //            {
    //                throw new Exception($"Placement conflict at {pos} after rotation adjustment");
    //            }
    //        }
    //        placedObjects[pos] = data;
    //    }
    //}
    public void AddObjectAt(Vector3Int gridPosition, Vector2Int objectSize, int ID, int placedObjIdx, float angle, Transform parent)
    {
        // 현재 저장하려는 preview 오브젝트의 그리드 좌표, 회전상태 
        List<Vector3Int> positionToOccupy = CalculatePositions(gridPosition, objectSize, parent);
        PlacementData data = new PlacementData(positionToOccupy, ID, placedObjIdx, angle, gridPosition, objectSize);

        foreach (var pos in positionToOccupy)
        {
            // 부모 기준 좌표로 변환
            Vector3Int localPos = ConvertToLocalGridPosition(pos, parent);

            // 누가 이미 점유한 자리라면
            if (placedObjects.ContainsKey(localPos))
            {
                // 기존에 저장된 오브젝트의 회전 값 가져오기
                float storedAngle = placedObjects[localPos].angle;
                // 저장된 회전값 - 현재 회전각의 차이를 구함
                float angleDiff = storedAngle - angle;
                /// 현재 pos를 회전 차이에 따라 변환
                Vector3Int rotatedPos = RotatePosition(localPos, gridPosition, angleDiff, parent);

                // 변환된 좌표가 기존 좌표 중 하나와 겹친다면 충돌
                if (placedObjects.ContainsKey(rotatedPos))
                {
                    throw new Exception($"Placement conflict at {pos} after rotation adjustment");
                }
            }
            placedObjects[localPos] = data;
        }
    }
    // 부모 기준으로 로컬 그리드 좌표를 변환
    private Vector3Int ConvertToLocalGridPosition(Vector3Int worldGridPos, Transform parent)
    {
        Vector3 worldPos = new Vector3(worldGridPos.x, worldGridPos.y, worldGridPos.z);
        Vector3 localPos = parent.InverseTransformPoint(worldPos);

        return new Vector3Int(
            Mathf.RoundToInt(localPos.x),
            Mathf.RoundToInt(localPos.y),
            Mathf.RoundToInt(localPos.z)
        );
    }



    /// <summary>
    /// 매개변수로 들어오는 Vector3Int 는 grid의 좌표
    /// 회전 차이를 적용하여 기존 pos를 변환(축의 회전각을 맞춘다면 실제로 있어야 할 자리는 어디인지 구하기 위해)
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="pivot"></param>
    /// <param name="rotationDiff"></param>
    /// <returns></returns>
    //private Vector3Int RotatePosition(Vector3 pos, Vector3 pivot, float angleDiff, Transform parent)
    //{
    //    // 중심을 보정하여 로컬 좌표 변환
    //    // RotatePosition에서는 pos의 x, z를 0.5씩 더하여 중심을 맞춘 후 회전 적용
    //    Vector3 adjustedPos = new Vector3(pos.x + 0.5f, pos.y, pos.z + 0.5f);
    //    Vector3 localPos = adjustedPos - pivot;

    //    // 회전 행렬 계산 (Y축 기준)
    //    float rad = Mathf.Deg2Rad * angleDiff;
    //    float cos = Mathf.Cos(rad);
    //    float sin = Mathf.Sin(rad);

    //    /// 90도 단위 회전이므로, 부동소수점 오차를 처리
    //    if (Mathf.Abs(cos) < 1e-6f) cos = 0f;
    //    if (Mathf.Abs(sin) < 1e-6f) sin = 0f;

    //    if (Mathf.Abs(cos - 1f) < 1e-6f) cos = 1f;
    //    if (Mathf.Abs(sin - 1f) < 1e-6f) sin = 1f;
    //    if (Mathf.Abs(cos + 1f) < 1e-6f) cos = -1f;
    //    if (Mathf.Abs(sin + 1f) < 1e-6f) sin = -1f;

    //    float rotatedX = cos * localPos.x + sin * localPos.z;
    //    float rotatedZ = -sin * localPos.x + cos * localPos.z;

    //    // 다시 월드 좌표로 변환하고, 그리드 기준으로 정수 변환
    //    Vector3 newPos = pivot + new Vector3(rotatedX, localPos.y, rotatedZ);

    //    return new Vector3Int(
    //        Mathf.FloorToInt(newPos.x),  // RoundToInt 대신 FloorToInt 사용
    //        Mathf.RoundToInt(newPos.y),
    //        Mathf.FloorToInt(newPos.z)
    //    );
    //}
    private Vector3Int RotatePosition(Vector3Int gridPos, Vector3Int pivot, float angleDiff, Transform parent)
    {
        // 부모 기준 로컬 좌표 변환
        Vector3 localPos = parent.InverseTransformPoint(new Vector3(gridPos.x, gridPos.y, gridPos.z));
        Vector3 localPivot = parent.InverseTransformPoint(new Vector3(pivot.x, pivot.y, pivot.z));

        // 중심점 기준 상대 좌표
        Vector3 relativePos = localPos - localPivot;

        // 회전 행렬 적용 (Y축 기준)
        float rad = Mathf.Deg2Rad * angleDiff;
        float cos = Mathf.Cos(rad);
        float sin = Mathf.Sin(rad);

        float rotatedX = cos * relativePos.x + sin * relativePos.z;
        float rotatedZ = -sin * relativePos.x + cos * relativePos.z;

        // 다시 부모 기준 월드 좌표로 변환
        Vector3 newWorldPos = parent.TransformPoint(localPivot + new Vector3(rotatedX, relativePos.y, rotatedZ));

        // 최종적으로 그리드 좌표 변환
        return new Vector3Int(
            Mathf.RoundToInt(newWorldPos.x),
            Mathf.RoundToInt(newWorldPos.y),
            Mathf.RoundToInt(newWorldPos.z)
        );
    }


    /// <summary>
    /// gridPosition은 preview 오브젝트의 Grid 좌표의 기준점(0도 회전 기준 왼쪽 아래)으로 마우스 커서가 있는 그리드의 좌표이다
    /// objectSize는 preview 오브젝트의 크기
    /// </summary>
    /// <param name="gridPosition"></param>
    /// <param name="objectSize"></param>
    /// <returns></returns>
    //private List<Vector3Int> CalculatePositions(Vector3Int gridPosition, Vector2Int objectSize)
    //{
    //    List<Vector3Int> returnVal = new List<Vector3Int>();
    //    // object의 왼쪽 아래 기준
    //    /// preview 오브젝트가 점유하고있는 grid좌표를 returnVal에 담아서 리턴한다 ★
    //    for (int x = 0; x < objectSize.x; x++)
    //    {
    //        for (int y = 0; y < objectSize.y; y++)
    //        {
    //            returnVal.Add(gridPosition + new Vector3Int(x, 0, y));
    //        }
    //    }
    //    return returnVal;
    //}
    private List<Vector3Int> CalculatePositions(Vector3Int gridPosition, Vector2Int objectSize, Transform parent)
    {
        List<Vector3Int> returnVal = new List<Vector3Int>();

        // 부모 기준 로컬 좌표 변환
        Vector3Int localGridPosition = ConvertToLocalGridPosition(gridPosition, parent);

        // object의 왼쪽 아래 기준
        /// preview 오브젝트가 점유하고있는 grid좌표를 returnVal에 담아서 리턴한다 ★
        for (int x = 0; x < objectSize.x; x++)
        {
            for (int y = 0; y < objectSize.y; y++)
            {
                returnVal.Add(localGridPosition + new Vector3Int(x, 0, y));
            }
        }
        return returnVal;
    }

    ///// <summary>
    ///// gridPosition은 preview 오브젝트의 Grid 좌표의 기준점(0도 회전 기준 왼쪽 아래)으로 마우스 커서가 있는 그리드의 좌표이다
    ///// objectSize는 preview오브젝트의 크기
    ///// angle으로 넘어오는건 TempInputManager.angle으로 회전각
    ///// </summary>
    ///// <param name="gridPosition"></param>
    ///// <param name="objectSize"></param>
    ///// <returns></returns>
    //public bool CanPlaceObjAt(Vector3Int gridPosition, Vector2Int objectSize, float angle)
    //{
    //    /// gridPosition는 그리드의 좌표(월드좌표 아니다)
    //    List<Vector3Int> positionToOccupy = CalculatePositions(gridPosition, objectSize);
    //    foreach (var pos in positionToOccupy)   /// pos는 Vector3Int값으로 그리드의 좌표
    //    {
    //        /// 해당 그리드 좌표에 다른 오브젝트 점유하고 있는 영역이 있는지 확인한다★
    //        /// placedObject는 PlacedObjectData를 저장하고있으며
    //        /// PlacedObjectData는 
    //        /// PlacedObject가 생성된 GameObject를 저장하고 있다
    //        /// 기존에는 pos를 대입하면 해당 pos가 있는 게임오브젝트를 리턴하기 때문에
    //        /// 수정한 다음에도 GameObject를 가져와야한다
    //        if (placedObjects.ContainsKey(pos))
    //        {
    //            // 기존 오브젝트 데이터 가져오기
    //            PlacementData existingData = placedObjects[pos];

    //            // 기존 오브젝트의 회전값과 현재 회전값의 차이 계산
    //            float angleDiff = (existingData.angle - angle) % 360;

    //            /// pivot을 기존 occupiedPositions[0]을 기준으로 설정
    //            /// occupiedPositions[1] 부터는 회전축이 될 수 없다!
    //            Vector3Int pivot = Vector3Int.RoundToInt(existingData.occupiedPositions[0]);

    //            ///// 회전축이 같은 경우
    //            if (pivot == pos)
    //            {
    //                Vector3 originPos = new Vector3(pos.x, pos.y, pos.z);
    //                // RotatePosition에서는 pos의 x, z를 0.5씩 더하여 중심을 맞춘 후 회전 적용
    //                Vector3Int rotatedPos = RotatePosition(originPos, pivot, angleDiff);

    //                // 변환된 좌표가 현재 배치하려는 위치와 충돌하는지 확인
    //                if (positionToOccupy.Contains(rotatedPos))
    //                {
    //                    return false; // 충돌 발생 → 배치 불가능
    //                }
    //            }
    //            else /// 회전축이 다른 경우
    //            {
    //                // 음수 각도 처리: -360 ~ 360 사이의 값을 0 ~ 360 사이로 변환
    //                if (angleDiff < 0)
    //                {
    //                    angleDiff += 360;  // 음수일 경우 360을 더하여 양수로 변환
    //                }
    //                // 회전 차이에 따라 위치를 변경
    //                Vector3Int offset = Vector3Int.zero;
    //                switch ((int)angleDiff)
    //                {
    //                    case 90:
    //                        offset = new Vector3Int(1, 0, 0); // x가 1 차이
    //                        break;
    //                    case 180:
    //                        offset = new Vector3Int(1, 0, 1); // x와 z가 1씩 차이
    //                        break;
    //                    case 270:
    //                        offset = new Vector3Int(0, 0, 1); // z가 1 차이
    //                        break;
    //                    default:
    //                        // 기본적으로 0도인 경우, 회전 없이 원래 위치
    //                        offset = Vector3Int.zero;
    //                        break;
    //                }

    //                // 기존 오브젝트의 회전 차이에 따른 새로운 위치를 계산
    //                Vector3Int adjustedPos = pos + offset;

    //                // 변환된 좌표가 현재 배치하려는 위치와 충돌하는지 확인
    //                if (positionToOccupy.Contains(adjustedPos))
    //                {
    //                    return false; // 충돌 발생 → 배치 불가능
    //                }
    //            }
    //        }
    //    }
    //    return true;
    //}

    /// <summary>
    /// gridPosition은 preview 오브젝트의 Grid 좌표의 기준점(0도 회전 기준 왼쪽 아래)으로 마우스 커서가 있는 그리드의 좌표이다
    /// objectSize는 preview오브젝트의 크기
    /// angle으로 넘어오는건 TempInputManager.angle으로 회전각
    /// 
    /// 월드좌표로 바꿨다
    /// </summary>
    /// <param name="gridPosition"></param>
    /// <param name="objectSize"></param>
    /// <param name="angle"></param>
    /// <returns></returns>
    public bool CanPlaceObjAt(Vector3Int gridPosition, Vector2Int objectSize, float angle, Transform parent)
    {
        // 부모 기준 로컬 좌표로 변환
        List<Vector3Int> positionToOccupy = CalculatePositions(gridPosition, objectSize, parent);

        // placedObjects에 있는 모든 그리드 좌표를 부모 기준 로컬 좌표로 변환하여 저장
        HashSet<Vector3Int> placedLocalPositions = new HashSet<Vector3Int>();
        foreach (var entry in placedObjects)
        {
            placedLocalPositions.Add(ConvertToLocalGridPosition(entry.Key, parent));
        }

        // 현재 배치하려는 좌표들도 부모 기준 로컬 좌표로 변환하여 충돌 검사
        foreach (var pos in positionToOccupy)
        {
            Vector3Int localPosToCheck = ConvertToLocalGridPosition(pos, parent);

            // 부모 기준 로컬 좌표에서 겹치는지 확인
            if (placedLocalPositions.Contains(localPosToCheck))
            {
                return false; // 충돌 발생 → 배치 불가능
            }
        }

        return true; // 겹침이 없으면 배치 가능
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
    public List<Vector3Int> occupiedPositions;  // 씬에 올려놓은 오브젝트가 점유하고 있는 그리드 좌표들
    public int ID { get; private set; }
    public int PlacedObjectIndex { get; private set; }
    public float angle; // (0, y, 0) 형태로 저장된다
    public Vector3Int gridPosition; // 추가
    public Vector2Int objectSize; // 추가

    public PlacementData(List<Vector3Int> occupiedPositions, int iD, int placedObjectIndex, float angle, Vector3Int gridPosition, Vector2Int objectSize)
    {
        this.occupiedPositions = occupiedPositions;
        ID = iD;
        PlacedObjectIndex = placedObjectIndex;
        this.angle = angle;
        this.gridPosition = gridPosition; // 저장
        this.objectSize = objectSize; // 저장
    }
}