using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public Animator animator;  // Animator ������Ʈ
    public float moveSpeed = 5f;  // �⺻ �̵� �ӵ�
    public float sprintMultiplier = 1.5f;  // ������Ʈ ���
    private float speed = 0f;
    private bool isSprinting = false;
    private bool isJumping = false;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();  // Rigidbody ������Ʈ �ʱ�ȭ
    }

    void Update()
    {
        // �̵� �Է� ó�� (WASD �Ǵ� ����Ű)
        float move = Input.GetAxis("Vertical");  // �յڷ� �̵� (W/S �Ǵ� Up/Down)
        float strafe = Input.GetAxis("Horizontal");  // �¿� �̵� (A/D �Ǵ� Left/Right)

        // �̵� ���� ���� ���
        Vector3 movement = new Vector3(strafe, 0f, move).normalized;

        // �ӵ� ��� (�̵� ���� ũ��)
        speed = movement.magnitude * moveSpeed;
        animator.SetFloat("Speed", speed);

        // ������Ʈ ���� üũ (Shift Ű�� ������ ������Ʈ)
        isSprinting = Input.GetKey(KeyCode.LeftShift);
        animator.SetBool("IsSprinting", isSprinting);

        // �̵� ���⿡ ���� �ִϸ��̼� ��ȯ
        if (speed > 0.1f)
        {
            if (isSprinting)
            {
                // ������Ʈ �ִϸ��̼� ��ȯ
                animator.SetFloat("Speed", speed * sprintMultiplier);
            }
            else if (move < 0)  // �ڷ� �̵�
            {
                animator.SetTrigger("RunBackward");
            }
            else  // �������� �̵�
            {
                animator.SetTrigger("RunForward");
            }
        }
        else
        {
            // �̵��� ���� �� (Idle)
            animator.SetTrigger("Idle");
        }

        // ���� ó��
        if (Input.GetButtonDown("Jump") && !isJumping)
        {
            Jump();
        }

        // ���� �� ���� ó�� (�ִϸ��̼� �� ���� ��ȯ)
        if (isJumping)
        {
            animator.SetBool("IsJumping", true);
        }
        else
        {
            animator.SetBool("IsJumping", false);
        }

        // �������� �̵� ó�� (Rigidbody ���)
        MoveCharacter(movement);
    }

    void MoveCharacter(Vector3 movement)
    {
        // ĳ���Ͱ� ���� ���� �ƴ϶�� �̵�
        if (!isJumping)
        {
            rb.MovePosition(transform.position + movement * speed * Time.deltaTime);
        }
    }

    void Jump()
    {
        isJumping = true;
        animator.SetTrigger("Jump");

        // ���� �ִϸ��̼��� ���� �� ���� ���¸� �����ϴ� ������ �߰��� �� �ֽ��ϴ�.
        // ��: ���� �ð� �� ���� ���� �Ǵ� ���� ����� �� ����
    }

    // ���� ���� �� ó�� (��: ���� ����� ��)
    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Ground"))  // �ٴڰ� �浹���� ��
        {
            isJumping = false;
            animator.SetTrigger("Land");
        }
    }

}
