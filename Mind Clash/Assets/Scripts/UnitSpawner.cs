using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSpawner : MonoBehaviour
{


    public GameObject unitPrefab; // Префаб юнита
    public Transform playerSpawnPoint; // Точка спавна
    public int points = 10; // Начальное количество очков
    public int unitCost = 2; // Стоимость одного юнита
    public int maxUnits = 5; // Начальный максимум юнитов, который может быть увеличен

    private int currentUnitCount = 0; // Количество юнитов, размещенных на поле

    public void OnSpawnButtonPressed()
    {
        // Проверяем, хватает ли очков для спавна юнита и не превышен ли лимит юнитов
        if (points >= unitCost && currentUnitCount < maxUnits)
        {
            SpawnUnit();
            points -= unitCost; // Вычитаем очки за призыв юнита
            currentUnitCount++; // Увеличиваем количество юнитов на поле
        }
        else
        {
            Debug.Log("Недостаточно очков для призыва юнита или достигнут лимит юнитов.");
        }
    }

    public void SpawnUnit()
    {
        if (unitPrefab == null || playerSpawnPoint == null)
        {
            Debug.LogError("Пожалуйста, назначьте UnitPrefab и PlayerSpawnPoint в инспекторе!");
            return;
        }

        // Спавним юнита на точке игрока и задаем ему тег
        Vector3 spawnPosition = playerSpawnPoint.position;
        GameObject newUnit = Instantiate(unitPrefab, spawnPosition, Quaternion.identity);
        newUnit.tag = "player";
    }

    // Метод для увеличения максимального числа юнитов (например, каждые 20 очков)
    public void IncreaseMaxUnits(int amount)
    {
        maxUnits += amount;
        Debug.Log("Максимальное количество юнитов увеличено на " + amount);
    }

    // Пример: добавление очков каждые несколько секунд (для увеличения доступных очков со временем)
    void Update()
    {
        // Логика добавления очков с течением времени
        if (Time.frameCount % 300 == 0) // Добавляем очки каждую 5-ю секунду (пример)
        {
            points += 1;
            Debug.Log("Очки: " + points);

            // Увеличиваем максимальный лимит юнитов каждые 20 очков
            if (points % 20 == 0)
            {
                IncreaseMaxUnits(1);
            }
        }
    }
}
