using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    private float playerSpeed = 5.0f;
    private float jumpHeight = 1.5f;
    private float gravityValue = -9.81f;

    public CharacterController controller;
    private Vector3 playerVelocity;
    private bool groundedPlayer;

    private InputManager inputManager;
    private Transform cameraTransform;

    private void Start()
    {
        controller = GetComponent<CharacterController>();

        inputManager = InputManager.Instance;

        cameraTransform = Camera.main.transform;
    }

void Update()
    {
        groundedPlayer = controller.isGrounded;

        if (groundedPlayer && playerVelocity.y < -2f)
        {
            playerVelocity.y = -2f;
        }

        // Read WASD input
        Vector2 input = inputManager.GetPlayerMovement();

        Vector3 move = new Vector3(input.x, 0f, input.y);
        move = Vector3.ClampMagnitude(move, 1f);

        // Make movement relative to camera
        Vector3 cameraForward = cameraTransform.forward;
        Vector3 cameraRight = cameraTransform.right;

        cameraForward.y = 0f;
        cameraRight.y = 0f;

        cameraForward.Normalize();
        cameraRight.Normalize();

        move = cameraForward * move.z + cameraRight * move.x;

        // Rotate player toward movement direction
        if (move != Vector3.zero)
        {
            transform.forward = move;
        }

        // Jump
        if (groundedPlayer && inputManager.PlayerJumpedThisFrame())
        {
            playerVelocity.y = Mathf.Sqrt(jumpHeight * -2f * gravityValue);
        }

        // Gravity
        playerVelocity.y += gravityValue * Time.deltaTime;

        // Move
        Vector3 finalMove = move * playerSpeed + Vector3.up * playerVelocity.y;
        controller.Move(finalMove * Time.deltaTime);
    }
}
