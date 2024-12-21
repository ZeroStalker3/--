using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject playerUnitPrefab; // ������ ����� ������
    public GameObject aiUnitPrefab; // ������ ����� ��
    public Transform playerSpawnPoint; // ����� ������ ������
    public Transform aiSpawnPoint; // ����� ������ ��
    public int numberOfUnits = 2; // ���������� ������ ��� ������
    public static GameManager Instance { get; private set; }
    public List<Unit> units = new List<Unit>(); // ��������� ������ ������

    public int maxUnits = 5; // ��������� ������������ ���������� ������
    public int points = 10; // ����������� ���������� �����
    public int unitCost = 2; // ��������� ��������� ������ �����
    public int currentUnitCount = 0; // ������� ���������� ������

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

    // ����� ��� ��������� �����
    public bool TrySpawnUnit()
    {
        if (currentUnitCount < maxUnits && points >= unitCost)
        {
            points -= unitCost; // ��������� ����
            currentUnitCount++; // ����������� ������� ������
            return true; // ������� ���������� ����
        }
        return false; // �� ������� ���������� ����
    }

    // ����� ��� ��������� ����� �� ��������
    public void AddPoints(int amount)
    {
        points += amount;
        points = Mathf.Clamp(points, 0, 100); // ����������� �� 100 ����� (����� ���������)
    }

    // ����� ��� ���������� ������������� ���������� ������
    public void IncreaseMaxUnits(int amount)
    {
        maxUnits += amount;
        Debug.Log($"Max units increased to: {maxUnits}");
    }

    void Start()
    {
        // ������� ��������� ������ ������
        for (int i = 0; i < numberOfUnits; i++)
        {
            Vector3 spawnPosition = playerSpawnPoint.position + new Vector3(i * 2, 0, 0);
            Instantiate(playerUnitPrefab, spawnPosition, Quaternion.identity);
        }

        // ������� ��������� ������ ��
        for (int i = 0; i < numberOfUnits; i++)
        {
            Vector3 spawnPosition = aiSpawnPoint.position + new Vector3(i * 2, 0, 0);
            Instantiate(aiUnitPrefab, spawnPosition, Quaternion.identity);
        }

        playerBase.OnBaseDestroyed += EndGame;
        enemyBase.OnBaseDestroyed += EndGame;
    }
    
    // ����� ��� ���������� ����
    private void EndGame()
    {
        Debug.Log("Game Over!");
        Time.timeScale = 0;  // ��������� ������� ��� ���������� ����
    }

    private void Update()
    {
        // ������� ������������ ����� �� ������
        units.RemoveAll(unit => unit == null);

        // ������: ������ 20 ����� ����������� ������������ ���������� ������ �� 1
        if (points % 20 == 0 && points > 0)
        {
            IncreaseMaxUnits(1);
        }
    }

    // ����� ��� ���������� ����� � ������
    public void RegisterUnit(Unit unit)
    {
        if (!units.Contains(unit))
        {
            units.Add(unit);
        }
    }

    // ����� ��� �������� ����� �� ������ ��� ������
    public void UnregisterUnit(Unit unit)
    {
        units.Remove(unit);
    }
}
