using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AISpawner : MonoBehaviour
{
    public GameObject enemyPrefab; // Префаб юнита ИИ
    public Transform[] spawnPoints; // Массив точек спавна
    public int points = 10; // Начальные очки для спавна юнитов ИИ
    public int costPerUnit = 2; // Стоимость одного юнита
    public float spawnInterval = 5f; // Интервал времени для добавления новых юнитов

    private void Start()
    {
        // Запускаем процесс спавна с определенным интервалом
        StartCoroutine(SpawnUnits());
    }

    private IEnumerator SpawnUnits()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);

            if (points >= costPerUnit)
            {
                // Спавним юнита ИИ на случайной позиции
                Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
                GameObject enemy = Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
                enemy.tag = "Enemy";

                points -= costPerUnit; // Снимаем очки за добавленного юнита
            }
        }
    }
}
