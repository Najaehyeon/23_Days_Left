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

    private float currentRotationAngle = 0f; // 현재 회전 상태 저장

    public TempInputManager tempInputManager;   // 회전각을 저장


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
        // 현재 프리뷰 크기 가져오기
        Vector2Int size = GetCurrentSize();

        // 중심점을 기준으로 회전하도록 변경
        Vector3 pivot = previewObject.transform.position + new Vector3(size.x / 2f, 0, size.y / 2f);

        // 1️ 회전 중심으로 이동
        previewObject.transform.position -= pivot;

        // 2️ 회전 수행
        previewObject.transform.Rotate(Vector3.up, angle);

        /// 회전각을 "누적하여" 저장(왜냐하면 누를때마다 angle에는 90만 들어올 수 있으므로)
        currentRotationAngle = (currentRotationAngle + angle) % 360;
        tempInputManager.angle = currentRotationAngle;

        // 3️ 다시 원래 위치로 이동
        previewObject.transform.position += pivot;


        /// cellIndicator는 실제로 회전하지 않지만, 크기를 조정해야 한다    
        /// 회전하면 회전한 것의 기준으로 size.x, size.y가 갱신된다
        /// scale은 양수여야 한다
        /// 처음에 size.x == 1, size.y == 2 였다면
        /// 90도 회전한 뒤에는 size.x == 2, size.y ==1 이 되는 것이다
        /// 
        /// position을 부호 고려해서 바꾸면 된다
        /// x = 1 y = 2
        /// x = 2 y = -1 (90도 회전)
        /// x = -1 y = -2 (180도 회전)
        /// x = -2 y = -1 (270도 회전)
        /// x에는 y가 들어가며 y는 -x가 들어간다

        /// scale은 양수
        /// size는 처음부터 양수니까 절댓값 필요없다
        int newX = size.y; 
        int newY = size.x;
        cellIndicator.transform.localScale = new Vector3(newX, 1, newY);

        /// position은 부호 반영
        Vector3 newPosition = cellIndicator.transform.position;
        newPosition.x += (Mathf.Sign(size.y) - 1) * 0.5f * newX;
        newPosition.z += (Mathf.Sign(size.x) - 1) * 0.5f * newY;
        cellIndicator.transform.position = newPosition;
    }

    // 현재 회전 각도를 가져오는 메서드 추가
    public int GetRotationAngle()
    {
        return Mathf.RoundToInt(currentRotationAngle);
    }
}
