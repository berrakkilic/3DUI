using UnityEngine;

public class NPCDialogue : MonoBehaviour
{
    public enum BreadcrumbStoryRole
    {
        None,
        Villager,
        Wizard
    }

    public string npcName;

    public BreadcrumbStoryRole breadcrumbStoryRole = BreadcrumbStoryRole.None;

    [TextArea(3, 8)]
    public string[] dialogueLines;
}