
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private DialogueManager dialogueManager;

    [Header("Dialogue")]
    [SerializeField]
    private DialogueLine[] dialogueLines;

    [Header("Settings")]
    [SerializeField]
    private bool triggerOnlyOnce = true;

    private bool hasTriggered = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Verifica se quem entrou foi o jogador.
        if (!other.CompareTag("Player"))
            return;

        // Impede que o diálogo seja repetido.
        if (triggerOnlyOnce && hasTriggered)
            return;

        // Verifica se existe um DialogueManager.
        if (dialogueManager == null)
        {
            Debug.LogWarning(
                "DialogueManager não configurado!"
            );
            return;
        }

        // Inicia o diálogo.
        dialogueManager.StartDialogue(dialogueLines);

        // Registra que o diálogo foi ativado.
        hasTriggered = true;
    }
}
