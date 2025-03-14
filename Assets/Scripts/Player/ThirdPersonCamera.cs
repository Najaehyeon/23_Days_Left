using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    public Transform player;  // 캐릭터의 Transform
    public float distance = 5f;  // 카메라와 캐릭터 사이의 거리
    public float height = 2f;  // 카메라의 높이 (캐릭터의 위쪽)
    public float rotationSpeed = 5f;  // 카메라 회전 속도
    public float smoothSpeed = 0.125f;  // 카메라의 부드러운 이동 속도
    public Vector3 offset;  // 카메라의 기본 오프셋 (디폴트는 player에서 떨어진 거리)

    private float currentRotationX = 0f;
    private float currentRotationY = 0f;
    private float rotationX = 0f;
    private float rotationY = 0f;

    void Start()
    {
        // 초기 오프셋 설정
        offset = new Vector3(0, height, -distance);
    }

    void Update()
    {
        // 마우스 입력에 따른 회전
        rotationX -= Input.GetAxis("Mouse Y") * rotationSpeed;
        rotationY += Input.GetAxis("Mouse X") * rotationSpeed;

        // 회전 제한
        rotationX = Mathf.Clamp(rotationX, -30f, 60f);

        // 카메라 회전 적용
        Quaternion rotation = Quaternion.Euler(rotationX, rotationY, 0);
        Vector3 targetPosition = player.position + rotation * offset;

        // 카메라 위치 부드럽게 보간
        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed);

        // 카메라 항상 캐릭터를 바라보게 하기
        transform.LookAt(player.position);
    }
}
