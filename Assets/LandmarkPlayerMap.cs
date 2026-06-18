using UnityEngine;

public class LandmarkPlayerMap : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] private Transform player;
    [SerializeField] private RectTransform playerIcon;

    [Header("World Landmarks")]
    [SerializeField] private Transform tavernWorld;
    [SerializeField] private Transform gazeboWorld;
    [SerializeField] private Transform volcanoWorld;

    [Header("Map Landmarks")]
    [SerializeField] private RectTransform tavernMap;
    [SerializeField] private RectTransform gazeboMap;
    [SerializeField] private RectTransform volcanoMap;

    [Header("Rotation")]
    [SerializeField] private bool rotateWithPlayer = true;
    [SerializeField] private float rotationOffset = 180f;

    private void Update()
    {
        if (player == null || playerIcon == null)
            return;

        UpdatePlayerIconPosition();
        UpdatePlayerIconRotation();
    }

    private void UpdatePlayerIconPosition()
    {
        Vector2 playerPos = new Vector2(player.position.x, player.position.z);

        Vector2 tavernWorldPos = ToWorld2D(tavernWorld);
        Vector2 gazeboWorldPos = ToWorld2D(gazeboWorld);
        Vector2 volcanoWorldPos = ToWorld2D(volcanoWorld);

        Vector3 barycentric = GetBarycentric(
            playerPos,
            tavernWorldPos,
            gazeboWorldPos,
            volcanoWorldPos
        );

        Vector2 mapPos =
            barycentric.x * tavernMap.anchoredPosition +
            barycentric.y * gazeboMap.anchoredPosition +
            barycentric.z * volcanoMap.anchoredPosition;

        playerIcon.anchoredPosition = mapPos;
    }

    private void UpdatePlayerIconRotation()
    {
        if (!rotateWithPlayer)
            return;

        playerIcon.localEulerAngles = new Vector3(
            0,
            0,
            -player.eulerAngles.y + rotationOffset
        );
    }

    private Vector2 ToWorld2D(Transform target)
    {
        return new Vector2(target.position.x, target.position.z);
    }

    private Vector3 GetBarycentric(Vector2 p, Vector2 a, Vector2 b, Vector2 c)
    {
        Vector2 v0 = b - a;
        Vector2 v1 = c - a;
        Vector2 v2 = p - a;

        float d00 = Vector2.Dot(v0, v0);
        float d01 = Vector2.Dot(v0, v1);
        float d11 = Vector2.Dot(v1, v1);
        float d20 = Vector2.Dot(v2, v0);
        float d21 = Vector2.Dot(v2, v1);

        float denominator = d00 * d11 - d01 * d01;

        if (Mathf.Abs(denominator) < 0.0001f)
            return new Vector3(1, 0, 0);

        float v = (d11 * d20 - d01 * d21) / denominator;
        float w = (d00 * d21 - d01 * d20) / denominator;
        float u = 1.0f - v - w;

        return new Vector3(u, v, w);
    }
}