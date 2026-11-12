
using UnityEngine;
using UnityEngine.InputSystem;

public class DialogueInput : MonoBehaviour
{
    [Header("References")]
    public DialogueManager dialogueManager;

    // Chamado quando o jogador pressiona E
    // ou o botão configurado no controle.
    public void OnDialogueNext(
        InputAction.CallbackContext context)
    {
        // Executa somente quando o botão
        // é pressionado.
        if (!context.performed)
            return;

        Debug.Log("Botão de diálogo pressionado!");

        // Verifica se o DialogueManager existe.
        if (dialogueManager != null)
        {
            dialogueManager.NextLine();
        }
        else
        {
            Debug.LogWarning(
                "DialogueManager não configurado!"
            );
        }
    }
}
