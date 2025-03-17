using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.VisualScripting.Member;

/// <summary>
/// 나무 위에는 건물을 지을 수 있다는 설정
/// </summary>
public class PlacementState : IBuildingState
{
    private int selectedObjIndex = -1;
    int ID;
    Grid grid;
    PreviewSystem previewSystem;
    ObjectsDatabaseSO database;
    GridData tree;  // 나무 위에는 빌딩을 지을 수 있다
    GridData buildingData;
    ObjectPlacer objectPlacer;
    SoundFeedBack soundFeedback;

    public PlacementState(int iD,
                          Grid grid,
                          PreviewSystem previewSystem,
                          ObjectsDatabaseSO database,
                          GridData treeData,
                          GridData buildingData,
                          ObjectPlacer objectPlacer,
                          SoundFeedBack soundFeedback)
    {
        ID = iD;
        this.grid = grid;
        this.previewSystem = previewSystem;
        this.database = database;
        this.tree = treeData;
        this.buildingData = buildingData;
        this.objectPlacer = objectPlacer;
        this.soundFeedback = soundFeedback;


        selectedObjIndex = database.objectsData.FindIndex(data => data.ID == ID);
        if (selectedObjIndex > -1)
        {
            // 오브젝트 선택했다면 그리드, 미리보기 오브젝트 보여준다
            previewSystem.StartShowingPlacementPreview(database.objectsData[selectedObjIndex].Prefab,
                                                 database.objectsData[selectedObjIndex].Size);
        }
        else
            throw new System.Exception($"No object with ID {iD}");
    }
    // 추가할 때 발생
    public void EndState()
    {
        previewSystem.StopShowingPreview();
    }
    /// <summary>
    /// 마우스 누를 때 발생, 맵에 개체를 배치
    /// 오브젝트를 내려놓는 위치를 mousePosition에서, preview의 position으로  수정
    /// 또한, preview의 회전상태를 반영하여야한다
    /// </summary>
    /// <param name="gridPosition"></param>
    public void OnAction(Vector3Int gridPosition)
    {
        // PlaceStructure

        /// 영역검사할때도 격자의 회전을 반영해야한다
        bool placementValidity = CheckPlacementValidity(gridPosition, selectedObjIndex);
        if (placementValidity == false)
        {
            soundFeedback.PlaySound(SoundType.wrongPlacement);
            return;
        }
        soundFeedback.PlaySound(SoundType.Place);
        /// 오브젝트 생성(오브젝트 정보는 previewSystem.previewObject가 가지고 있다)
        int index = objectPlacer.PlaceObject(database.objectsData[selectedObjIndex].Prefab, grid.CellToWorld(gridPosition));

        // 이 객체의 인덱스를 데이터에 추가
        GridData selectedData = database.objectsData[selectedObjIndex].ID == 0 ? tree : buildingData;   // 나무와 건물을 구분한다
        selectedData.AddObjectAt(gridPosition,
            database.objectsData[selectedObjIndex].Size,
            database.objectsData[selectedObjIndex].ID,
            index);

        previewSystem.UpdatePosition(grid.CellToWorld(gridPosition), false);
    }

    /// <summary>
    /// 격자 검사할때 회전으로 격자의 위치가 마우스 위치와 달라진 것을 반영해야한다
    /// </summary>
    /// <param name="gridPosition"></param>
    /// <param name="selectedObjIndex"></param>
    /// <returns></returns>
    private bool CheckPlacementValidity(Vector3Int gridPosition, int selectedObjIndex)
    {
        GridData selectedData = database.objectsData[selectedObjIndex].ID == 0 ? tree : buildingData;
        return selectedData.CanPlaceObjAt(gridPosition, database.objectsData[selectedObjIndex].Size);
    }

    /// <summary>
    /// 오브젝트가 점유하고 있는 그리드의 영역을 표시한다
    /// </summary>
    /// <param name="gridPosition"></param>
    public void UpdateState(Vector3Int gridPosition)
    {
        bool placementValidity = CheckPlacementValidity(gridPosition, selectedObjIndex);

        previewSystem.UpdatePosition(grid.CellToWorld(gridPosition), placementValidity);
    }


}
