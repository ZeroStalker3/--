using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float moveSpeed = 10f; // �������� ����������� ������
    public float lookSpeed = 2f; // �������� �������� ������

    void Update()
    {
        // ����������� ������ � ������� WASD ��� �������
        float moveHorizontal = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
        float moveVertical = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;

        transform.Translate(moveHorizontal, 0, moveVertical);

        // ������� ������ ��� ������� ������ ������ ����
        if (Input.GetMouseButton(1)) // 1 - ������ ������ ����
        {
            float mouseX = Input.GetAxis("Mouse X") * lookSpeed;
            float mouseY = Input.GetAxis("Mouse Y") * lookSpeed;

            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x - mouseY, transform.rotation.eulerAngles.y + mouseX, 0);
        }

        // ���������� ��� ���������� ������ ������
        if (Input.GetKey(KeyCode.Q))
        {
            transform.Translate(0, -moveSpeed * Time.deltaTime, 0); // ����� ����
        }
        if (Input.GetKey(KeyCode.E))
        {
            transform.Translate(0, moveSpeed * Time.deltaTime, 0); // ������ �����
        }
    }
}
