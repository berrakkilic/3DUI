using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class DancematTranslater : MonoBehaviour
{
    private Vector2 matMovement = Vector2.zero;
    private Vector2 matLookMovement = Vector2.zero;
    private bool matJumped = false;

    void Update()
    {
        ReadInputsFromDevices(out matMovement, out matLookMovement, out matJumped);
    }

    private void ReadInputsFromDevices(out Vector2 movement, out Vector2 lookMovement, out bool jumped)
    {
        if (TryReadInputs(Joystick.current, out movement, out lookMovement, out jumped)) return;

        foreach (var joystick in Joystick.all)
        {
            if (TryReadInputs(joystick, out movement, out lookMovement, out jumped)) return;
        }

        if (TryReadInputs(Gamepad.current, out movement, out lookMovement, out jumped)) return;

        foreach (var gamepad in Gamepad.all)
        {
            if (TryReadInputs(gamepad, out movement, out lookMovement, out jumped)) return;
        }

        movement = Vector2.zero;
        lookMovement = Vector2.zero;
        jumped = false;
    }

    public Vector2 GetMatMovement() => matMovement;
    public Vector2 GetMatLookMovement() => matLookMovement;
    
    public bool PlayerJumpedThisFrame() => matJumped;

    private bool TryReadInputs(InputDevice device, out Vector2 movement, out Vector2 lookMovement, out bool jumped)
    {
        movement = Vector2.zero;
        lookMovement = Vector2.zero;
        jumped = false;

        if (device == null) return false;

        float x = 0f;
        float y = 0f;

        bool isLeftPressed = false;
        bool isRightPressed = false;

        foreach (var control in device.allControls)
        {
            if (control is not ButtonControl button || !button.IsPressed()) continue;

            string path = button.path;
            if (path == "/Gamepad/buttonSouth") y = -1f;
            if (path == "/Gamepad/buttonEast") y = 1f;

            if (path == "/Gamepad/buttonWest") isLeftPressed = true;
            if (path == "/Gamepad/buttonNorth") isRightPressed = true;

            if (path == "/Gamepad/leftShoulder") lookMovement.y = 1f;
            if (path == "/Gamepad/rightShoulder") lookMovement.y = -1f;
            if (path == "/Gamepad/select") lookMovement.x = -1f;
            if (path == "/Gamepad/start") lookMovement.x = 1f;
        }

        if (isLeftPressed && isRightPressed)
        {
            jumped = true;
            x = 0f;
        }
        else
        {
            if (isLeftPressed) x = -1f;
            if (isRightPressed) x = 1f;
        }

        movement = new Vector2(x, y);
        return movement != Vector2.zero || lookMovement != Vector2.zero || jumped;
    }
}