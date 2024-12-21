using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UnitInfoUI : MonoBehaviour
{
    public TMP_Text healthText; // ����������� TextMeshPro Health
    public TMP_Text attackPowerText; // ����������� TextMeshPro Attack Power
    public TMP_Text speedText; // ����������� TextMeshPro Speed

    private Unit selectedUnit;

    public void SelectUnit(Unit unit)
    {
        selectedUnit = unit;
        UpdateUnitInfo();
    }

    private void UpdateUnitInfo()
    {
        if (selectedUnit != null)
        {
            Debug.Log("��������� ���������� � �����: " + selectedUnit.name);

            if (healthText != null)
            {
                healthText.text = "Health: " + selectedUnit.health.ToString();
            }
            else
            {
                Debug.LogWarning("HealthText �� ��������.");
            }

            if (attackPowerText != null)
            {
                attackPowerText.text = "Attack: " + selectedUnit.attackPower.ToString();
            }
            else
            {
                Debug.LogWarning("AttackPowerText �� ��������.");
            }

            if (speedText != null)
            {
                speedText.text = "Speed: " + selectedUnit.speed.ToString();
            }
            else
            {
                Debug.LogWarning("SpeedText �� ��������.");
            }
        }
        else
        {
            Debug.LogWarning("SelectedUnit ����� null.");
        }
    }

}
