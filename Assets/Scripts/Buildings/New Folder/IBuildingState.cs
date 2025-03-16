using UnityEngine;

// 건축 상태 구분
public interface IBuildingState
{
    void EndState();
    void OnAction(Vector3Int gridPosition);
    void UpdateState(Vector3Int gridPosition);
}