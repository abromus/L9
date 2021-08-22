using System;

using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private int _maxHealth = 200;

    private int _currentHealth;
    private int _minHealth = 0;

    public int MinHealth => _minHealth;
    public int MaxHealth => _maxHealth;
    public int CurrentHealth => _currentHealth;

    public event Action<int> Damaged;
    public event Action<int> Cured;

    private void Start()
    {
        _currentHealth = _maxHealth;
    }

    private void TakeDamage(int damage)
    {
        _currentHealth = Mathf.Clamp(_currentHealth - damage, _minHealth, _maxHealth);

        Damaged?.Invoke(damage);
    }

    private void Cure(int health)
    {
        _currentHealth = Mathf.Clamp(_currentHealth + health, _minHealth, _maxHealth);

        Cured?.Invoke(health);
    }
}
