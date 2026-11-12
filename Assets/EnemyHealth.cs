
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("Health Settings")]

    // Vida máxima do inimigo.
    public int maxHealth = 60;

    // Vida atual.
    private int currentHealth;

    // Impede executar a morte várias vezes.
    private bool isDead = false;

    private void Awake()
    {
        // Inicializa a vida do inimigo.
        currentHealth = maxHealth;
    }

    // Recebe o dano causado pelo projétil.
    public void TakeDamage(int damage)
    {
        if (isDead || damage <= 0)
            return;

        // Reduz a vida.
        currentHealth = Mathf.Max(
            currentHealth - damage,
            0
        );

        Debug.Log(
            "Enemy recebeu dano! Vida: " +
            currentHealth
        );

        // Verifica se o inimigo morreu.
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        isDead = true;

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayEnemyDeath();
        }

        Debug.Log("Enemy morreu!");

        // Destrói o inimigo.
        Destroy(gameObject);
    }
}
