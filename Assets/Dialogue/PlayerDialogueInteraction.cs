using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerDialogueInteraction : MonoBehaviour
{
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TMP_Text speakerNameText;
    [SerializeField] private TMP_Text dialogueText;

    [SerializeField] private BreadcrumbsPath breadcrumbsPath;
    [SerializeField] private BeaconManager beaconManager;
    [SerializeField] private Image dialoguePanelImage;

    [SerializeField] private Color playerColor = new Color(0.2f, 0.3f, 0.5f, 0.85f);
    [SerializeField] private Color npcColor = new Color(0.15f, 0.15f, 0.15f, 0.85f);
    [SerializeField] private Color wizardColor = new Color(0.35f, 0.2f, 0.5f, 0.85f);

    private NPCDialogue currentNPC;
    private int currentLineIndex = 0;
    private bool dialogueOpen = false;
    private DancematTranslater danceMat;

    private void Awake()
    {
        danceMat = FindObjectOfType<DancematTranslater>();

        if (breadcrumbsPath == null)
            breadcrumbsPath = FindObjectOfType<BreadcrumbsPath>();
        if (beaconManager == null)
            beaconManager = FindObjectOfType<BeaconManager>();
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
    Debug.Log("SHOWLINE RUNNING: " + currentNPC.dialogueLines[currentLineIndex]);
    string line = currentNPC.dialogueLines[currentLineIndex];

    dialogueText.text = line;
    speakerNameText.text = currentNPC.npcName;

    if (dialoguePanelImage == null)
        {
            Debug.LogWarning("Dialogue panel image is NULL");
            return;
        }

        Color newColor = npcColor;

        if (line.TrimStart().StartsWith("You:"))
        {
            newColor = playerColor;
        }
        else if (currentNPC.breadcrumbStoryRole == NPCDialogue.BreadcrumbStoryRole.Wizard)
        {
            newColor = wizardColor;
        }
        else
        {
            newColor = npcColor;
        }

        dialoguePanelImage.color = newColor;

        Debug.Log("Panel image is: " + dialoguePanelImage.name);
        Debug.Log("New panel color: " + dialoguePanelImage.color);
}

    private void EndDialogue(bool completedDialogue)
    {
        if (completedDialogue && currentNPC != null)
        {
            if (breadcrumbsPath != null)
                breadcrumbsPath.OnNPCDialogueCompleted(currentNPC);
            if (beaconManager != null)
                beaconManager.OnNPCDialogueCompleted(currentNPC);
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