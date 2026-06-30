using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using TMPro;

public class BreadcrumbsPath : MonoBehaviour
{
    [Header("Marker Configuration")]
    [SerializeField] private GameObject[] markers;
    [SerializeField] private float markerDistance = 5.0f;
    [SerializeField] private int skipAFewMarkers = 2;
    [SerializeField] private float markerYOffset = 0.05f;

    [Header("Spell Configuration")]
    [SerializeField] private float revealDuration = 20.0f;

    [Header("Collectible Configuration")]
    [SerializeField] private bool markersAreCollectible = true;
    [SerializeField] private float collectDistance = 1.5f;

    [Header("Path Configuration")]
    [SerializeField] private Transform player;
    [SerializeField] private Transform target;
    [SerializeField] private float navMeshSampleDistance = 5.0f;

    [Header("Story Targets")]
    [SerializeField] private Transform villagerTarget;
    [SerializeField] private Transform wizardTarget;
    [SerializeField] private Transform potionTarget;
    
    [Header("End State")]
    [SerializeField] private bool breadcrumbsDisabled = false;
    [SerializeField] private GameObject noHintsMessagePanel;
    [SerializeField] private TMP_Text noHintsMessageText;
    [SerializeField] private string noHintsMessage = "No more hints available.";
    [SerializeField] private float noHintsMessageDuration = 2.5f;

    private Coroutine noHintsMessageRoutine;

    private enum BreadcrumbQuestStep
    {
        ToVillager,
        ToWizard,
        ToPotion
    }

    [SerializeField] private BreadcrumbQuestStep currentQuestStep = BreadcrumbQuestStep.ToVillager;

    private NavMeshPath currentPath;
    private Coroutine revealRoutine;
    private DancematTranslater danceMat;

    private void Awake()
    {
        currentPath = new NavMeshPath();
        SetTargetForCurrentQuestStep();
        HideMarkers();
        
        if (noHintsMessagePanel != null)
            noHintsMessagePanel.SetActive(false);
        
        danceMat = FindObjectOfType<DancematTranslater>();
    }

    private void Update()
    {
        bool pressed = (Keyboard.current != null && Keyboard.current.bKey.wasPressedThisFrame)
            || (danceMat != null && danceMat.BreadcrumbsThisFrame());

        if (pressed)
        {
            CastBreadcrumbSpell();
        }
    }

    public void CastBreadcrumbSpell()
    {
        if (breadcrumbsDisabled)
        {
            HideMarkers();
            ShowNoHintsMessage();
            return;
        }

        SetTargetForCurrentQuestStep();

        if (revealRoutine != null)
        {
            StopCoroutine(revealRoutine);
        }

        HideMarkers();

        if (TryCalculatePath(out Vector3[] pathCorners))
        {
            UpdateMarkers(pathCorners);
            revealRoutine = StartCoroutine(RevealBreadcrumbsRoutine());
        }
        else
        {
            Debug.LogWarning("Could not calculate breadcrumb path.");
        }
    }

    public void OnNPCDialogueCompleted(NPCDialogue npc)
    {
        if (npc == null)
            return;

        if (npc.breadcrumbStoryRole == NPCDialogue.BreadcrumbStoryRole.Villager
            && currentQuestStep == BreadcrumbQuestStep.ToVillager)
        {
            currentQuestStep = BreadcrumbQuestStep.ToWizard;
            SetTargetForCurrentQuestStep();
        }
        else if (npc.breadcrumbStoryRole == NPCDialogue.BreadcrumbStoryRole.Wizard
            && currentQuestStep == BreadcrumbQuestStep.ToWizard)
        {
            currentQuestStep = BreadcrumbQuestStep.ToPotion;
            SetTargetForCurrentQuestStep();
        }
    }

    private void SetTargetForCurrentQuestStep()
    {
        switch (currentQuestStep)
        {
            case BreadcrumbQuestStep.ToVillager:
                if (villagerTarget != null)
                    target = villagerTarget;
                break;

            case BreadcrumbQuestStep.ToWizard:
                if (wizardTarget != null)
                    target = wizardTarget;
                break;

            case BreadcrumbQuestStep.ToPotion:
                if (potionTarget != null)
                    target = potionTarget;
                break;
        }
    }

    private IEnumerator RevealBreadcrumbsRoutine()
    {
        float timer = 0f;

        while (timer < revealDuration)
        {
            if (markersAreCollectible)
            {
                CollectNearbyMarkers();
            }

            timer += Time.deltaTime;
            yield return null;
        }

        HideMarkers();
        revealRoutine = null;
    }

    private void CollectNearbyMarkers()
    {
        if (player == null)
            return;

        foreach (GameObject marker in markers)
        {
            if (marker == null || !marker.activeSelf)
                continue;

            float distanceToPlayer = Vector3.Distance(player.position, marker.transform.position);

            if (distanceToPlayer <= collectDistance)
            {
                marker.SetActive(false);
            }
        }
    }

    private bool TryCalculatePath(out Vector3[] pathCorners)
    {
        pathCorners = null;

        if (player == null || target == null)
            return false;

        bool foundPlayerPosition = NavMesh.SamplePosition(
            player.position,
            out NavMeshHit playerHit,
            navMeshSampleDistance,
            NavMesh.AllAreas
        );

        bool foundTargetPosition = NavMesh.SamplePosition(
            target.position,
            out NavMeshHit targetHit,
            navMeshSampleDistance,
            NavMesh.AllAreas
        );

        if (!foundPlayerPosition || !foundTargetPosition)
            return false;

        bool pathFound = NavMesh.CalculatePath(
            playerHit.position,
            targetHit.position,
            NavMesh.AllAreas,
            currentPath
        );

        if (!pathFound || currentPath.status == NavMeshPathStatus.PathInvalid)
            return false;

        if (currentPath.corners.Length < 2)
            return false;

        pathCorners = currentPath.corners;
        return true;
    }

    private void UpdateMarkers(Vector3[] path)
    {
        HideMarkers();

        List<Vector3> markerPositions = new List<Vector3>();

        for (int i = 0; i < path.Length - 1; i++)
        {
            Vector3 from = path[i];
            Vector3 to = path[i + 1];

            float distance = Vector3.Distance(from, to);

            if (distance <= 0.01f)
                continue;

            for (float d = 0; d < distance; d += markerDistance)
            {
                float t = d / distance;
                Vector3 position = Vector3.Lerp(from, to, t);
                markerPositions.Add(position);
            }
        }

        int markerIndex = 0;

        for (int i = skipAFewMarkers; i < markerPositions.Count && markerIndex < markers.Length; i++)
        {
            if (markers[markerIndex] == null)
                continue;

            Vector3 position = markerPositions[i];
            position.y += markerYOffset;

            markers[markerIndex].transform.position = position;
            markers[markerIndex].SetActive(true);

            markerIndex++;
        }
    }

    private void HideMarkers()
    {
        foreach (GameObject marker in markers)
        {
            if (marker != null)
            {
                marker.SetActive(false);
            }
        }
    }
    
    public void DisableBreadcrumbs()
    {
        breadcrumbsDisabled = true;

        if (revealRoutine != null)
        {
            StopCoroutine(revealRoutine);
            revealRoutine = null;
        }

        HideMarkers();
    }

    private void ShowNoHintsMessage()
    {
        if (noHintsMessagePanel == null || noHintsMessageText == null)
            return;

        noHintsMessageText.text = noHintsMessage;
        noHintsMessagePanel.SetActive(true);

        if (noHintsMessageRoutine != null)
            StopCoroutine(noHintsMessageRoutine);

        noHintsMessageRoutine = StartCoroutine(HideNoHintsMessageAfterDelay());
    }

    private IEnumerator HideNoHintsMessageAfterDelay()
    {
        yield return new WaitForSeconds(noHintsMessageDuration);

        if (noHintsMessagePanel != null)
            noHintsMessagePanel.SetActive(false);

        noHintsMessageRoutine = null;
    }
}