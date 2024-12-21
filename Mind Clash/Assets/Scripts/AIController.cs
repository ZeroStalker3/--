using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
    public Unit[] aiUnits; // Все юниты ИИ
    public float retreatThreshold = 30f; // Порог здоровья для отступления
    public float aggressiveThreshold = 50f; // Порог для агрессивного поведения
    public float defensiveThreshold = 20f; // Порог для оборонительной тактики
    private Unit targetUnit; // Целевой юнит

    void Start()
    {
        // Инициализация только юнитов с тегом "AI"
        aiUnits = FindObjectsByType<Unit>(FindObjectsSortMode.None);
        aiUnits = FilterAIUnits(aiUnits);
    }

    void Update()
    {
        foreach (Unit unit in aiUnits)
        {
            // Логика для отступления, если здоровье ниже порога
            if (unit != null && unit.health <= retreatThreshold)
            {
                Debug.Log($"Unit {unit.name} is retreating. Health: {unit.health}");
                Retreat(unit);
            }
            else
            {
                // Логика для других тактик, если здоровье в других пределах
                if (unit.health > aggressiveThreshold)
                {
                    unit.SetTactic(Unit.Tactic.Attack);
                }
                else if (unit.health > defensiveThreshold)
                {
                    unit.SetTactic(Unit.Tactic.Defense);
                }
                else
                {
                    unit.SetTactic(Unit.Tactic.Retreat);
                }
            }
        }

        // Проверка наличия и состояния целевого юнита
        if (targetUnit != null && targetUnit.CompareTag("AI"))
        {
            ChaseTarget(targetUnit.transform.position);
        }
        else
        {
            FindNewTarget();
        }
    }

    private void ChaseTarget(Vector3 targetPosition)
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * 5f);
    }

    private void FindNewTarget()
    {
        // Ищем ближайшую цель с тегом Players
        GameObject newTarget = GameObject.FindWithTag("player");
        if (newTarget != null)
        {
            targetUnit = newTarget.GetComponent<Unit>();
        }
    }

    // Метод для отступления юнита
    void Retreat(Unit unit)
    {
        Vector3 retreatDirection = (unit.transform.position - GetClosestEnemyPosition(unit)).normalized;
        unit.transform.position += retreatDirection * unit.speed * Time.deltaTime;
    }

    // Поиск ближайшего противника
    Vector3 GetClosestEnemyPosition(Unit unit)
    {
        Unit[] allUnits = FindObjectsByType<Unit>(FindObjectsSortMode.None);
        float closestDistance = Mathf.Infinity;
        Vector3 closestPosition = Vector3.zero;

        foreach (Unit enemyUnit in allUnits)
        {
            if (enemyUnit.CompareTag("player"))
            {
                float distance = Vector3.Distance(unit.transform.position, enemyUnit.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestPosition = enemyUnit.transform.position;
                }
            }
        }

        return closestPosition;
    }

    // Фильтрация списка юнитов, чтобы оставить только юниты с тегом "AI"
    private Unit[] FilterAIUnits(Unit[] units)
    {
        List<Unit> aiUnitList = new List<Unit>();
        foreach (Unit unit in units)
        {
            if (unit.CompareTag("AI"))
            {
                aiUnitList.Add(unit);
            }
        }
        return aiUnitList.ToArray();
    }
}
