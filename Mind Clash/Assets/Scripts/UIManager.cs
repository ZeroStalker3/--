using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    public static UIManager Instance; // �������� ��� ������� �� ������ �������
    public Text pointsText; // ����� ��� ����������� �����
    public Text unitsText; // ����� ��� ����������� ���������� ������

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

    void Start()
    {
        RefreshUI(); // ��������� UI ��� ������
    }

    public void RefreshUI()
    {
        pointsText.text = "Points: " + GameManager.Instance.points;
        unitsText.text = "Units: " + GameManager.Instance.currentUnitCount + "/" + GameManager.Instance.maxUnits;
    }
}
