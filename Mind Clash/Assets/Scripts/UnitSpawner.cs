using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSpawner : MonoBehaviour
{


    public GameObject unitPrefab; // ������ �����
    public Transform playerSpawnPoint; // ����� ������
    public int points = 10; // ��������� ���������� �����
    public int unitCost = 2; // ��������� ������ �����
    public int maxUnits = 5; // ��������� �������� ������, ������� ����� ���� ��������

    private int currentUnitCount = 0; // ���������� ������, ����������� �� ����

    public void OnSpawnButtonPressed()
    {
        // ���������, ������� �� ����� ��� ������ ����� � �� �������� �� ����� ������
        if (points >= unitCost && currentUnitCount < maxUnits)
        {
            SpawnUnit();
            points -= unitCost; // �������� ���� �� ������ �����
            currentUnitCount++; // ����������� ���������� ������ �� ����
        }
        else
        {
            Debug.Log("������������ ����� ��� ������� ����� ��� ��������� ����� ������.");
        }
    }

    public void SpawnUnit()
    {
        if (unitPrefab == null || playerSpawnPoint == null)
        {
            Debug.LogError("����������, ��������� UnitPrefab � PlayerSpawnPoint � ����������!");
            return;
        }

        // ������� ����� �� ����� ������ � ������ ��� ���
        Vector3 spawnPosition = playerSpawnPoint.position;
        GameObject newUnit = Instantiate(unitPrefab, spawnPosition, Quaternion.identity);
        newUnit.tag = "player";
    }

    // ����� ��� ���������� ������������� ����� ������ (��������, ������ 20 �����)
    public void IncreaseMaxUnits(int amount)
    {
        maxUnits += amount;
        Debug.Log("������������ ���������� ������ ��������� �� " + amount);
    }

    // ������: ���������� ����� ������ ��������� ������ (��� ���������� ��������� ����� �� ��������)
    void Update()
    {
        // ������ ���������� ����� � �������� �������
        if (Time.frameCount % 300 == 0) // ��������� ���� ������ 5-� ������� (������)
        {
            points += 1;
            Debug.Log("����: " + points);

            // ����������� ������������ ����� ������ ������ 20 �����
            if (points % 20 == 0)
            {
                IncreaseMaxUnits(1);
            }
        }
    }
}
