using UnityEngine;

public class PlayerMap : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform player;
    [SerializeField] private RectTransform mapImageRect;
    [SerializeField] private RectTransform playerIcon;

    [Header("World Map Bounds")]
    [SerializeField] private Vector2 worldBottomLeft;
    [SerializeField] private Vector2 worldTopRight;

    [Header("Options")]
    [SerializeField] private bool rotateWithPlayer = true;
    [SerializeField] private float rotationOffset = 0f;

    private void Update()
    {
        if (player == null || mapImageRect == null || playerIcon == null)
            return;

        UpdateIconPosition();
    }

    private void UpdateIconPosition()
    {
        // World X/Z position of the player
        Vector2 playerWorldPosition = new Vector2(player.position.x, player.position.z);

        // Convert world position into 0-1 map percentage
        float normalizedX = Mathf.InverseLerp(worldBottomLeft.x, worldTopRight.x, playerWorldPosition.x);
        float normalizedY = Mathf.InverseLerp(worldBottomLeft.y, worldTopRight.y, playerWorldPosition.y);

        // Clamp so the icon stays inside the map
        normalizedX = Mathf.Clamp01(normalizedX);
        normalizedY = Mathf.Clamp01(normalizedY);

        // Convert 0-1 position into UI position
        float mapWidth = mapImageRect.rect.width;
        float mapHeight = mapImageRect.rect.height;

        Vector2 iconPosition = new Vector2(
            (normalizedX - 0.5f) * mapWidth,
            (normalizedY - 0.5f) * mapHeight
        );

        playerIcon.anchoredPosition = iconPosition;

        // Optional: rotate icon based on player direction
        if (rotateWithPlayer)
        {
            playerIcon.localEulerAngles = new Vector3(
                0,
                0,
                -player.eulerAngles.y + rotationOffset
            );
        }
    }
}