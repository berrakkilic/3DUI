using UnityEngine;

public class NPCDialogue : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public string npcName;

    [TextArea(3, 8)]

    public string[] dialogueLines;
}
