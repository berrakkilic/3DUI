using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build.Content;
using UnityEngine;
using UnityEngine.UIElements;

public class CompassBarElement : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Transform target;
    [SerializeField] private bool useFixDirection = false;
    [SerializeField] private Vector3 fixDirection;

    private CompassBar bar;
    private RectTransform _rectTransform;

    private void Start()
    {
        _rectTransform = GetComponent<RectTransform>();
        bar = GetComponentInParent<CompassBar>();
    }

    private void Update()
    {
    Vector3 directionToTarget;
        if (useFixDirection) {
            directionToTarget = fixDirection;
        } else {
            directionToTarget = target.position - player.position;
        }

        directionToTarget.y = 0f;
        Vector3 playerForward = player.forward;
        playerForward.y = 0f;
        float angle = Vector3.SignedAngle(playerForward, directionToTarget, Vector3.up);
        float mappedAngle = angle / bar.BarRange;
        float xPosition = mappedAngle * (360f / bar.BarRange) * (bar.BarRectTransform.rect.width / 2f);

        _rectTransform.anchoredPosition = new Vector2(xPosition, _rectTransform.anchoredPosition.y);
    }
}