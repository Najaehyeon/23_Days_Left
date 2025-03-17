using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.EventSystems;

public class TempInputManager : MonoBehaviour
{
    [SerializeField] private Camera sceneCamera;    // 3��Ī ������ ����ī�޶� �ִµ� ���� �ʿ��ұ�?

    public Vector3 lastPos;

    [SerializeField] private LayerMask placementLayerMask;  // �������� ������ layer�� Grid�� ���� �������� �� �ִ�

    // Ŭ���ϸ� �ٸ� Ŭ������ �˸��� �뵵
    public event Action OnClicked, OnExit;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
            OnClicked?.Invoke();    // ������ ȣ��(�����ϰ� �̺�Ʈ ȣ��)
        if (Input.GetKeyDown(KeyCode.Escape))
            OnExit?.Invoke();
    }
    public bool IsPointerOverUi() => EventSystem.current.IsPointerOverGameObject(); // ���콺�� UI���� �ִ��� Ȯ���ؼ� ������ true, ������ false


    //
    public Vector3 GetSelectedMapPos()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = sceneCamera.nearClipPlane; // ī�޶󿡼� ������ ���� ���� ��ü�� ������ �� ���� ����
        Ray ray = sceneCamera.ScreenPointToRay(mousePos);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100, placementLayerMask))
        {
            lastPos = hit.point;
        }
        return lastPos;
    }
}
