using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject playerUnitPrefab; // Префаб юнита игрока
    public GameObject aiUnitPrefab; // Префаб юнита ИИ
    public Transform playerSpawnPoint; // Точка спауна игрока
    public Transform aiSpawnPoint; // Точка спауна ИИ
    public int numberOfUnits = 2; // Количество юнитов для спауна
    public static GameManager Instance { get; private set; }
    public List<Unit> units = new List<Unit>(); // Объявляем список юнитов

    public int maxUnits = 5; // Начальное максимальное количество юнитов
    public int points = 10; // Изначальное количество очков
    public int unitCost = 2; // Стоимость установки одного юнита
    public int currentUnitCount = 0; // Текущая количество юнитов

    public BaseHealth playerBase;
    public BaseHealth enemyBase;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Метод для установки юнита
    public bool TrySpawnUnit()
    {
        if (currentUnitCount < maxUnits && points >= unitCost)
        {
            points -= unitCost; // Уменьшаем очки
            currentUnitCount++; // Увеличиваем счетчик юнитов
            return true; // Успешно установили юнит
        }
        return false; // Не удалось установить юнит
    }

    // Метод для получения очков со временем
    public void AddPoints(int amount)
    {
        points += amount;
        points = Mathf.Clamp(points, 0, 100); // Ограничение до 100 очков (можно настроить)
    }

    // Метод для увеличения максимального количества юнитов
    public void IncreaseMaxUnits(int amount)
    {
        maxUnits += amount;
        Debug.Log($"Max units increased to: {maxUnits}");
    }

    void Start()
    {
        // Спауним несколько юнитов игрока
        for (int i = 0; i < numberOfUnits; i++)
        {
            Vector3 spawnPosition = playerSpawnPoint.position + new Vector3(i * 2, 0, 0);
            Instantiate(playerUnitPrefab, spawnPosition, Quaternion.identity);
        }

        // Спауним несколько юнитов ИИ
        for (int i = 0; i < numberOfUnits; i++)
        {
            Vector3 spawnPosition = aiSpawnPoint.position + new Vector3(i * 2, 0, 0);
            Instantiate(aiUnitPrefab, spawnPosition, Quaternion.identity);
        }

        playerBase.OnBaseDestroyed += EndGame;
        enemyBase.OnBaseDestroyed += EndGame;
    }
    
    // Метод для завершения игры
    private void EndGame()
    {
        Debug.Log("Game Over!");
        Time.timeScale = 0;  // Остановка времени для завершения игры
    }

    private void Update()
    {
        // Удаляем уничтоженные юниты из списка
        units.RemoveAll(unit => unit == null);

        // Пример: каждые 20 очков увеличиваем максимальное количество юнитов на 1
        if (points % 20 == 0 && points > 0)
        {
            IncreaseMaxUnits(1);
        }
    }

    // Метод для добавления юнита в список
    public void RegisterUnit(Unit unit)
    {
        if (!units.Contains(unit))
        {
            units.Add(unit);
        }
    }

    // Метод для удаления юнита из списка при смерти
    public void UnregisterUnit(Unit unit)
    {
        units.Remove(unit);
    }
}
