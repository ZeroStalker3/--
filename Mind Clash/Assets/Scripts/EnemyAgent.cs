using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class EnemyAgent : Agent
{
    public Transform target; // Цель для атаки
    public float moveSpeed = 2f;
    public Transform[] patrolPoints; // Массив точек для патрулирования
    private int currentPatrolIndex;
    public Transform[] spawnPoints;  // Массив точек для спавна
    public GameObject enemyPrefab;   // Префаб юнита
    private Rigidbody rb; // Rigidbody для физического перемещения
    private float spawnCooldown = 5f;  // Время между спавнами
    private float lastSpawnTime = 0f;  // Время последнего спавна

    void Start()
    {
        // Проверка на назначенные точки патруля и цель
        if (patrolPoints.Length == 0)
        {
            Debug.LogError("Patrol points are not assigned in the inspector!");
        }

        if (target == null)
        {
            Debug.LogError("Target is not assigned!");
        }

        // Получаем Rigidbody
        rb = GetComponent<Rigidbody>();
    }

    public override void OnEpisodeBegin()
    {
        if (spawnPoints.Length > 0)
        {
            // Спавним юнитов на случайной точке
            Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
            Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
            // Инициализируем патруль
            currentPatrolIndex = 0;
        }
        else
        {
            Debug.LogError("Spawn points are not assigned!");
        }
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        // Добавление позиции агента в качестве наблюдения
        sensor.AddObservation(transform.position);
        sensor.AddObservation(GetComponent<Rigidbody>().velocity);

        // Наблюдения: позиция агента, цель и расстояние до цели
        sensor.AddObservation(transform.position); // Позиция агента
        sensor.AddObservation(target.position); // Позиция цели
        sensor.AddObservation(Vector3.Distance(transform.position, target.position)); // Расстояние до цели
        sensor.AddObservation(target.forward); // Направление цели
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        // Получаем действие из DiscreteActions
        int moveDirection = actions.DiscreteActions[0];

        // Выполняем действие на основе выборки
        switch (moveDirection)
        {
            case 0: Patrol(); break;    // Патрулирование
            case 1: AttackTarget(); break; // Атака
            case 2: Retreat(); break;    // Отступление
        };

        TrySpawnUnit();

        // Вычисляем награду
        float reward = CalculateReward();
        SetReward(reward);
    }

    private void TrySpawnUnit()
    {
        if (Time.time - lastSpawnTime > spawnCooldown)
        {
            SpawnUnit();
            lastSpawnTime = Time.time;
        }
    }

    private void SpawnUnit()
    {
        if (spawnPoints.Length > 0)
        {
            Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
            Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
            Debug.Log("Spawned new enemy unit!");
        }
        else
        {
            Debug.LogError("Spawn points are not assigned!");
        }
    }

    private float CalculateReward()
    {
        // Награда зависит от расстояния до цели
        float distanceToTarget = Vector3.Distance(transform.position, target.position);

        if (distanceToTarget < 1.0f)
        {
            return 1.0f; // Награда за достижение цели
        }
        else if (distanceToTarget < 5.0f)
        {
            return 0.5f; // Маленькая награда за близость
        }
        else
        {
            return -0.1f; // Негативная награда за отдаление от цели
        }
    }

    private void Patrol()
    {
        // Логика патрулирования
        if (patrolPoints.Length > 0)
        {
            Transform patrolPoint = patrolPoints[currentPatrolIndex];
            Vector3 direction = patrolPoint.position - transform.position;
            rb.MovePosition(transform.position + direction.normalized * moveSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, patrolPoint.position) < 0.1f)
            {
                currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
            }
        }
    }

    private void AttackTarget()
    {
        // Атака цели
        Vector3 direction = target.position - transform.position;
        rb.MovePosition(transform.position + direction.normalized * moveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, target.position) < 1.0f)
        {
            Debug.Log("Attacking target!");
            SetReward(2.0f); // Награда за атаку
        }
        else
        {
            SetReward(-0.1f); // Негативная награда за промедление
        }
    }

    private void Retreat()
    {
        // Логика отступления
        Vector3 direction = patrolPoints[0].position - transform.position;
        rb.MovePosition(transform.position + direction.normalized * moveSpeed * Time.deltaTime);

        SetReward(-0.1f); // Негативная награда за отступление
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        // Вручную можно выбирать действия для тестирования
        ActionSegment<int> discreteActions = actionsOut.DiscreteActions;
        discreteActions[0] = 0; // Патрулирование по умолчанию
    }
}
