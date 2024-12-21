using System.Collections.Generic;
using UnityEngine;

public class UnitCommandController : MonoBehaviour
{
    private List<Unit> playerUnits; // Все юниты с тегом "player"
    private List<Unit> selectedUnits; // Выделенные юниты

    void Start()
    {
        playerUnits = new List<Unit>();
        selectedUnits = new List<Unit>();

        UpdatePlayerUnitsList(); // Инициализируем список всех юнитов с тегом "player"
    }

    // Обновление списка юнитов
    private void UpdatePlayerUnitsList()
    {
        playerUnits.Clear();
        GameObject[] players = GameObject.FindGameObjectsWithTag("player"); // Убедитесь, что тег "player" прописан верно

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

    // Команда для атаки выбранных юнитов
    public void SetUnitsToAttack()
    {
        Debug.Log("Setting selected units to attack");
        foreach (Unit unit in selectedUnits)
        {
            unit.SetTactic(Unit.Tactic.Attack);
        }
    }

    // Команда для обороны выбранных юнитов
    public void SetUnitsToDefend()
    {
        Debug.Log("Setting selected units to defend");
        foreach (Unit unit in selectedUnits)
        {
            unit.SetTactic(Unit.Tactic.Defense);
        }
    }

    // Команда для отступления выбранных юнитов
    public void SetUnitsToRetreat()
    {
        Debug.Log("Setting selected units to retreat");
        foreach (Unit unit in selectedUnits)
        {
            unit.SetTactic(Unit.Tactic.Retreat);
        }
    }

    // Метод для обновления списка выделенных юнитов в зоне
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

    // Метод для обновления всех юнитов в случае добавления новых
    public void RefreshUnits()
    {
        UpdatePlayerUnitsList();
    }
}
