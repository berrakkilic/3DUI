using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class PlayerDialogueInteraction : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TMP_Text speakerNameText;
    [SerializeField] private TMP_Text dialogueText;

    private NPCDialogue currentNPC;
    private int currentLineIndex = 0;
    private bool dialogueOpen = false;
    private DancematTranslater danceMat;

    private void Awake()
    {
        danceMat = FindObjectOfType<DancematTranslater>();
    }

    void Update() {
        if (currentNPC == null) return;

        bool interactThisFrame = Keyboard.current.eKey.wasPressedThisFrame
            || (danceMat != null && danceMat.PlayerSelectedThisFrame());

        if (interactThisFrame)
        {
            if (!dialogueOpen)
                StartDialogue();
            else
                NextLine();
        }

        if (dialogueOpen && (danceMat != null && danceMat.DialogNextThisFrame()))
        {
            NextLine();
        }

        bool goBackThisFrame = Keyboard.current.rKey.wasPressedThisFrame
            || (danceMat != null && danceMat.DialogBackThisFrame());
        if (dialogueOpen && goBackThisFrame)
        {
            PreviousLine();
        }
    }

private void PreviousLine()
{
    currentLineIndex--;

    if (currentLineIndex < 0)
        currentLineIndex = 0;

    ShowLine();
}

    private void StartDialogue() {
        dialogueOpen = true;
        currentLineIndex = 0;
        dialoguePanel.SetActive(true);
        ShowLine();
    }

    private void NextLine() {
        currentLineIndex++;
        if (currentLineIndex >= currentNPC.dialogueLines.Length) {
            EndDialogue();
        } else {
            ShowLine();
        }
    }

    private void ShowLine() {
        speakerNameText.text = currentNPC.npcName;
        dialogueText.text = currentNPC.dialogueLines[currentLineIndex];
    }

    private void EndDialogue() {
        dialogueOpen = false;
        dialoguePanel.SetActive(false);
        currentLineIndex = 0;
    }

    private void OnTriggerEnter(Collider other) {
        NPCDialogue npc = other.GetComponent<NPCDialogue>();
        if (npc != null) {
            currentNPC = npc;
        }
    }

    private void OnTriggerExit(Collider other) {
        NPCDialogue npc = other.GetComponent<NPCDialogue>();
        if (npc != null && npc == currentNPC) {
            currentNPC = null;
            EndDialogue();
        }

    }
}
