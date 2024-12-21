using UnityEngine;

public class BaseHealth : MonoBehaviour
{
    public int maxHealth = 1000;  // Максимальное количество HP
    private int currentHealth;

    public delegate void BaseDestroyedHandler();  // Делегат для события уничтожения базы
    public event BaseDestroyedHandler OnBaseDestroyed;

    void Start()
    {
        currentHealth = maxHealth;  // Установка начального значения HP
    }

    // Метод для нанесения урона
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            BaseDestroyed();  // Вызов метода при уничтожении базы
        }
    }

    // Метод для окончания игры при уничтожении базы
    private void BaseDestroyed()
    {
        Debug.Log($"{gameObject.name} destroyed! Game Over.");
        OnBaseDestroyed?.Invoke();
    }
}
