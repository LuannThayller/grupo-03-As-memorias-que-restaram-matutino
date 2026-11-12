
using System.Collections;
using TMPro;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    [Header("UI References")]
    public GameObject dialoguePanel;
    public TMP_Text characterNameText;
    public TMP_Text dialogueText;

    [Header("Settings")]
    public float typingSpeed = 0.035f;

    [Header("Player References")]
    public PlayerMovement playerMovement;
    public PlayerAttack playerAttack;

    private DialogueLine[] currentDialogue;
    private int currentLineIndex;

    private Coroutine typingCoroutine;
    private bool isTyping;
    private bool isDialogueActive;

    private void Awake()
    {
        dialoguePanel.SetActive(false);
    }

    // Inicia uma sequência de diálogos.
    public void StartDialogue(DialogueLine[] lines)
    {
        if (lines == null || lines.Length == 0)
            return;

        if (isDialogueActive)
            return;

        currentDialogue = lines;
        currentLineIndex = 0;
        isDialogueActive = true;

        dialoguePanel.SetActive(true);

        // Bloqueia o controle durante o diálogo.
        if (playerMovement != null)
            playerMovement.enabled = false;

        if (playerAttack != null)
            playerAttack.enabled = false;

        ShowCurrentLine();
    }

    // Mostra a fala atual.
    private void ShowCurrentLine()
    {
        DialogueLine line =
            currentDialogue[currentLineIndex];

        characterNameText.text = line.characterName;

        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        typingCoroutine = StartCoroutine(
            TypeSentence(line.dialogueText)
        );
    }

    // Revela o texto progressivamente.
    private IEnumerator TypeSentence(string sentence)
    {
        isTyping = true;

        dialogueText.text = sentence;
        dialogueText.maxVisibleCharacters = 0;

        // Atualiza o layout do TextMeshPro.
        dialogueText.ForceMeshUpdate();

        int totalCharacters =
            dialogueText.textInfo.characterCount;

        for (int i = 0; i < totalCharacters; i++)
        {
            dialogueText.maxVisibleCharacters = i + 1;

            yield return new WaitForSecondsRealtime(
                typingSpeed
            );
        }

        isTyping = false;
        typingCoroutine = null;
    }

    // Avança o diálogo.
    public void NextLine()
    {
        if (!isDialogueActive)
            return;

        // Se ainda está digitando, revela tudo.
        if (isTyping)
        {
            if (typingCoroutine != null)
                StopCoroutine(typingCoroutine);

            dialogueText.maxVisibleCharacters =
                int.MaxValue;

            isTyping = false;
            typingCoroutine = null;
            return;
        }

        currentLineIndex++;

        // Se ainda existem falas, mostra a próxima.
        if (currentLineIndex < currentDialogue.Length)
        {
            ShowCurrentLine();
        }
        else
        {
            EndDialogue();
        }
    }

    // Finaliza a sequência.
    private void EndDialogue()
    {
        isDialogueActive = false;

        dialoguePanel.SetActive(false);

        // Devolve o controle ao jogador.
        if (playerMovement != null)
            playerMovement.enabled = true;

        if (playerAttack != null)
            playerAttack.enabled = true;
    }
}
