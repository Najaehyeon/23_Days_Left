using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// 매 프레임마다 preview 오브젝트와 grid 격자를 표시한다
/// </summary>
public class PlacementSystem : MonoBehaviour
{
    [SerializeField] private TempInputManager tempInputManager;   // 마우스
    [SerializeField] private Grid grid;   // 그리드 컴포넌트

    [SerializeField] private ObjectsDatabaseSO database;

    [SerializeField] private GameObject gridVisualization;


    [SerializeField] private AudioClip correctPlacementClip, wrongPlacementClip;
    [SerializeField] private AudioSource source;

    private GridData floorData, furnitureData;

    /// <summary>
    /// 미리 보여주는 preview 오브젝트
    /// </summary>
    [SerializeField] private PreviewSystem preview;



    private Vector3Int lastDetectedPosition = Vector3Int.zero;

    [SerializeField] ObjectPlacer objectPlacer;

    /// <summary>
    /// PlacementState에 접근가능한 IBuildingState 인터페이스
    /// </summary>
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

        tempInputManager.OnClicked += PlaceStructure;
        tempInputManager.OnExit += StopPlacement;

    }

    private void PlaceStructure()
    {
        if (tempInputManager.IsPointerOverUi())
        {
            return;
        }
        Vector3 mousePos = tempInputManager.GetSelectedMapPos(); // 마우스로 선택한 위치 가져온다
        Vector3Int gridPosition = grid.WorldToCell(mousePos);   // 마우스가 있는 위치를 3d로 변환하여 그리드의 어느 격자 내에 있는지 알아낸다

        /// 오브젝트 생성
        /// preview의 회전상태를 반영해야한다
        buildingState.OnAction(gridPosition, tempInputManager.angle);
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

        tempInputManager.OnClicked -= PlaceStructure;
        tempInputManager.OnExit -= StopPlacement;
        lastDetectedPosition = Vector3Int.zero;
        buildingState = null;
    }

    private void Update()
    {
        //if (selectedObjIndex < 0)
        if (buildingState == null)
            return;
        Vector3 mousePos = tempInputManager.GetSelectedMapPos(); // 마우스로 선택한 위치 가져온다
        Vector3Int gridPosition = grid.WorldToCell(mousePos);   // 마우스가 있는 위치를 3d로 변환하여 그리드의 어느 격자 내에 있는지 알아낸다
        /// gridPosition은 grid의 좌표(월드 좌표가 아니다)

        // grid 내에서 커서가 이동하면 연산하지 않는다
        if (lastDetectedPosition != gridPosition)
        {
            buildingState.UpdateState(gridPosition, tempInputManager.angle);    /// gridPosition은 월드좌표가 아님에 주의한다
            lastDetectedPosition = gridPosition;
        }
        // R키를 누르면 시계방향으로 회전
        if (Input.GetKeyDown(KeyCode.R))
        {
            preview.RotatePreview(90); // 90도 회전
        }
    }
}
