using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build.Content;
using UnityEngine;
using UnityEngine.AI;

public class BreadcrumbsPath : MonoBehaviour
{
    [Header("Marker Configuration")]
    [SerializeField] private GameObject[] markers;
    [SerializeField] private Vector3 inactivePosition;
    [SerializeField] private float markerDistance = 5.0f;
    [SerializeField] private int skipAFewMarkers = 2;
    [Header("Agent Configuration")]
    [SerializeField] private Transform player;
    [SerializeField] private Transform target;

    private NavMeshPath _currentPath;
    private NavMeshAgent _navAgent;

    // Start is called before the first frame update
    private void Start()
    {
        _currentPath = new NavMeshPath();
    }

    // Update is called once per frame
    private void Update()
    {
        if (player == null || target == null) return;

        NavMeshHit playerHit;

        NavMeshHit targetHit;

        if (!NavMesh.SamplePosition(player.position, out playerHit, 5f, NavMesh.AllAreas)) return;

        if (!NavMesh.SamplePosition(target.position, out targetHit, 5f, NavMesh.AllAreas)) return;

        NavMesh.CalculatePath(

            playerHit.position,

            targetHit.position,

            NavMesh.AllAreas,

            _currentPath

        );

        UpdateMarkers(_currentPath.corners);
    }

    private void UpdateMarkers(Vector3[] path)
    {
        foreach (GameObject marker in markers)

        {

            marker.transform.position = inactivePosition;

        }

        if (path.Length < 2)
        {
            return;
        }

        var markerPositions = new List<Vector3>();
         for (int i = 0; i < path.Length - 1; i++)

        {

            Vector3 from = path[i];

            Vector3 to = path[i + 1];

            float distance = Vector3.Distance(from, to);

            int steps = Mathf.FloorToInt(distance / markerDistance);

            for (int j = 0; j < steps; j++)

            {

                float t = (j * markerDistance) / distance;

                Vector3 pos = Vector3.Lerp(from, to, t);

                markerPositions.Add(pos);

            }

        }

        int markerIndex = 0;

        for (int i = skipAFewMarkers; i < markerPositions.Count && markerIndex < markers.Length; i++)

        {

            Vector3 pos = markerPositions[i];

            pos.y += 0.05f;

            markers[markerIndex].transform.position = pos;

            markerIndex++;

        }
    }
}