using UnityEngine;

public class PreviewSystem : MonoBehaviour
{
    [SerializeField] private float previewYOffset = 0.06f;  // 오브젝트는 그리드보다 약간 위에 있다

    [SerializeField] private GameObject cellIndicator;
    private GameObject previewObject;   // 생성 전 미리 보여줄 오브젝트

    [SerializeField] private Material previewMaterialPrefab;    // 투명상태를 나타내는 material
    private Material previewMaterialInstance;   // 생성한 오브젝트의 원래 material

    private Renderer cellIndicatorRenderer;


    private void Start()
    {
        previewMaterialInstance = new Material(previewMaterialPrefab);
        cellIndicator.SetActive(false);
        cellIndicatorRenderer = cellIndicator.GetComponentInChildren<Renderer>();

    }

    public void StartShowingPlacementPreview(GameObject prefab, Vector2Int size)
    {
        previewObject = Instantiate(prefab);
        PreparePreview(previewObject);
        PrepareCursor(size);
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

}
