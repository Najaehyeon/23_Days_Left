using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Move")]
    public float moveSpeed; // �÷��̾� �̵� �ӵ�
    Vector3 moveDir; // ���� �̵� ���� ����
    Vector2 currentMoveInput; // �Էµ� �̵� ���� (WASD)

    [Header("Look")]
    public float sensitivity; // ���콺 ���� (ȸ�� �ӵ� ����)
    float curCamXRot; // ���� ī�޶� X�� ȸ���� (����)
    float curCamYRot; // ���� ī�޶� Y�� ȸ���� (�¿�)
    Vector2 mouseDelta; // ���콺 �̵� �Է� ��

    [Header("Jump")]
    public float jumpPower; // ���� ��

    public Transform cameraContainer; // ī�޶� ���� ������Ʈ
    public Transform playerBody; // �÷��̾��� ��ü
    public LayerMask groundLayerMask; // �ٴ� ������ ���� ���̾� ����ũ

    Rigidbody _rigidbody; // �÷��̾��� �������� (�߷�, �浹 ó��)
    Animator _animator; // �ִϸ��̼� ��Ʈ�ѷ�

    private void Awake()
    {
        // Rigidbody �� Animator ������Ʈ�� ������ ����
        _rigidbody = GetComponent<Rigidbody>();
        _animator = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        // ���콺 Ŀ���� ȭ�� �߾ӿ� ����
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void FixedUpdate()
    {
        // �̵� ó�� (���� ������ �����)
        Move();
    }

    private void LateUpdate()
    {
        // ī�޶� ȸ�� ó�� (LateUpdate�� �����Ӹ��� �����)
        Look();
    }

    /// <summary>
    /// �÷��̾� �̵� ó��
    /// </summary>
    void Move()
    {
        // ī�޶��� ���� ���͸� �������� �̵� ���� ����
        Vector3 camForward = cameraContainer.forward;
        camForward.y = 0; // ����(y) ������ �����Ͽ� ���� �������� �̵�
        camForward.Normalize(); // ũ�⸦ 1�� ����ȭ

        // ī�޶��� ���� ���͸� �������� �¿� �̵� ���� ����
        Vector3 camRight = cameraContainer.right;
        camRight.y = 0;
        camRight.Normalize();

        // �Էµ� �̵� ������ ������� ���� �̵� ������ ���
        moveDir = camForward * currentMoveInput.y + camRight * currentMoveInput.x;
        moveDir *= moveSpeed; // �̵� �ӵ� ����
        moveDir.y = _rigidbody.velocity.y; // y��(�߷�) �� ����

        // Rigidbody�� �̿��� �̵� ó��
        _rigidbody.velocity = moveDir;

        // �̵� �ִϸ��̼��� ����
        _animator.SetFloat("Speed", _rigidbody.velocity.magnitude);

        // �̵� ���̶�� �÷��̾ �̵� ������ �ٶ󺸵��� ȸ��
        if (currentMoveInput != Vector2.zero)
        {
            Turn();
        }
    }

    /// <summary>
    /// ���콺 �Է��� �̿��� ī�޶� ȸ�� ó��
    /// </summary>
    void Look()
    {
        // ���콺 �̵� ���� �޾� ī�޶� ȸ�� �� ����
        curCamXRot += mouseDelta.y * sensitivity;
        curCamYRot += mouseDelta.x * sensitivity;

        // ī�޶� ȸ�� ���� (X�� ȸ���� �ݴ�� �����ؾ� �������� ������)
        cameraContainer.localEulerAngles = new Vector3(-curCamXRot, curCamYRot, 0);
    }

    /// <summary>
    /// �÷��̾ �̵� ������ �ٶ󺸵��� ȸ��
    /// </summary>
    void Turn()
    {
        playerBody.forward = moveDir; // �̵� �������� ȸ��
        playerBody.localEulerAngles = new Vector3(0, playerBody.localEulerAngles.y, 0); // X, Z�� ȸ�� ����
    }

    /// <summary>
    /// �÷��̾ �ٴڿ� �ִ��� Ȯ���ϴ� �Լ�
    /// </summary>
    /// <returns>�ٴڿ� ������ true, ������ false</returns>
    bool IsGrounded()
    {
        // �ٴ� ������ ���� 4���� Ray�� ���� (��, ��, ��, ��)
        Ray[] rays = new Ray[4]
        {
            new Ray(transform.position + (transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (-transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (transform.right * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (-transform.right * 0.2f) + (transform.up * 0.01f), Vector3.down)
        };

        // Ray�� ���� �ٴ��� �����Ǹ� true ��ȯ
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
    /// �̵� �Է� ó�� (WASD)
    /// </summary>
    public void OnMoveInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            // �Է��� ������ ���� �̵� ������ ����
            currentMoveInput = context.ReadValue<Vector2>();
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            // Ű���� ���� ���� �̵��� ����
            currentMoveInput = Vector2.zero;
        }
    }

    /// <summary>
    /// ���콺 �̵� �Է� ó��
    /// </summary>
    public void OnLookInput(InputAction.CallbackContext context)
    {
        // ���콺 �̵� ���� ���� (ī�޶� ȸ���� ���)
        mouseDelta = context.ReadValue<Vector2>();
    }

    /// <summary>
    /// ���� �Է� ó�� (�����̽���)
    /// </summary>
    public void OnJumpInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && IsGrounded())
        {
            // ���� �� Rigidbody�� ���� �߰�
            _rigidbody.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);

            // ���� �ִϸ��̼� ����
            _animator.SetTrigger("DoJump");
        }
    }
}
