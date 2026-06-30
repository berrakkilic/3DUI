using UnityEngine;

public class StartFacingTarget : MonoBehaviour
{
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private Transform target;

    private void Start()
    {
        if (cameraTransform == null || target == null)
            return;

        Vector3 direction = target.position - cameraTransform.position;
        direction.y = 0f;

        if (direction.sqrMagnitude < 0.001f)
            return;

        cameraTransform.rotation = Quaternion.LookRotation(direction);
    }
}