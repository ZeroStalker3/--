using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform playerTransform; // ������ �� ������
    public Vector3 offset; // �������� ������ ������������ ������
    public float moveSpeed = 10f; // �������� ����������� ������
    public float lookSpeed = 2f; // �������� �������� ������

    private void LateUpdate()
    {
        // ���������� ������ �� ������� � ������ ��������
        Vector3 desiredPosition = playerTransform.position + offset;
        transform.position = Vector3.Lerp(transform.position, desiredPosition, moveSpeed * Time.deltaTime);

        // ������� ������ ��� ������� ������ ������ ����
        if (Input.GetMouseButton(1)) // 1 - ������ ������ ����
        {
            float mouseX = Input.GetAxis("Mouse X") * lookSpeed;
            float mouseY = Input.GetAxis("Mouse Y") * lookSpeed;

            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x - mouseY, transform.rotation.eulerAngles.y + mouseX, 0);
        }
    }
}
