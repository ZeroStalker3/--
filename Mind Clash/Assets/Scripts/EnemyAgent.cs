using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class EnemyAgent : Agent
{
    public Transform target; // ���� ��� �����
    public float moveSpeed = 2f;
    public Transform[] patrolPoints; // ������ ����� ��� ��������������
    private int currentPatrolIndex;
    public Transform[] spawnPoints;  // ������ ����� ��� ������
    public GameObject enemyPrefab;   // ������ �����
    private Rigidbody rb; // Rigidbody ��� ����������� �����������
    private float spawnCooldown = 5f;  // ����� ����� ��������
    private float lastSpawnTime = 0f;  // ����� ���������� ������

    void Start()
    {
        // �������� �� ����������� ����� ������� � ����
        if (patrolPoints.Length == 0)
        {
            Debug.LogError("Patrol points are not assigned in the inspector!");
        }

        if (target == null)
        {
            Debug.LogError("Target is not assigned!");
        }

        // �������� Rigidbody
        rb = GetComponent<Rigidbody>();
    }

    public override void OnEpisodeBegin()
    {
        if (spawnPoints.Length > 0)
        {
            // ������� ������ �� ��������� �����
            Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
            Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
            // �������������� �������
            currentPatrolIndex = 0;
        }
        else
        {
            Debug.LogError("Spawn points are not assigned!");
        }
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        // ���������� ������� ������ � �������� ����������
        sensor.AddObservation(transform.position);
        sensor.AddObservation(GetComponent<Rigidbody>().velocity);

        // ����������: ������� ������, ���� � ���������� �� ����
        sensor.AddObservation(transform.position); // ������� ������
        sensor.AddObservation(target.position); // ������� ����
        sensor.AddObservation(Vector3.Distance(transform.position, target.position)); // ���������� �� ����
        sensor.AddObservation(target.forward); // ����������� ����
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        // �������� �������� �� DiscreteActions
        int moveDirection = actions.DiscreteActions[0];

        // ��������� �������� �� ������ �������
        switch (moveDirection)
        {
            case 0: Patrol(); break;    // ��������������
            case 1: AttackTarget(); break; // �����
            case 2: Retreat(); break;    // �����������
        };

        TrySpawnUnit();

        // ��������� �������
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
        // ������� ������� �� ���������� �� ����
        float distanceToTarget = Vector3.Distance(transform.position, target.position);

        if (distanceToTarget < 1.0f)
        {
            return 1.0f; // ������� �� ���������� ����
        }
        else if (distanceToTarget < 5.0f)
        {
            return 0.5f; // ��������� ������� �� ��������
        }
        else
        {
            return -0.1f; // ���������� ������� �� ��������� �� ����
        }
    }

    private void Patrol()
    {
        // ������ ��������������
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
        // ����� ����
        Vector3 direction = target.position - transform.position;
        rb.MovePosition(transform.position + direction.normalized * moveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, target.position) < 1.0f)
        {
            Debug.Log("Attacking target!");
            SetReward(2.0f); // ������� �� �����
        }
        else
        {
            SetReward(-0.1f); // ���������� ������� �� �����������
        }
    }

    private void Retreat()
    {
        // ������ �����������
        Vector3 direction = patrolPoints[0].position - transform.position;
        rb.MovePosition(transform.position + direction.normalized * moveSpeed * Time.deltaTime);

        SetReward(-0.1f); // ���������� ������� �� �����������
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        // ������� ����� �������� �������� ��� ������������
        ActionSegment<int> discreteActions = actionsOut.DiscreteActions;
        discreteActions[0] = 0; // �������������� �� ���������
    }
}
