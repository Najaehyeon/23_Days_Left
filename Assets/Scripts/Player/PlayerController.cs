using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Move")]
    public float moveSpeed; // 플레이어 이동 속도
    Vector3 moveDir; // 실제 이동 방향 벡터
    Vector2 currentMoveInput; // 입력된 이동 방향 (WASD)

    [Header("Look")]
    public float sensitivity; // 마우스 감도 (회전 속도 조절)
    float curCamXRot; // 현재 카메라 X축 회전값 (상하)
    float curCamYRot; // 현재 카메라 Y축 회전값 (좌우)
    Vector2 mouseDelta; // 마우스 이동 입력 값

    [Header("Jump")]
    public float jumpPower; // 점프 힘

    public Transform cameraContainer; // 카메라를 붙일 오브젝트
    public Transform playerBody; // 플레이어의 몸체
    public LayerMask groundLayerMask; // 바닥 감지를 위한 레이어 마스크

    Rigidbody _rigidbody; // 플레이어의 물리엔진 (중력, 충돌 처리)
    Animator _animator; // 애니메이션 컨트롤러

    private void Awake()
    {
        // Rigidbody 및 Animator 컴포넌트를 가져와 저장
        _rigidbody = GetComponent<Rigidbody>();
        _animator = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        // 마우스 커서를 화면 중앙에 고정
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void FixedUpdate()
    {
        // 이동 처리 (물리 엔진이 적용됨)
        Move();
    }

    private void LateUpdate()
    {
        // 카메라 회전 처리 (LateUpdate는 프레임마다 실행됨)
        Look();
    }

    /// <summary>
    /// 플레이어 이동 처리
    /// </summary>
    void Move()
    {
        // 카메라의 전방 벡터를 기준으로 이동 방향 결정
        Vector3 camForward = cameraContainer.forward;
        camForward.y = 0; // 수직(y) 방향을 제거하여 지면 기준으로 이동
        camForward.Normalize(); // 크기를 1로 정규화

        // 카메라의 우측 벡터를 기준으로 좌우 이동 방향 결정
        Vector3 camRight = cameraContainer.right;
        camRight.y = 0;
        camRight.Normalize();

        // 입력된 이동 방향을 기반으로 실제 이동 방향을 계산
        moveDir = camForward * currentMoveInput.y + camRight * currentMoveInput.x;
        moveDir *= moveSpeed; // 이동 속도 적용
        moveDir.y = _rigidbody.velocity.y; // y축(중력) 값 유지

        // Rigidbody를 이용해 이동 처리
        _rigidbody.velocity = moveDir;

        // 이동 애니메이션을 적용
        _animator.SetFloat("Speed", _rigidbody.velocity.magnitude);

        // 이동 중이라면 플레이어가 이동 방향을 바라보도록 회전
        if (currentMoveInput != Vector2.zero)
        {
            Turn();
        }
    }

    /// <summary>
    /// 마우스 입력을 이용한 카메라 회전 처리
    /// </summary>
    void Look()
    {
        // 마우스 이동 값을 받아 카메라 회전 값 변경
        curCamXRot += mouseDelta.y * sensitivity;
        curCamYRot += mouseDelta.x * sensitivity;

        // 카메라 회전 적용 (X축 회전은 반대로 적용해야 정상적인 움직임)
        cameraContainer.localEulerAngles = new Vector3(-curCamXRot, curCamYRot, 0);
    }

    /// <summary>
    /// 플레이어가 이동 방향을 바라보도록 회전
    /// </summary>
    void Turn()
    {
        playerBody.forward = moveDir; // 이동 방향으로 회전
        playerBody.localEulerAngles = new Vector3(0, playerBody.localEulerAngles.y, 0); // X, Z축 회전 방지
    }

    /// <summary>
    /// 플레이어가 바닥에 있는지 확인하는 함수
    /// </summary>
    /// <returns>바닥에 있으면 true, 없으면 false</returns>
    bool IsGrounded()
    {
        // 바닥 감지를 위한 4개의 Ray를 생성 (앞, 뒤, 좌, 우)
        Ray[] rays = new Ray[4]
        {
            new Ray(transform.position + (transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (-transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (transform.right * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (-transform.right * 0.2f) + (transform.up * 0.01f), Vector3.down)
        };

        // Ray를 쏴서 바닥이 감지되면 true 반환
        for (int i = 0; i < rays.Length; i++)
        {
            if (Physics.Raycast(rays[i], 0.1f, groundLayerMask))
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// 이동 입력 처리 (WASD)
    /// </summary>
    public void OnMoveInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            // 입력이 들어오면 현재 이동 방향을 저장
            currentMoveInput = context.ReadValue<Vector2>();
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            // 키에서 손을 떼면 이동을 멈춤
            currentMoveInput = Vector2.zero;
        }
    }

    /// <summary>
    /// 마우스 이동 입력 처리
    /// </summary>
    public void OnLookInput(InputAction.CallbackContext context)
    {
        // 마우스 이동 값을 저장 (카메라 회전에 사용)
        mouseDelta = context.ReadValue<Vector2>();
    }

    /// <summary>
    /// 점프 입력 처리 (스페이스바)
    /// </summary>
    public void OnJumpInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && IsGrounded())
        {
            // 점프 시 Rigidbody에 힘을 추가
            _rigidbody.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);

            // 점프 애니메이션 실행
            _animator.SetTrigger("DoJump");
        }
    }
}
