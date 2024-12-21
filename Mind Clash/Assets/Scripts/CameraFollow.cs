using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform playerTransform; // Ссылка на игрока
    public Vector3 offset; // Смещение камеры относительно игрока
    public float moveSpeed = 10f; // Скорость перемещения камеры
    public float lookSpeed = 2f; // Скорость поворота камеры

    private void LateUpdate()
    {
        // Перемещаем камеру за игроком с учетом смещения
        Vector3 desiredPosition = playerTransform.position + offset;
        transform.position = Vector3.Lerp(transform.position, desiredPosition, moveSpeed * Time.deltaTime);

        // Поворот камеры при зажатой правой кнопке мыши
        if (Input.GetMouseButton(1)) // 1 - правая кнопка мыши
        {
            float mouseX = Input.GetAxis("Mouse X") * lookSpeed;
            float mouseY = Input.GetAxis("Mouse Y") * lookSpeed;

            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x - mouseY, transform.rotation.eulerAngles.y + mouseX, 0);
        }
    }
}
