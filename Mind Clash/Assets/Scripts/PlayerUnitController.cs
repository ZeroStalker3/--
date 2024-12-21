using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUnitController : MonoBehaviour
{
    public Unit unit; // Ссылка на компонент Unit юнита игрока
    private bool hasCommand = false; // Показывает, была ли отдана команда

    void Start()
    {
        unit = GetComponent<Unit>();
    }

    void Update()
    {
        if (hasCommand)
        {
            ExecuteCommand();
        }
    }

    public void ReceiveCommand(Vector3 destination)
    {
        hasCommand = true;
        unit.SetDestination(destination); // Устанавливаем цель для перемещения
    }

    private void ExecuteCommand()
    {
        if (Vector3.Distance(transform.position, unit.GetDestination()) > 0.1f)
        {
            // Перемещаем юнита к указанной точке
            transform.position = Vector3.MoveTowards(transform.position, unit.GetDestination(), unit.speed * Time.deltaTime);
        }
        else
        {
            hasCommand = false; // Команда выполнена, ожидаем следующую
        }
    }
}
