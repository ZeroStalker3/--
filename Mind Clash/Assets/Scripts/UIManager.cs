using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    public static UIManager Instance; // Синглтон для доступа из других классов
    public Text pointsText; // Текст для отображения очков
    public Text unitsText; // Текст для отображения количества юнитов

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
        RefreshUI(); // Обновляем UI при старте
    }

    public void RefreshUI()
    {
        pointsText.text = "Points: " + GameManager.Instance.points;
        unitsText.text = "Units: " + GameManager.Instance.currentUnitCount + "/" + GameManager.Instance.maxUnits;
    }
}
