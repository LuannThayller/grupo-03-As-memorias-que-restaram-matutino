
using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]

    // Quantidade máxima de vida do jogador.
    public int maxHealth = 100;

    // Vida atual. Pode ser consultada por outros scripts,
    // mas só pode ser modificada por esta classe.
    public int currentHealth { get; private set; }

    [Header("Damage Settings")]

    // Tempo em que o jogador fica invulnerável
    // depois de receber dano.
    public float invincibilityDuration = 1f;

    // Indica se o jogador está invulnerável.
    private bool isInvincible = false;

    // Indica se o jogador já morreu.
    private bool isDead = false;

    [Header("Visual Feedback")]

    // Sprite utilizado para o efeito de dano.
    public SpriteRenderer spriteRenderer;


    [Header("Game Over")]
    public GameOverUI gameOverUI;

    // Guarda a cor original do personagem.
    private Color originalColor;

    private void Awake()
    {
        // Inicia com a vida máxima.
        currentHealth = maxHealth;

        // Procura o SpriteRenderer caso não
        // tenha sido configurado no Inspector.
        if (spriteRenderer == null)
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        if (spriteRenderer != null)
            originalColor = spriteRenderer.color;
    }

    // Método chamado quando o jogador recebe dano.
    public void TakeDamage(int damage)
    {
        // Não recebe dano se estiver morto,
        // invulnerável ou se o dano for inválido.
        if (isDead || isInvincible || damage <= 0)
            return;

        // Reduz a vida sem permitir valores negativos.
        currentHealth = Mathf.Max(
            currentHealth - damage,
            0
        );

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayPlayerDamage();
        }

        Debug.Log(
            "Player recebeu " + damage +
            " de dano. Vida: " + currentHealth
        );

        // Verifica se o jogador morreu.
        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            // Ativa a invulnerabilidade temporária.
            StartCoroutine(InvincibilityRoutine());
        }
    }

    // Recupera pontos de vida.
    public void Heal(int amount)
    {
        if (isDead || amount <= 0)
            return;

        // Impede que a vida ultrapasse o máximo.
        currentHealth = Mathf.Min(
            currentHealth + amount,
            maxHealth
        );
    }

    // Controla o tempo de invulnerabilidade.
    private IEnumerator InvincibilityRoutine()
    {
        isInvincible = true;

        // Muda a cor do sprite para indicar dano.
        if (spriteRenderer != null)
            spriteRenderer.color = Color.red;

        // Aguarda o tempo configurado.
        yield return new WaitForSeconds(
            invincibilityDuration
        );

        // Restaura a cor original.
        if (spriteRenderer != null)
            spriteRenderer.color = originalColor;

        // Permite receber dano novamente.
        isInvincible = false;
    }

    // Executado quando a vida chega a zero.

    private void Die()
    {
        // Impede que a morte seja executada novamente.
        if (isDead)
            return;

        isDead = true;

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayPlayerDeath();
        }

        Debug.Log("Player morreu!");

        // Desativa os controles de movimento.
        PlayerMovement movement =
            GetComponent<PlayerMovement>();

        if (movement != null)
        {
            movement.enabled = false;
        }

        // Mostra a tela de Game Over.
        if (gameOverUI != null)
        {
            gameOverUI.ShowGameOver();
        }
        else
        {
            Debug.LogWarning(
                "GameOverUI não configurado no PlayerHealth!"
            );
        }
    }

}
