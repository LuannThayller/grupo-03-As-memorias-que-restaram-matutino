using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenuController : MonoBehaviour
{
    [Header("Scene Settings")]

    // Nome da cena que será carregada ao clicar em Play.
    [SerializeField]
    private string gameSceneName = "GameScene";

    private void Start()
    {
        // Garante que o jogo não esteja pausado.
        Time.timeScale = 1f;
    }

    // Inicia o jogo.
    public void PlayGame()
    {
        // Carrega a cena principal.
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayButtonClick();
        }

        SceneManager.LoadScene(gameSceneName);
    }

    // Fecha o jogo.
    public void ExitGame()
    {
        Debug.Log("Saindo do jogo...");

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayButtonClick();
        }

        Application.Quit();
    }
}
