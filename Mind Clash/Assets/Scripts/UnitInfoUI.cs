using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UnitInfoUI : MonoBehaviour
{
    public TMP_Text healthText; // Привязываем TextMeshPro Health
    public TMP_Text attackPowerText; // Привязываем TextMeshPro Attack Power
    public TMP_Text speedText; // Привязываем TextMeshPro Speed

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
            Debug.Log("Обновляем информацию о юните: " + selectedUnit.name);

            if (healthText != null)
            {
                healthText.text = "Health: " + selectedUnit.health.ToString();
            }
            else
            {
                Debug.LogWarning("HealthText не привязан.");
            }

            if (attackPowerText != null)
            {
                attackPowerText.text = "Attack: " + selectedUnit.attackPower.ToString();
            }
            else
            {
                Debug.LogWarning("AttackPowerText не привязан.");
            }

            if (speedText != null)
            {
                speedText.text = "Speed: " + selectedUnit.speed.ToString();
            }
            else
            {
                Debug.LogWarning("SpeedText не привязан.");
            }
        }
        else
        {
            Debug.LogWarning("SelectedUnit равен null.");
        }
    }

}
