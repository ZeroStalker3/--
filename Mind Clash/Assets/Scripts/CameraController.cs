using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float moveSpeed = 10f; // Скорость перемещения камеры
    public float lookSpeed = 2f; // Скорость поворота камеры

    void Update()
    {
        // Перемещение камеры с помощью WASD или стрелок
        float moveHorizontal = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
        float moveVertical = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;

        transform.Translate(moveHorizontal, 0, moveVertical);

        // Поворот камеры при зажатой правой кнопке мыши
        if (Input.GetMouseButton(1)) // 1 - правая кнопка мыши
        {
            float mouseX = Input.GetAxis("Mouse X") * lookSpeed;
            float mouseY = Input.GetAxis("Mouse Y") * lookSpeed;

            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x - mouseY, transform.rotation.eulerAngles.y + mouseX, 0);
        }

        // Увеличение или уменьшение высоты камеры
        if (Input.GetKey(KeyCode.Q))
        {
            transform.Translate(0, -moveSpeed * Time.deltaTime, 0); // Спуск вниз
        }
        if (Input.GetKey(KeyCode.E))
        {
            transform.Translate(0, moveSpeed * Time.deltaTime, 0); // Подъем вверх
        }
    }
}
