
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    [Header("Scene Settings")]

    // Nome da cena que será carregada.
    [SerializeField]
    private string sceneToLoad;

    // Evita carregar a cena várias vezes.
    private bool isTransitioning = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Verifica se o objeto que entrou
        // no Trigger é o jogador.
        if (!collision.CompareTag("Player"))
            return;

        // Impede múltiplas transições.
        if (isTransitioning)
            return;

        // Verifica se uma cena foi configurada.
        if (string.IsNullOrEmpty(sceneToLoad))
        {
            Debug.LogWarning(
                "SceneTransition: Nenhuma cena selecionada!"
            );
            return;
        }

        isTransitioning = true;

        // Garante que o jogo não esteja pausado.
        Time.timeScale = 1f;

        // Carrega a cena selecionada.
        SceneManager.LoadScene(sceneToLoad);
    }
}
