using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// BuildingManager가 되어야 할 것
/// </summary>
public class PlacementSystem : MonoBehaviour
{
    [SerializeField] private TempInputManager inputManager;   // 마우스
    [SerializeField] private Grid grid;   // 그리드 컴포넌트

    [SerializeField] private ObjectsDatabaseSO database;

    [SerializeField] private GameObject gridVisualization;


    [SerializeField] private AudioClip correctPlacementClip, wrongPlacementClip;
    [SerializeField] private AudioSource source;

    private GridData floorData, furnitureData;


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
    }

    /// <summary>
    /// UI에서 건물을 선택하면 호출되는 이벤트 함수
    /// </summary>
    /// <param name="ID"></param>
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
        Vector3 mousePos = inputManager.GetSelectedMapPos(); // 마우스로 선택한 위치 가져온다
        Vector3Int gridPosition = grid.WorldToCell(mousePos);   // 마우스가 있는 위치를 3d로 변환하여 그리드의 어느 격자 내에 있는지 알아낸다

        // 물체가 회전했다면 회전된 오브젝트의 중심 좌표를 계산하여 위치 보정
        /// 하지만 회전중심은 gridPosition이므로, adjustGridPosition은 gridPosition과 같다
        Vector3Int adjustedGridPosition = AdjustPositionForRotation(gridPosition);

        /// 달라지는 것은 rotation의 y값이다
        /// 이걸 반영해서 격자를 그리고, 오브젝트를 생성해야한다


        /// 오브젝트 생성
        /// preview의 회전상태를 반영해야한다
        buildingState.OnAction(adjustedGridPosition);
    }

    private Vector3Int AdjustPositionForRotation(Vector3Int originalPosition)
    {
        // 현재 배치 중인 오브젝트의 크기 가져오기
        Vector2Int size = preview.GetCurrentSize();
        // 현재 프리뷰의 회전값 가져오기 (Y축 회전)
        int rotation = preview.GetRotationAngle();

        Vector3Int adjustedPosition = originalPosition;

        switch (rotation)
        {
            case 90:
                adjustedPosition.x -= size.y - 1;
                break;
            case 180:
                adjustedPosition.x -= size.x - 1;
                adjustedPosition.z -= size.y - 1;
                break;
            case 270:
                adjustedPosition.z -= size.x - 1;
                break;
        }

        return adjustedPosition;
    }

    /// <summary>
    /// 선택한 건물을 Grid에 내려놓을때 호출된다
    /// 회전을 고려한 위치에 내려놓아야한다(현재는 그렇지 않은 문제가 있다)
    /// </summary>
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

        // 회전된 오브젝트의 위치 보정
        Vector3Int adjustedGridPosition = AdjustPositionForRotation(gridPosition);

        // grid 내에서 커서가 이동하면 연산하지 않는다
        if (lastDetectedPosition != adjustedGridPosition)
        {
            buildingState.UpdateState(adjustedGridPosition);
            lastDetectedPosition = adjustedGridPosition;
        }


        // R키를 누르면 시계방향으로 회전
        if (Input.GetKeyDown(KeyCode.R))
        {
            preview.RotatePreview(90); // 90도 회전
        }
    }
}
