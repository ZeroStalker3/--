using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUnitController : MonoBehaviour
{
    public Unit unit; // ������ �� ��������� Unit ����� ������
    private bool hasCommand = false; // ����������, ���� �� ������ �������

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
        unit.SetDestination(destination); // ������������� ���� ��� �����������
    }

    private void ExecuteCommand()
    {
        if (Vector3.Distance(transform.position, unit.GetDestination()) > 0.1f)
        {
            // ���������� ����� � ��������� �����
            transform.position = Vector3.MoveTowards(transform.position, unit.GetDestination(), unit.speed * Time.deltaTime);
        }
        else
        {
            hasCommand = false; // ������� ���������, ������� ���������
        }
    }
}
