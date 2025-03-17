using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    public Transform player;  // ĳ������ Transform
    public float distance = 5f;  // ī�޶�� ĳ���� ������ �Ÿ�
    public float height = 2f;  // ī�޶��� ���� (ĳ������ ����)
    public float rotationSpeed = 5f;  // ī�޶� ȸ�� �ӵ�
    public float smoothSpeed = 0.125f;  // ī�޶��� �ε巯�� �̵� �ӵ�
    public Vector3 offset;  // ī�޶��� �⺻ ������ (����Ʈ�� player���� ������ �Ÿ�)

    private float currentRotationX = 0f;
    private float currentRotationY = 0f;
    private float rotationX = 0f;
    private float rotationY = 0f;

    void Start()
    {
        // �ʱ� ������ ����
        offset = new Vector3(0, height, -distance);
    }

    void Update()
    {
        // ���콺 �Է¿� ���� ȸ��
        rotationX -= Input.GetAxis("Mouse Y") * rotationSpeed;
        rotationY += Input.GetAxis("Mouse X") * rotationSpeed;

        // ȸ�� ����
        rotationX = Mathf.Clamp(rotationX, -30f, 60f);

        // ī�޶� ȸ�� ����
        Quaternion rotation = Quaternion.Euler(rotationX, rotationY, 0);
        Vector3 targetPosition = player.position + rotation * offset;

        // ī�޶� ��ġ �ε巴�� ����
        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed);

        // ī�޶� �׻� ĳ���͸� �ٶ󺸰� �ϱ�
        transform.LookAt(player.position);
    }
}
