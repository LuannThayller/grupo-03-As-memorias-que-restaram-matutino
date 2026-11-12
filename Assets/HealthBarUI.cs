
using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    [Header("References")]

    // Referência ao sistema de vida do jogador.
    public PlayerHealth playerHealth;

    // Referência ao Slider da barra de vida.
    public Slider healthSlider;

    private void Start()
    {
        // Verifica se as referências foram configuradas.
        if (playerHealth == null || healthSlider == null)
        {
            Debug.LogError(
                "HealthBarUI: Configure as referências no Inspector!"
            );
            return;
        }

        // Define os limites da barra.
        healthSlider.minValue = 0;
        healthSlider.maxValue = playerHealth.maxHealth;

        // Inicia mostrando a vida atual.
        healthSlider.value = playerHealth.currentHealth;
    }

    private void Update()
    {
        // Verifica se as referências existem.
        if (playerHealth == null || healthSlider == null)
            return;

        // Atualiza a barra para mostrar
        // a vida atual do jogador.
        healthSlider.value = playerHealth.currentHealth;
    }
}
