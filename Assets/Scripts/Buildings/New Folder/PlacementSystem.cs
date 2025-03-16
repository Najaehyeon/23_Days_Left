using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// BuildingManager가 되어야 할 것
/// </summary>
public class PlacementSystem : MonoBehaviour
{
    //[SerializeField] private GameObject mouseIndicator; // 선택하는 위치를 시각화(작은 Sphere 프리팹 연결해서 커서 위치를 보여준다)
    //[SerializeField] private GameObject cellIndicator; // 마우스가 해당 격자 안에 있음을 표시(마우스 커서를 따라이동)
    [SerializeField] private TempInputManager inputManager;   // 마우스
    [SerializeField] private Grid grid;   // 그리드 컴포넌트

    [SerializeField] private ObjectsDatabaseSO database;
    //private int selectedObjIndex = -1;   // -1: 오브젝트가 선택되지 않았다

    [SerializeField] private GameObject gridVisualization;


    [SerializeField] private AudioClip correctPlacementClip, wrongPlacementClip;
    [SerializeField] private AudioSource source;

    private GridData floorData, furnitureData;

    //private Renderer previewRenderer;

    //private List<GameObject> placedGameObjects = new List<GameObject>(); // 생성된 게임오브젝트 저장

    [SerializeField] private PreviewSystem preview;

    private Vector3Int lastDetectedPosition = Vector3Int.zero;

    [SerializeField] ObjectPlacer objectPlacer;

    IBuildingState buildingState;

    [SerializeField] private SoundFeedBack soundFeedback;


    private void Start()
    {
        StopPlacement();
        floorData = new GridData();
        furnitureData = new GridData();

        //previewRenderer = cellIndicator.GetComponentInChildren<Renderer>();


    }
    public void StartPlacement(int ID)
    {
        StopPlacement();
        gridVisualization.SetActive(true);
        buildingState = new PlacementState(ID,
                                           grid,
                                           preview,
                                           database,
                                           floorData,
                                           furnitureData,
                                           objectPlacer,
                                           soundFeedback);

        inputManager.OnClicked += PlaceStructure;
        inputManager.OnExit += StopPlacement;

    }

    private void PlaceStructure()
    {
        if (inputManager.IsPointerOverUi())
        {
            return;
        }
        // 커서가 UI위에 있지 않다면(그리드 위에 있다면)
        Vector3 mousePos = inputManager.GetSelectedMapPos(); // 마우스로 선택한 위치 가져온다
        Vector3Int gridPosition = grid.WorldToCell(mousePos);   // 마우스가 있는 위치를 3d로 변환하여 그리드의 어느 격자 내에 있는지 알아낸다

        buildingState.OnAction(gridPosition);
    }

    //private bool CheckPlacementValidity(Vector3Int gridPosition, int selectedObjIndex)
    //{
    //    GridData selectedData = database.objectsData[selectedObjIndex].ID == 0 ? floorData : furnitureData;
    //    return selectedData.CanPlaceObjAt(gridPosition, database.objectsData[selectedObjIndex].Size);
    //}

    private void StopPlacement()
    {
        soundFeedback.PlaySound(SoundType.Click);
        if (buildingState == null)
            return;
        //selectedObjIndex = -1;
        gridVisualization.SetActive(false);
        //cellIndicator.SetActive(false);
        //preview.StopShowingPreview();
        buildingState.EndState();

        inputManager.OnClicked -= PlaceStructure;
        inputManager.OnExit -= StopPlacement;
        lastDetectedPosition = Vector3Int.zero;
        buildingState = null;
    }

    private void Update()
    {
        //if (selectedObjIndex < 0)
        if (buildingState == null)
            return;
        Vector3 mousePos = inputManager.GetSelectedMapPos(); // 마우스로 선택한 위치 가져온다
        Vector3Int gridPosition = grid.WorldToCell(mousePos);   // 마우스가 있는 위치를 3d로 변환하여 그리드의 어느 격자 내에 있는지 알아낸다

        // grid 내에서 커서가 이동하면 연산하지 않는다
        if (lastDetectedPosition != gridPosition)
        {
            //bool placementValidity = CheckPlacementValidity(gridPosition, selectedObjIndex);
            ////previewRenderer.material.color = placementValidity ? Color.green : Color.red;

            //mouseIndicator.transform.position = mousePos;
            ////cellIndicator.transform.position = grid.CellToWorld(gridPosition); // 격자 오브젝트를 그 좌표로 이동
            //preview.UpdatePosition(grid.CellToWorld(gridPosition), placementValidity);
            buildingState.UpdateState(gridPosition);

            lastDetectedPosition = gridPosition;
        }
    }
}
