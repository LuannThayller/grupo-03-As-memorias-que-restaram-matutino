
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour
{
    [Header("UI References")]

    // Painel que contém a tela de Game Over.
    public GameObject gameOverPanel;

    private void Start()
    {
        // Garante que o jogo comece normalmente.
        Time.timeScale = 1f;

        // Esconde a tela de morte no início.
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }
    }

    // Chamado quando o jogador morre.
    public void ShowGameOver()
    {
        // Mostra o painel de Game Over.
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }

        // Pausa o tempo do jogo.
        // Isso interrompe a física e as
        // movimentações dependentes de deltaTime.
        Time.timeScale = 0f;

        Debug.Log("Game Over!");
    }

    // Chamado pelo botão Retry.
    public void Retry()
    {
        // Restaura o tempo antes de reiniciar.
        Time.timeScale = 1f;

        // Obtém a cena atual.
        int currentScene =
            SceneManager.GetActiveScene().buildIndex;

        // Recarrega a cena atual.
        SceneManager.LoadScene(currentScene);
    }
}
