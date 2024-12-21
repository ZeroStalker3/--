using UnityEngine;

public class BaseHealth : MonoBehaviour
{
    public int maxHealth = 1000;  // ������������ ���������� HP
    private int currentHealth;

    public delegate void BaseDestroyedHandler();  // ������� ��� ������� ����������� ����
    public event BaseDestroyedHandler OnBaseDestroyed;

    void Start()
    {
        currentHealth = maxHealth;  // ��������� ���������� �������� HP
    }

    // ����� ��� ��������� �����
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            BaseDestroyed();  // ����� ������ ��� ����������� ����
        }
    }

    // ����� ��� ��������� ���� ��� ����������� ����
    private void BaseDestroyed()
    {
        Debug.Log($"{gameObject.name} destroyed! Game Over.");
        OnBaseDestroyed?.Invoke();
    }
}
