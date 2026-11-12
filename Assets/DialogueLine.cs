using System;
using UnityEngine;

[Serializable]
public class DialogueLine
{
    [Header("Character")]
    public string characterName;

    [Header("Dialogue")]
    [TextArea(3, 6)]
    public string dialogueText;
}
