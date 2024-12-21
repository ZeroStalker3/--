using System.Collections.Generic;
using UnityEngine;

public class UnitCommandController : MonoBehaviour
{
    private List<Unit> playerUnits; // ��� ����� � ����� "player"
    private List<Unit> selectedUnits; // ���������� �����

    void Start()
    {
        playerUnits = new List<Unit>();
        selectedUnits = new List<Unit>();

        UpdatePlayerUnitsList(); // �������������� ������ ���� ������ � ����� "player"
    }

    // ���������� ������ ������
    private void UpdatePlayerUnitsList()
    {
        playerUnits.Clear();
        GameObject[] players = GameObject.FindGameObjectsWithTag("player"); // ���������, ��� ��� "player" �������� �����

        foreach (GameObject playerObj in players)
        {
            Unit unit = playerObj.GetComponent<Unit>();
            if (unit != null)
            {
                playerUnits.Add(unit);
            }
        }

        Debug.Log($"Number of player units found: {playerUnits.Count}");
    }

    // ������� ��� ����� ��������� ������
    public void SetUnitsToAttack()
    {
        Debug.Log("Setting selected units to attack");
        foreach (Unit unit in selectedUnits)
        {
            unit.SetTactic(Unit.Tactic.Attack);
        }
    }

    // ������� ��� ������� ��������� ������
    public void SetUnitsToDefend()
    {
        Debug.Log("Setting selected units to defend");
        foreach (Unit unit in selectedUnits)
        {
            unit.SetTactic(Unit.Tactic.Defense);
        }
    }

    // ������� ��� ����������� ��������� ������
    public void SetUnitsToRetreat()
    {
        Debug.Log("Setting selected units to retreat");
        foreach (Unit unit in selectedUnits)
        {
            unit.SetTactic(Unit.Tactic.Retreat);
        }
    }

    // ����� ��� ���������� ������ ���������� ������ � ����
    public void SelectUnitsInArea(Rect selectionRect)
    {
        selectedUnits.Clear();

        foreach (Unit unit in playerUnits)
        {
            Vector3 screenPosition = Camera.main.WorldToScreenPoint(unit.transform.position);
            if (selectionRect.Contains(screenPosition, true))
            {
                selectedUnits.Add(unit);
            }
        }

        Debug.Log($"Number of units selected: {selectedUnits.Count}");
    }

    // ����� ��� ���������� ���� ������ � ������ ���������� �����
    public void RefreshUnits()
    {
        UpdatePlayerUnitsList();
    }
}
