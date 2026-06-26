using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class PlayerDialogueInteraction : MonoBehaviour
{
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TMP_Text speakerNameText;
    [SerializeField] private TMP_Text dialogueText;

    [SerializeField] private BreadcrumbsPath breadcrumbsPath;

    private NPCDialogue currentNPC;
    private int currentLineIndex = 0;
    private bool dialogueOpen = false;
    private DancematTranslater danceMat;

    private void Awake()
    {
        danceMat = FindObjectOfType<DancematTranslater>();

        if (breadcrumbsPath == null)
            breadcrumbsPath = FindObjectOfType<BreadcrumbsPath>();
    }

    void Update()
    {
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

    private void StartDialogue()
    {
        dialogueOpen = true;
        currentLineIndex = 0;
        dialoguePanel.SetActive(true);
        ShowLine();
    }

    private void NextLine()
    {
        currentLineIndex++;

        if (currentLineIndex >= currentNPC.dialogueLines.Length)
        {
            EndDialogue(true);
        }
        else
        {
            ShowLine();
        }
    }

    private void ShowLine()
    {
        speakerNameText.text = currentNPC.npcName;
        dialogueText.text = currentNPC.dialogueLines[currentLineIndex];
    }

    private void EndDialogue(bool completedDialogue)
    {
        if (completedDialogue && currentNPC != null && breadcrumbsPath != null)
        {
            breadcrumbsPath.OnNPCDialogueCompleted(currentNPC);
        }

        dialogueOpen = false;
        dialoguePanel.SetActive(false);
        currentLineIndex = 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        NPCDialogue npc = other.GetComponent<NPCDialogue>();

        if (npc != null)
        {
            currentNPC = npc;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        NPCDialogue npc = other.GetComponent<NPCDialogue>();

        if (npc != null && npc == currentNPC)
        {
            EndDialogue(false);
            currentNPC = null;
        }
    }
}