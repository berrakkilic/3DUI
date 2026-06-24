using System;
using System.Collections.Generic;
using UnityEngine;

public class CalibratedMap : MonoBehaviour
{
    [Serializable]
    public class CalibrationPoint
    {
        public string name;
        public Transform worldPoint;
        public RectTransform mapPoint;
    }

    [Header("References")]
    [SerializeField] private Transform player;
    [SerializeField] private RectTransform playerIcon;

    [Header("Calibration Points")]
    [SerializeField] private List<CalibrationPoint> points = new List<CalibrationPoint>();

    [Header("Tuning")]
    [SerializeField] private float weightPower = 6f;

    [Header("Rotation")]
    [SerializeField] private bool rotateWithPlayer = true;
    [SerializeField] private float rotationOffset = 180f;

    private void Update()
    {
        if (player == null || playerIcon == null || points.Count == 0)
            return;

        UpdatePosition();
        UpdateRotation();
    }

    private void UpdatePosition()
    {
        Vector2 playerWorld = new Vector2(player.position.x, player.position.z);

        Vector2 weightedMapPosition = Vector2.zero;
        float totalWeight = 0f;

        foreach (CalibrationPoint point in points)
        {
            if (point.worldPoint == null || point.mapPoint == null)
                continue;

            Vector2 pointWorld = new Vector2(
                point.worldPoint.position.x,
                point.worldPoint.position.z
            );

            float distance = Vector2.Distance(playerWorld, pointWorld);

            if (distance < 0.1f)
            {
                playerIcon.anchoredPosition = point.mapPoint.anchoredPosition;
                return;
            }

            float weight = 1f / Mathf.Pow(distance, weightPower);

            weightedMapPosition += point.mapPoint.anchoredPosition * weight;
            totalWeight += weight;
        }

        if (totalWeight > 0f)
        {
            playerIcon.anchoredPosition = weightedMapPosition / totalWeight;
        }
    }

    private void UpdateRotation()
    {
        if (!rotateWithPlayer)
            return;

        playerIcon.localEulerAngles = new Vector3(
            0f,
            0f,
            -player.eulerAngles.y + rotationOffset
        );
    }
}