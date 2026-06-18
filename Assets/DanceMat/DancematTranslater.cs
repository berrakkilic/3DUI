using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class DancematTranslater : MonoBehaviour
{
    private Vector2 matMovement = Vector2.zero;
    private Vector2 matLookMovement = Vector2.zero;
    private bool matJumped = false;
    private bool matSelected = false;

    void Update()
    {
        ReadInputsFromDevices(out matMovement, out matLookMovement, out matJumped, out matSelected);
    }

    private void ReadInputsFromDevices(out Vector2 movement, out Vector2 lookMovement, out bool jumped, out bool selected)
    {
        if (TryReadInputs(Joystick.current, out movement, out lookMovement, out jumped, out selected)) return;

        foreach (var joystick in Joystick.all)
        {
            if (TryReadInputs(joystick, out movement, out lookMovement, out jumped, out selected)) return;
        }

        if (TryReadInputs(Gamepad.current, out movement, out lookMovement, out jumped, out selected)) return;

        foreach (var gamepad in Gamepad.all)
        {
            if (TryReadInputs(gamepad, out movement, out lookMovement, out jumped, out selected)) return;
        }

        movement = Vector2.zero;
        lookMovement = Vector2.zero;
        jumped = false;
        selected = false;
    }

    public Vector2 GetMatMovement() => matMovement;
    public Vector2 GetMatLookMovement() => matLookMovement;
    public bool PlayerJumpedThisFrame() => matJumped;
    public bool PlayerSelectedThisFrame() => matSelected;

    private bool TryReadInputs(InputDevice device, out Vector2 movement, out Vector2 lookMovement, out bool jumped, out bool selected)
    {
        movement = Vector2.zero;
        lookMovement = Vector2.zero;
        jumped = false;
        selected = false;

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

        selected = isLeftPressed && isRightPressed;

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
        return movement != Vector2.zero || lookMovement != Vector2.zero || jumped || selected;
    }
}