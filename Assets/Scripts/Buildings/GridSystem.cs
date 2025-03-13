using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

/// <summary>
/// 맵에 붙일 건축에 적용할 그리드 시스템
/// 그리드 시스템에서 할 일: 맵을 격자로 구분하기, 격자를 조사하여 투명한 건축물 표시하기
/// </summary>
public class GridSystem : MonoBehaviour
{
    // 현재는 임시로 Cube 프리팹 연결
    public GameObject objToPlace;   // 지을 건축물: 플레이어가 UI에서 선택한 건축물이 여기에 저장

    public float gridSize = 1f;
    private GameObject ghostObj;    // ghost: 투명하게 보여줄 건축물

    /// 오브젝트를 놓을 때 occupiedPosition에 추가해야한다
    private HashSet<Vector3> occupiedPositions = new HashSet<Vector3>();    // 이미 점유하고 있는 좌표들

       
    // 테스트를 위한 Start
    private void Start()
    {
        CreateGhostObj();
    }
    // 테스트를 위한 Update
    private void Update()
    {
        UpdateGhostPosition();

        if (Input.GetMouseButtonDown(0)) 
            PlaceObject();
    }


    // 투명한 건축물을 만든다
    // 실제로 건축물을 만드는 것이 아니므로 GridSystem에 놓는 것이 적당하다
    // 실제로 건축물을 만드는 것을 BuildingFactory에서 한다
    public void CreateGhostObj()
    {
        ghostObj = Instantiate(objToPlace);
        // 다른 오브젝트들과 상호작용을 하지 않기 위해 collider를 비활성화
        ghostObj.GetComponent<Collider>().enabled = false;

        // ghost의 모든 렌더러를 가져온다
        Renderer[] renderers = ghostObj.GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in renderers)
        {
            // 반투명 처리
            Material mat = renderer.material;
            Color color = mat.color;
            color.a = 0.5f; // 반투명
            mat.color = color;

            mat.SetFloat("_Mode", 2);   // _Mode를 2로 설정: 반투명 모드 활성화.
            // _ScrBlend, _DstBlend 설정: 알파 블렌딩 적용
            mat.SetInt("_ScrBlend", (int)BlendMode.SrcAlpha);   // BlendMode.SrcAlpha: 소스(자신)의 알파 값 사용
            mat.SetInt("_DstBlend", (int)BlendMode.OneMinusSrcAlpha);   // BlendMode.OneMinusSrcAlpha: 배경과 부드럽게 혼합.
            mat.SetInt("_ZWrite", 0);   // _ZWrite = 0: 깊이 버퍼를 비활성화하여 겹쳐도 투명하게 보이도록 설정.
            mat.DisableKeyword("_ALPHATEST_ON");    // _ALPHATEST_ON 비활성화: 알파 테스트(단순 투명/불투명 구분) 사용 안 함.
            mat.EnableKeyword("_ALPHABLEND_ON");    // _ALPHABLEND_ON 활성화: 알파 블렌딩 사용
            mat.DisableKeyword("_ALPHAPREMULTIPLY_ON"); // _ALPHAPREMULTIPLY_ON 비활성화: 미리 곱해진 알파 값 사용 안 함.

            mat.renderQueue = 3000; /// renderQueue = 3000 설정 → 투명한 오브젝트로 렌더링하도록 지정.
                                    /// 3000번은 Transparent Queue로, 불투명 오브젝트보다 나중에 그려짐.
        }
    }    
    public void UpdateGhostPosition()
    {
        /// 마우스의 좌표(2d screen space) 에서 3d world로 raycast
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        // 해당 지점에서 object 크기만큼 grid를 비교하여
        // occupiedPositions을 검사하여, 이미 물체가 있는 곳은 ghostObj을 빨간색으로 표시, 비어있는 곳은 녹색으로 표시한다
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            Vector3 point = hit.point;  // 마우스 좌표

            // 해당 마우스 좌표가 포함되어있는 그리드의 왼쪽 아래지점을 기준점 snappedPos
            Vector3 snappedPos = new Vector3(
                Mathf.Round(point.x / gridSize) * gridSize,
                Mathf.Round(point.y / gridSize) * gridSize,
                Mathf.Round(point.z / gridSize) * gridSize
                );

            // ghost의 중심을 snappedPos에 맞춘다
            ghostObj.transform.position = snappedPos;

            // 이 자리에 오브젝트가 있는 경우 빨간색, 없는 경우 초록색 표시
            if (occupiedPositions.Contains(snappedPos))
                SetGhostColor(new Color(1f, 0f, 0f, 0.5f));
            else
                SetGhostColor(new Color(0f, 1f, 0f, 0.5f));
        }
    }
    void SetGhostColor(Color color)
    {
        Renderer[] renderers = ghostObj.GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in renderers)
        {
            Material mat = renderer.material;
            mat.color = color;
        }
    }

    // 디버그용으로 그리드 그려준다
    public void DrawGrid()
    {
        // 검은색 실선으로 그리드 표시
        

    }

    // 실제 
    public void PlaceObject()
    {
        // 동일한 그리드 셀에 여러 객체가 중복 배치되는 것을 방지
        Vector3 placementPosition = ghostObj.transform.position;
        if (!occupiedPositions.Contains(placementPosition))
        {
            // 건물의 방향은 프리팹에 따라 다르니까 일단 보류
            Instantiate(objToPlace, placementPosition, Quaternion.identity);    

            // 오브젝트 배치한 위치를 저장
            occupiedPositions.Add(placementPosition);
        }
    }
}
