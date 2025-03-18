using DaysLeft.Menu;
using DaysLeft.Inventory;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    Rigidbody _rigidbody; // �÷��̾��� �������� (�߷�, �浹 ó��)
    Animator _animator; // �ִϸ��̼� ��Ʈ�ѷ�

    PlayerInventory inventory;
    public int itemId = -1;
    public GameObject gatheringObject;
    public Action OnPlayerDead;

    [Header("Other Controller")]
    [SerializeField]
    private InventoryUIController InventoryUIController;

    [Header("Move")]
    public float moveSpeed; // �÷��̾� �̵� �ӵ�
    Vector3 moveDir; // ���� �̵� ���� ����
    Vector2 currentMoveInput; // �Էµ� �̵� ���� (WASD)
    public bool isAttack = false;
    public bool isGathering = false;

    [Header("Look")]
    public float sensitivity; // ���콺 ���� (ȸ�� �ӵ� ����)
    float curCamXRot; // ���� ī�޶� X�� ȸ���� (����)
    float curCamYRot; // ���� ī�޶� Y�� ȸ���� (�¿�)
    Vector2 mouseDelta; // ���콺 �̵� �Է� ��
    public Transform cameraContainer; // ī�޶� ���� ������Ʈ
    public Transform playerBody; // �÷��̾��� ��ü

    [Header("Jump")]
    public float jumpPower; // ���� ��
    public LayerMask groundLayerMask; // �ٴ� ������ ���� ���̾� ����ũ

    [Header("Status")]
    public float health;
    private bool isDead;
    public float hunger;
    public float hungerSpeed;
    
    public bool IsDead => isDead;

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
        inventory = CharacterManager.Instance.Player.inventory;
        isDead = false;
        health = 100;
        hunger = 100;
    }

    private void FixedUpdate()
    {
        if (!isDead && !isAttack && !isGathering)
        {
            Move();
        }
    }

    private void Update()
    {
        Hungry();
    }

    private void LateUpdate()
    {
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
        if (context.phase == InputActionPhase.Started && IsGrounded() && !isDead && !isAttack && !isGathering)
        {
            // ���� �� Rigidbody�� ���� �߰�
            _rigidbody.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);

            // ���� �ִϸ��̼� ����
            _animator.SetTrigger("DoJump");
        }
    }
    public void OnInteractInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && !isDead && !isAttack && !isGathering)
        {
            isGathering = true;
            gatheringObject.SetActive(true);
            _animator.SetTrigger("DoInteract");
            Invoke("gatheringLock", 1.8f);
        }
    }

    void gatheringLock()
    {
        gatheringObject.SetActive(false);
        isGathering = false;
    }

    public void GetHit(float damage)
    {
        health -= damage;
        if (health > 0)
        {
            _animator.SetTrigger("DoGetHit");
        }
        else if (health <= 0)
        {
            _animator.SetTrigger("DoDie");
            isDead = true;
            OnPlayerDead?.Invoke();
        }
    }

    public void Eat(float amount)
    {
        hunger = Mathf.Min(hunger + amount, 100);
    }

    void Hungry()
    {
        if (hunger > 0)
        {
            hunger -= hungerSpeed * Time.deltaTime;
            health += Time.deltaTime * 0.5f;
        }
        else if (hunger <= 0)
        { 
            health -= Time.deltaTime;
        }
    }
}
 