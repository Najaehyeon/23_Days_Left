using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public Animator animator;  // Animator 컴포넌트
    public float moveSpeed = 5f;  // 기본 이동 속도
    public float sprintMultiplier = 1.5f;  // 스프린트 배수
    private float speed = 0f;
    private bool isSprinting = false;
    private bool isJumping = false;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();  // Rigidbody 컴포넌트 초기화
    }

    void Update()
    {
        // 이동 입력 처리 (WASD 또는 방향키)
        float move = Input.GetAxis("Vertical");  // 앞뒤로 이동 (W/S 또는 Up/Down)
        float strafe = Input.GetAxis("Horizontal");  // 좌우 이동 (A/D 또는 Left/Right)

        // 이동 방향 벡터 계산
        Vector3 movement = new Vector3(strafe, 0f, move).normalized;

        // 속도 계산 (이동 벡터 크기)
        speed = movement.magnitude * moveSpeed;
        animator.SetFloat("Speed", speed);

        // 스프린트 상태 체크 (Shift 키를 누르면 스프린트)
        isSprinting = Input.GetKey(KeyCode.LeftShift);
        animator.SetBool("IsSprinting", isSprinting);

        // 이동 방향에 따른 애니메이션 전환
        if (speed > 0.1f)
        {
            if (isSprinting)
            {
                // 스프린트 애니메이션 전환
                animator.SetFloat("Speed", speed * sprintMultiplier);
            }
            else if (move < 0)  // 뒤로 이동
            {
                animator.SetTrigger("RunBackward");
            }
            else  // 앞쪽으로 이동
            {
                animator.SetTrigger("RunForward");
            }
        }
        else
        {
            // 이동이 없을 때 (Idle)
            animator.SetTrigger("Idle");
        }

        // 점프 처리
        if (Input.GetButtonDown("Jump") && !isJumping)
        {
            Jump();
        }

        // 점프 중 상태 처리 (애니메이션 및 상태 전환)
        if (isJumping)
        {
            animator.SetBool("IsJumping", true);
        }
        else
        {
            animator.SetBool("IsJumping", false);
        }

        // 물리적인 이동 처리 (Rigidbody 사용)
        MoveCharacter(movement);
    }

    void MoveCharacter(Vector3 movement)
    {
        // 캐릭터가 점프 중이 아니라면 이동
        if (!isJumping)
        {
            rb.MovePosition(transform.position + movement * speed * Time.deltaTime);
        }
    }

    void Jump()
    {
        isJumping = true;
        animator.SetTrigger("Jump");

        // 점프 애니메이션이 끝난 후 점프 상태를 종료하는 로직을 추가할 수 있습니다.
        // 예: 일정 시간 후 점프 종료 또는 땅에 닿았을 때 종료
    }

    // 점프 종료 후 처리 (예: 땅에 닿았을 때)
    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Ground"))  // 바닥과 충돌했을 때
        {
            isJumping = false;
            animator.SetTrigger("Land");
        }
    }

}
