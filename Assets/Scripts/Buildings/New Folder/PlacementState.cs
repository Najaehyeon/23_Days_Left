using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.VisualScripting.Member;

/// <summary>
/// ���� ������ �ǹ��� ���� �� �ִٴ� ����
/// </summary>
public class PlacementState : IBuildingState
{
    private int selectedObjIndex = -1;
    int ID;
    Grid grid;
    PreviewSystem previewSystem;
    ObjectsDatabaseSO database;
    GridData tree;  // ���� ������ ������ ���� �� �ִ�
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
            // ������Ʈ �����ߴٸ� �׸���, �̸����� ������Ʈ �����ش�
            previewSystem.StartShowingPlacementPreview(database.objectsData[selectedObjIndex].Prefab,
                                                 database.objectsData[selectedObjIndex].Size);
        }
        else
            throw new System.Exception($"No object with ID {iD}");
    }
    // �߰��� �� �߻�
    public void EndState()
    {
        previewSystem.StopShowingPreview();
    }
    // ���콺 ���� �� �߻�, �ʿ� ��ü�� ��ġ
    public void OnAction(Vector3Int gridPosition)
    {
        // PlaceStructure
        bool placementValidity = CheckPlacementValidity(gridPosition, selectedObjIndex);
        if (placementValidity == false)
        {
            soundFeedback.PlaySound(SoundType.wrongPlacement);
            return;
        }
        soundFeedback.PlaySound(SoundType.Place);
        int index = objectPlacer.PlaceObject(database.objectsData[selectedObjIndex].Prefab, grid.CellToWorld(gridPosition));

        // �� ��ü�� �ε����� �����Ϳ� �߰�
        GridData selectedData = database.objectsData[selectedObjIndex].ID == 0 ? tree : buildingData;   // ������ �ǹ��� �����Ѵ�
        selectedData.AddObjectAt(gridPosition,
            database.objectsData[selectedObjIndex].Size,
            database.objectsData[selectedObjIndex].ID,
            index);

        previewSystem.UpdatePosition(grid.CellToWorld(gridPosition), false);
    }

    private bool CheckPlacementValidity(Vector3Int gridPosition, int selectedObjIndex)
    {
        GridData selectedData = database.objectsData[selectedObjIndex].ID == 0 ? tree : buildingData;
        return selectedData.CanPlaceObjAt(gridPosition, database.objectsData[selectedObjIndex].Size);
    }
    public void UpdateState(Vector3Int gridPosition)
    {
        bool placementValidity = CheckPlacementValidity(gridPosition, selectedObjIndex);

        previewSystem.UpdatePosition(grid.CellToWorld(gridPosition), placementValidity);
    }


}
