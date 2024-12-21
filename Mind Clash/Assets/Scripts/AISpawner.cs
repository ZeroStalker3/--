using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AISpawner : MonoBehaviour
{
    public GameObject enemyPrefab; // ������ ����� ��
    public Transform[] spawnPoints; // ������ ����� ������
    public int points = 10; // ��������� ���� ��� ������ ������ ��
    public int costPerUnit = 2; // ��������� ������ �����
    public float spawnInterval = 5f; // �������� ������� ��� ���������� ����� ������

    private void Start()
    {
        // ��������� ������� ������ � ������������ ����������
        StartCoroutine(SpawnUnits());
    }

    private IEnumerator SpawnUnits()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);

            if (points >= costPerUnit)
            {
                // ������� ����� �� �� ��������� �������
                Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
                GameObject enemy = Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
                enemy.tag = "Enemy";

                points -= costPerUnit; // ������� ���� �� ������������ �����
            }
        }
    }
}
