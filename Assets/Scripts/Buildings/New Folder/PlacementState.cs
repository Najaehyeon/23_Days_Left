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
    /// angle으로 넘어오는건 TempInputManager.angle으로 회전각
    /// </summary>
    /// <param name="gridPosition"></param>
    public void OnAction(Vector3Int gridPosition, float angle, Transform parent)
    {
        // PlaceStructure

        /// 영역검사할때도 격자의 회전을 반영해야한다
        bool placementValidity = CheckPlacementValidity(gridPosition, selectedObjIndex, angle, parent);
        if (placementValidity == false)
        {
            soundFeedback.PlaySound(SoundType.wrongPlacement);
            return;
        }
        soundFeedback.PlaySound(SoundType.Place);
        /// 오브젝트 생성(오브젝트 정보는 previewSystem.previewObject가 가지고 있다)
        /// grid 좌표를 world좌표로 환산한 값을 대입하니까 PlaceObject에서 다루는 것은 월드 좌표다
        int index = objectPlacer.PlaceObject(database.objectsData[selectedObjIndex].Prefab, grid.CellToWorld(gridPosition));

        /// preview 오브젝트의 회전상태
        //Quaternion rotation = Quaternion.Euler(0, angle, 0);

        // 이 객체의 인덱스를 데이터에 추가
        GridData selectedData = database.objectsData[selectedObjIndex].ID == 0 ? tree : buildingData;   // 나무와 건물을 구분한다
        selectedData.AddObjectAt(gridPosition,
            database.objectsData[selectedObjIndex].Size,
            database.objectsData[selectedObjIndex].ID,
            index, angle, parent);

        previewSystem.UpdatePosition(grid.CellToWorld(gridPosition), false);
    }

    /// <summary>
    /// preview 오브젝트가 차지하는 그리드의 종류를 판단한다
    /// angle은 TempInputManager.angle 이다
    /// </summary>
    /// <param name="gridPosition"></param>
    /// <param name="selectedObjIndex"></param>
    /// <returns></returns>
    private bool CheckPlacementValidity(Vector3Int gridPosition, int selectedObjIndex, float angle, Transform parent)
    {
        GridData selectedData = database.objectsData[selectedObjIndex].ID == 0 ? tree : buildingData;

        /// gridPosition는 그리드의 좌표
        /// database.objectsData[selectedObjIndex]는 preview 오브젝트
        /// CanPlaceObjAt는 마우스가 있는 gridPosition에 preview오브젝트를 놓을 수 있으면 true를 리턴
        return selectedData.CanPlaceObjAt(gridPosition, database.objectsData[selectedObjIndex].Size, angle, parent);
    }

    /// <summary>
    /// preview 오브젝트가 점유하고 있는 그리드의 영역을 표시한다
    /// angle은 TempInputManager.angle 이다
    /// </summary>
    /// <param name="gridPosition"></param>
    public void UpdateState(Vector3Int gridPosition, float angle, Transform parent)
    {
        /// gridPosition는 그리드의 좌표
        /// selectedObjIndex는 UI에서 선택한 인덱스(preview 오브젝트)
        bool placementValidity = CheckPlacementValidity(gridPosition, selectedObjIndex, angle, parent);

        previewSystem.UpdatePosition(grid.CellToWorld(gridPosition), placementValidity);
    }
}
