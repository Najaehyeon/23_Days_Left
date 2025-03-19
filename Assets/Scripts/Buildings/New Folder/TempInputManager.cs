using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.EventSystems;

public class TempInputManager : MonoBehaviour
{
    [SerializeField] public Camera sceneCamera;    // 3인칭 시점의 메인카메라가 있는데 굳이 필요할까?

    public Vector3 lastPos;

    [SerializeField] private LayerMask placementLayerMask;  // 내려놓을 지점의 layer가 Grid일 때만 내려놓을 수 있다

    public float angle; /// 회전각을 저장

    // 클릭하면 다른 클래스에 알리는 용도
    public event Action OnClicked, OnExit;

    private void Awake()
    {
        
    }

    private void Start()
    {
        // 카메라를 연결하는 것
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
            OnClicked?.Invoke();    // 있으면 호출(안전하게 이벤트 호출)
        if (Input.GetKeyDown(KeyCode.Escape))
            OnExit?.Invoke();
    }
    public bool IsPointerOverUi() => EventSystem.current.IsPointerOverGameObject(); // 마우스가 UI위에 있는지 확인해서 있으면 true, 없으면 false


    //
    public Vector3 GetSelectedMapPos()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = sceneCamera.nearClipPlane; // 카메라에서 렌더링 되지 않은 물체를 선택할 수 없게 만듦
        Ray ray = sceneCamera.ScreenPointToRay(mousePos);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100, placementLayerMask))
        {
            lastPos = hit.point;
        }
        return lastPos;
    }
}
