using UnityEngine;

/// <summary>
/// 미리 보여주는 격자, 오브젝트
/// 격자의 회전 처리는 여기서 해야된다
/// </summary>
public class PreviewSystem : MonoBehaviour
{
    [SerializeField] private float previewYOffset = 0.06f;  // 오브젝트는 그리드보다 약간 위에 있다

    [SerializeField] private GameObject cellIndicator;  // 생성 전에 오브젝트가 걸치고 있는 격자
    public GameObject previewObject;   // 생성 전 미리 보여줄 오브젝트

    [SerializeField] private Material previewMaterialPrefab;    // 투명상태를 나타내는 material
    private Material previewMaterialInstance;   // 생성한 오브젝트의 원래 material

    private Renderer cellIndicatorRenderer;


    private void Start()
    {
        previewMaterialInstance = new Material(previewMaterialPrefab);
        cellIndicator.SetActive(false);
        cellIndicatorRenderer = cellIndicator.GetComponentInChildren<Renderer>();

    }

    /// <summary>
    /// UI버튼을 클릭할때 미리보기를 처음 표시한다
    /// </summary>
    /// <param name="prefab"></param>
    /// <param name="size"></param>
    public void StartShowingPlacementPreview(GameObject prefab, Vector2Int size)
    {
        previewObject = Instantiate(prefab);
        PreparePreview(previewObject);
        PrepareCursor(size);
        /// 격자를 보여준다
        cellIndicator.SetActive(true);
    }
    // 오브젝트의 크기를 보여준다
    private void PrepareCursor(Vector2Int size)
    {
        if (size.x > 0 || size.y > 0)
        {
            cellIndicator.transform.localScale = new Vector3(size.x, 1, size.y);
            cellIndicatorRenderer.material.mainTextureScale = size;
        }
    }
    // 선택한 오브젝트의 크기를 리턴한다
    public Vector2Int GetCurrentSize()
    {
        if (previewObject == null) 
            return Vector2Int.zero;

        Renderer renderer = previewObject.GetComponentInChildren<Renderer>();
        if (renderer != null)
        {
            Vector3 size = renderer.bounds.size;
            return new Vector2Int(Mathf.CeilToInt(size.x), Mathf.CeilToInt(size.z));
        }
        return Vector2Int.zero;
    }
    private void PreparePreview(GameObject previewObject)
    {
        Renderer[] renderers = previewObject.GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in renderers)
        {
            Material[] materials = renderer.materials;
            for (int i = 0; i < materials.Length; i++)
            {
                materials[i] = previewMaterialInstance;
            }
            renderer.materials = materials;
        }
    }
    public void StopShowingPreview()
    {
        cellIndicator.SetActive(false);
        if (previewObject != null)
            Destroy(previewObject);
    }
    public void UpdatePosition(Vector3 position, bool validity)
    {
        if (previewObject != null)
        {
            MovePreview(position);
            ApplyFeedbackToPreview(validity);
        }
        // cursor는 null이 아니므로
        MoveCursor(position);
        ApplyFeedbackToCursor(validity);

    }

    // 유효한 영역이 아니라면 preview를 빨간색으로 바꾼다
    // 유효한 영역이 아니라면 격자를 빨간색으로 바꾼다
    private void ApplyFeedbackToPreview(bool validity)
    {
        Color c = validity ? Color.green : Color.red;
        c.a = 0.5f;
        //cellIndicatorRenderer.material.color = c;
        previewMaterialInstance.color = c;
    }
    private void ApplyFeedbackToCursor(bool validity)
    {
        Color c = validity ? Color.green : Color.red;
        c.a = 0.5f;
        cellIndicatorRenderer.material.color = c;
    }
    private void MoveCursor(Vector3 position)
    {
        cellIndicator.transform.position = position;
    }
    private void MovePreview(Vector3 position)
    {
        previewObject.transform.position = new Vector3(
            position.x,
            position.y + previewYOffset,
            position.z);
    }

    /// <summary>
    /// R키 누르면 y축 기준으로 시계방향 회전
    /// 격자, 프리팹 모두 회전한다
    /// </summary>
    /// <param name="angle"></param>
    public void RotatePreview(float angle)
    {
        previewObject.transform.Rotate(Vector3.up, angle);
        /// 격자 다시 표시해야함
        /// cellIndicator 또한 angle만큼 회전한다
        cellIndicator.transform.Rotate(Vector3.up, angle);
    }

}
