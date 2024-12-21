using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Unit : MonoBehaviour
{
    public float health = 200f;
    public float maxHealth = 200f;

    public float speed = 15f;
    public float attackPower = 10f;
    public float attackRange = 1f;
    public float attackCooldown = 1.5f; // Задержка между атаками (в секундах)

    private Vector3 destination;
    private bool hasDestination = false;

    private Unit targetEnemy;
    private Rigidbody rb;
    private float lastAttackTime; // Время последней атаки
    
    private UnitInfoUI unitInfoUI;
    public GameObject healthBarPrefab; // Префаб полоски здоровья
    private GameObject healthBar; // Объект полоски здоровья
    private Image healthBarImage; // Изображение полоски здоровья
    private Transform canvasTransform;
    private Transform healthBarTransform;

    public enum Tactic { Attack, Defense, Retreat };
    public Tactic currentTactic;

    // Устанавливает точку назначения для юнита
    public void SetDestination(Vector3 newDestination)
    {
        destination = newDestination;
        hasDestination = true;
    }

    // Возвращает текущую точку назначения юнита
    public Vector3 GetDestination()
    {
        return destination;
    }

    void Start()
    {
        GameManager.Instance.RegisterUnit(this);
        rb = GetComponent<Rigidbody>();

        GameObject canvasObject = GameObject.Find("Canvas");
        if (canvasObject == null)
        {
            Debug.LogError("Canvas not found!");
            return;
        }
        canvasTransform = canvasObject.transform;

        if (healthBarPrefab != null)
        {
            healthBar = Instantiate(healthBarPrefab, canvasTransform);
            Debug.Log("Health bar created successfully!");

            // Поиск объекта Fill
            Transform fillTransform = healthBar.transform.Find("Fill");
            if (fillTransform != null)
            {
                healthBarImage = fillTransform.GetComponent<Image>();
                if (healthBarImage == null)
                {
                    Debug.LogError("Image component on Fill not found!");
                }
            }
            else
            {
                Debug.LogError("Fill object not found in HealthBar prefab!");
            }
        }
        else
        {
            Debug.LogError("HealthBarPrefab is not assigned!");
        }

        health = maxHealth;

        // Создание полоски здоровья
        if (healthBarPrefab != null)
        {
            healthBar = Instantiate(healthBarPrefab);
            healthBarTransform = healthBar.transform;

            // Устанавливаем родительский canvas для полоски
            healthBarTransform.SetParent(GameObject.Find("Canvas").transform, false);

            // Получаем Image компонента полоски здоровья
            Transform fillTransform = healthBarTransform.Find("Fill");
            if (fillTransform != null)
            {
                healthBarImage = fillTransform.GetComponent<Image>();
            }
        }

        lastAttackTime = -attackCooldown;

        unitInfoUI = FindFirstObjectByType<UnitInfoUI>(); // Находим первый объект UnitInfoUI в сцене
    }
    void OnMouseDown()
    {
        UnitInfoUI unitInfoUI = Object.FindAnyObjectByType<UnitInfoUI>();
        if (unitInfoUI != null)
        {
            unitInfoUI.SelectUnit(this);
        }
        else
        {
            Debug.LogWarning("UnitInfoUI не найден.");
        }
    }


    void Update()
    {
        if (hasDestination)
        {
            MoveToDestination();
        }

        UpdateHealthBar(); // Обновляем полоску здоровья

        if (targetEnemy == null)
        {
            FindClosestEnemy();
        }
        else
        {
            MoveTowardsTarget();
            if (Vector3.Distance(transform.position, targetEnemy.transform.position) <= attackRange)
            {
                if (Time.time >= lastAttackTime + attackCooldown)
                {
                    Attack(targetEnemy);
                    lastAttackTime = Time.time;
                }
            }
        }

        // Обновляем позицию полоски здоровья, размещая её над юнитом
        if (healthBar != null)
        {
            Vector3 offset = new Vector3(0, 2, 0); // Смещение по высоте (над головой юнита)
            healthBarTransform.position = transform.position + offset;
        }

        switch (currentTactic)
        {
            case Tactic.Attack:
                // Логика для атаки
                MoveTowardsTarget();
                break;
            case Tactic.Defense:
                // Логика для обороны
                // Например, юнит остается на месте и усиливает свою защиту
                break;
            case Tactic.Retreat:
                // Логика для отступления
                RetreatFromTarget();
                break;
        }

        if (Input.GetMouseButtonDown(0)) // ЛКМ
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                Unit unit = hit.collider.GetComponent<Unit>();
                if (unit != null)
                {
                    unitInfoUI.SelectUnit(unit);
                }
            }
        }
    }
    private void MoveToDestination()
    {
        // Перемещение юнита к точке назначения
        if (Vector3.Distance(transform.position, destination) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, destination, speed * Time.deltaTime);
        }
        else
        {
            hasDestination = false; // Останавливаемся, если достигли цели
        }
    }

    public void SetTactic(Tactic tactic)
    {
        currentTactic = tactic;
    }

    void RetreatFromTarget()
    {
        // Логика для отступления от врага
        if (targetEnemy != null)
        {
            Vector3 retreatDirection = (transform.position - targetEnemy.transform.position).normalized;
            transform.position += retreatDirection * speed * Time.deltaTime;
        }
    }

    // Обновляем позицию и значение полоски здоровья
    private void UpdateHealthBar()
    {
        if (healthBar == null) return;

        Vector3 healthBarPosition = Camera.main.WorldToScreenPoint(transform.position + Vector3.up * 2);
        healthBar.transform.position = healthBarPosition;

        if (healthBarImage != null)
        {
            // Обновляем заполнение полоски здоровья
            healthBarImage.fillAmount = health / maxHealth;
            //Debug.Log("Health bar updated: " + (health / maxHealth));
        }
        else
        {
            Debug.LogError("HealthBarImage is null. Cannot update health bar.");
        }
    }

    // Метод для поиска ближайшего врага
    void FindClosestEnemy()
    {
        Unit[] allUnits = FindObjectsByType<Unit>(FindObjectsSortMode.None);
        float closestDistance = Mathf.Infinity;

        foreach (Unit unit in allUnits)
        {
            if (unit != this && unit.tag != this.tag) // Находим юнитов противоположной команды
            {
                float distance = Vector3.Distance(transform.position, unit.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    targetEnemy = unit;
                }
            }
        }
    }

    // Метод для передвижения к цели
    private void MoveTowardsTarget()
    {
        if (targetEnemy != null)
        {
            Vector3 direction = (targetEnemy.transform.position - transform.position).normalized;
            rb.MovePosition(transform.position + direction * speed * Time.deltaTime);
        }
    }

    // Метод для атаки врага
    public void Attack(Unit enemy)
    {
        enemy.TakeDamage(attackPower);
    }

    // Получение урона
    public void TakeDamage(float amount)
    {
        health -= amount;
        Debug.Log("Unit took damage, current health: " + health);

        health = Mathf.Clamp(health, 0, maxHealth);

        if (health <= 0)
        {
            Die();
        }
    }

    // Удаляем полоску при смерти
    private void Die()
    {                             
        // Удаляем юнит из GameManager перед уничтожением
        GameManager.Instance.UnregisterUnit(this);
        Destroy(healthBar); // Удаляем полоску здоровья
        Destroy(gameObject); // Удаляем сам объект

    }

}