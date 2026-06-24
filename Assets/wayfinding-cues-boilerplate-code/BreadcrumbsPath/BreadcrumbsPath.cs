using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class BreadcrumbsPath : MonoBehaviour
{
    [Header("Marker Configuration")]
    [SerializeField] private GameObject[] markers;
    [SerializeField] private float markerDistance = 5.0f;
    [SerializeField] private int skipAFewMarkers = 2;
    [SerializeField] private float markerYOffset = 0.05f;

    [Header("Spell Configuration")]
    [SerializeField] private float revealDuration = 5.0f;
    [SerializeField] private bool recalculateWhileVisible = true;

    [Header("Path Configuration")]
    [SerializeField] private Transform player;
    [SerializeField] private Transform target;
    [SerializeField] private float navMeshSampleDistance = 5.0f;

    private NavMeshPath currentPath;
    private Coroutine revealRoutine;

    private void Awake()
    {
        currentPath = new NavMeshPath();
        HideMarkers();
    }

    private void Update()
    {
        if (Keyboard.current == null)
            return;

        if (Keyboard.current.bKey.wasPressedThisFrame)
        {
            CastBreadcrumbSpell();
        }
    }

    private void CastBreadcrumbSpell()
    {
        if (revealRoutine != null)
        {
            StopCoroutine(revealRoutine);
        }

        revealRoutine = StartCoroutine(RevealBreadcrumbsRoutine());
    }

    private IEnumerator RevealBreadcrumbsRoutine()
    {
        float timer = 0f;

        while (timer < revealDuration)
        {
            if (recalculateWhileVisible || timer == 0f)
            {
                if (TryCalculatePath(out Vector3[] pathCorners))
                {
                    UpdateMarkers(pathCorners);
                }
                else
                {
                    HideMarkers();
                }
            }

            timer += Time.deltaTime;
            yield return null;
        }

        HideMarkers();
        revealRoutine = null;
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
}