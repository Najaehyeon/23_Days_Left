using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

/// <summary>
/// �ʿ� ���� ���࿡ ������ �׸��� �ý���
/// �׸��� �ý��ۿ��� �� ��: ���� ���ڷ� �����ϱ�, ���ڸ� �����Ͽ� ������ ���๰ ǥ���ϱ�
/// </summary>
public class GridSystem : MonoBehaviour
{
    // ����� �ӽ÷� Cube ������ ����
    public GameObject objToPlace;   // ���� ���๰: �÷��̾ UI���� ������ ���๰�� ���⿡ ����

    public float gridSize = 1f;
    private GameObject ghostObj;    // ghost: �����ϰ� ������ ���๰

    /// ������Ʈ�� ���� �� occupiedPosition�� �߰��ؾ��Ѵ�
    private HashSet<Vector3> occupiedPositions = new HashSet<Vector3>();    // �̹� �����ϰ� �ִ� ��ǥ��

       
    // �׽�Ʈ�� ���� Start
    private void Start()
    {
        CreateGhostObj();
    }
    // �׽�Ʈ�� ���� Update
    private void Update()
    {
        UpdateGhostPosition();

        if (Input.GetMouseButtonDown(0)) 
            PlaceObject();
    }


    // ������ ���๰�� �����
    // ������ ���๰�� ����� ���� �ƴϹǷ� GridSystem�� ���� ���� �����ϴ�
    // ������ ���๰�� ����� ���� BuildingFactory���� �Ѵ�
    public void CreateGhostObj()
    {
        ghostObj = Instantiate(objToPlace);
        // �ٸ� ������Ʈ��� ��ȣ�ۿ��� ���� �ʱ� ���� collider�� ��Ȱ��ȭ
        ghostObj.GetComponent<Collider>().enabled = false;

        // ghost�� ��� �������� �����´�
        Renderer[] renderers = ghostObj.GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in renderers)
        {
            // ������ ó��
            Material mat = renderer.material;
            Color color = mat.color;
            color.a = 0.5f; // ������
            mat.color = color;

            mat.SetFloat("_Mode", 2);   // _Mode�� 2�� ����: ������ ��� Ȱ��ȭ.
            // _ScrBlend, _DstBlend ����: ���� ���� ����
            mat.SetInt("_ScrBlend", (int)BlendMode.SrcAlpha);   // BlendMode.SrcAlpha: �ҽ�(�ڽ�)�� ���� �� ���
            mat.SetInt("_DstBlend", (int)BlendMode.OneMinusSrcAlpha);   // BlendMode.OneMinusSrcAlpha: ���� �ε巴�� ȥ��.
            mat.SetInt("_ZWrite", 0);   // _ZWrite = 0: ���� ���۸� ��Ȱ��ȭ�Ͽ� ���ĵ� �����ϰ� ���̵��� ����.
            mat.DisableKeyword("_ALPHATEST_ON");    // _ALPHATEST_ON ��Ȱ��ȭ: ���� �׽�Ʈ(�ܼ� ����/������ ����) ��� �� ��.
            mat.EnableKeyword("_ALPHABLEND_ON");    // _ALPHABLEND_ON Ȱ��ȭ: ���� ���� ���
            mat.DisableKeyword("_ALPHAPREMULTIPLY_ON"); // _ALPHAPREMULTIPLY_ON ��Ȱ��ȭ: �̸� ������ ���� �� ��� �� ��.

            mat.renderQueue = 3000; /// renderQueue = 3000 ���� �� ������ ������Ʈ�� �������ϵ��� ����.
                                    /// 3000���� Transparent Queue��, ������ ������Ʈ���� ���߿� �׷���.
        }
    }    
    public void UpdateGhostPosition()
    {
        /// ���콺�� ��ǥ(2d screen space) ���� 3d world�� raycast
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        // �ش� �������� object ũ�⸸ŭ grid�� ���Ͽ�
        // occupiedPositions�� �˻��Ͽ�, �̹� ��ü�� �ִ� ���� ghostObj�� ���������� ǥ��, ����ִ� ���� ������� ǥ���Ѵ�
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            Vector3 point = hit.point;  // ���콺 ��ǥ

            // �ش� ���콺 ��ǥ�� ���ԵǾ��ִ� �׸����� ���� �Ʒ������� ������ snappedPos
            Vector3 snappedPos = new Vector3(
                Mathf.Round(point.x / gridSize) * gridSize,
                Mathf.Round(point.y / gridSize) * gridSize,
                Mathf.Round(point.z / gridSize) * gridSize
                );

            // ghost�� �߽��� snappedPos�� �����
            ghostObj.transform.position = snappedPos;

            // �� �ڸ��� ������Ʈ�� �ִ� ��� ������, ���� ��� �ʷϻ� ǥ��
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

    // ����׿����� �׸��� �׷��ش�
    public void DrawGrid()
    {
        // ������ �Ǽ����� �׸��� ǥ��
        

    }

    // ���� 
    public void PlaceObject()
    {
        // ������ �׸��� ���� ���� ��ü�� �ߺ� ��ġ�Ǵ� ���� ����
        Vector3 placementPosition = ghostObj.transform.position;
        if (!occupiedPositions.Contains(placementPosition))
        {
            // �ǹ��� ������ �����տ� ���� �ٸ��ϱ� �ϴ� ����
            Instantiate(objToPlace, placementPosition, Quaternion.identity);    

            // ������Ʈ ��ġ�� ��ġ�� ����
            occupiedPositions.Add(placementPosition);
        }
    }
}
