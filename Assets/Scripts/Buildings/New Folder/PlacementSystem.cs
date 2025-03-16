using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// BuildingManager�� �Ǿ�� �� ��
/// </summary>
public class PlacementSystem : MonoBehaviour
{
    //[SerializeField] private GameObject mouseIndicator; // �����ϴ� ��ġ�� �ð�ȭ(���� Sphere ������ �����ؼ� Ŀ�� ��ġ�� �����ش�)
    //[SerializeField] private GameObject cellIndicator; // ���콺�� �ش� ���� �ȿ� ������ ǥ��(���콺 Ŀ���� �����̵�)
    [SerializeField] private TempInputManager inputManager;   // ���콺
    [SerializeField] private Grid grid;   // �׸��� ������Ʈ

    [SerializeField] private ObjectsDatabaseSO database;
    //private int selectedObjIndex = -1;   // -1: ������Ʈ�� ���õ��� �ʾҴ�

    [SerializeField] private GameObject gridVisualization;


    [SerializeField] private AudioClip correctPlacementClip, wrongPlacementClip;
    [SerializeField] private AudioSource source;

    private GridData floorData, furnitureData;

    //private Renderer previewRenderer;

    //private List<GameObject> placedGameObjects = new List<GameObject>(); // ������ ���ӿ�����Ʈ ����

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
        // Ŀ���� UI���� ���� �ʴٸ�(�׸��� ���� �ִٸ�)
        Vector3 mousePos = inputManager.GetSelectedMapPos(); // ���콺�� ������ ��ġ �����´�
        Vector3Int gridPosition = grid.WorldToCell(mousePos);   // ���콺�� �ִ� ��ġ�� 3d�� ��ȯ�Ͽ� �׸����� ��� ���� ���� �ִ��� �˾Ƴ���

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
        Vector3 mousePos = inputManager.GetSelectedMapPos(); // ���콺�� ������ ��ġ �����´�
        Vector3Int gridPosition = grid.WorldToCell(mousePos);   // ���콺�� �ִ� ��ġ�� 3d�� ��ȯ�Ͽ� �׸����� ��� ���� ���� �ִ��� �˾Ƴ���

        // grid ������ Ŀ���� �̵��ϸ� �������� �ʴ´�
        if (lastDetectedPosition != gridPosition)
        {
            //bool placementValidity = CheckPlacementValidity(gridPosition, selectedObjIndex);
            ////previewRenderer.material.color = placementValidity ? Color.green : Color.red;

            //mouseIndicator.transform.position = mousePos;
            ////cellIndicator.transform.position = grid.CellToWorld(gridPosition); // ���� ������Ʈ�� �� ��ǥ�� �̵�
            //preview.UpdatePosition(grid.CellToWorld(gridPosition), placementValidity);
            buildingState.UpdateState(gridPosition);

            lastDetectedPosition = gridPosition;
        }
    }
}
