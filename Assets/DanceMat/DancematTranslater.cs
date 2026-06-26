using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class DancematTranslater : MonoBehaviour
{
    private Vector2 matMovement = Vector2.zero;
    private Vector2 matLookMovement = Vector2.zero;
    private bool matJumped = false;
    private bool matInteracted = false;
    private bool matSpellCast = false;
    private bool matMapToggled = false;
    private bool matDialogNext = false;
    private bool matDialogBack = false;

    void Update()
    {
        ReadInputsFromDevices();
    }

    private void ReadInputsFromDevices()
    {
        matMovement = Vector2.zero;
        matLookMovement = Vector2.zero;
        matJumped = false;
        matInteracted = false;
        matSpellCast = false;
        matMapToggled = false;
        matDialogNext = false;
        matDialogBack = false;

        if (TryReadInputs(Joystick.current)) return;
        foreach (var joystick in Joystick.all)
            if (TryReadInputs(joystick)) return;
        if (TryReadInputs(Gamepad.current)) return;
        foreach (var gamepad in Gamepad.all)
            if (TryReadInputs(gamepad)) return;
    }

    public Vector2 GetMatMovement() => matMovement;
    public Vector2 GetMatLookMovement() => matLookMovement;
    public bool PlayerJumpedThisFrame() => matJumped;
    public bool PlayerSelectedThisFrame() => matInteracted;
    public bool SpellCastThisFrame() => matSpellCast;
    public bool MapToggledThisFrame() => matMapToggled;
    public bool DialogNextThisFrame() => matDialogNext;
    public bool DialogBackThisFrame() => matDialogBack;

    private bool TryReadInputs(InputDevice device)
    {
        if (device == null) return false;

        bool shiftHeld = false;
        bool leftArrow = false;
        bool rightArrow = false;
        bool forwardArrow = false;
        bool backwardArrow = false;
        bool selectJustPressed = false;

        foreach (var control in device.allControls)
        {
            if (control is not ButtonControl button) continue;
            string path = button.path;

            // hinten links = shift modifier
            if (path == "/Gamepad/leftShoulder" && button.IsPressed()) shiftHeld = true;
            // pfeil nach links
            if (path == "/Gamepad/buttonWest" && button.IsPressed()) leftArrow = true;
            // pfeil nach rechts
            if (path == "/Gamepad/buttonNorth" && button.IsPressed()) rightArrow = true;
            // pfeil nach vorne
            if (path == "/Gamepad/buttonEast" && button.IsPressed()) forwardArrow = true;
            // pfeil nach hinten
            if (path == "/Gamepad/buttonSouth" && button.IsPressed()) backwardArrow = true;
            // hinten rechts = hint menu
            if (path == "/Gamepad/rightShoulder" && button.wasPressedThisFrame) matMapToggled = true;
            // select
            if (path == "/Gamepad/select" && button.wasPressedThisFrame) selectJustPressed = true;
            // start = spell
            if (path == "/Gamepad/start" && button.wasPressedThisFrame) matSpellCast = true;
        }

        // select allein = interact / mit shift = dialog zurück
        if (selectJustPressed)
        {
            if (shiftHeld) matDialogBack = true;
            else matInteracted = true;
        }

        // links + rechts gleichzeitig = jump (unabhängig von shift)
        matJumped = leftArrow && rightArrow;

        if (shiftHeld)
        {
            // shift + links = nach links laufen
            if (leftArrow && !rightArrow) matMovement.x = -1f;
            // shift + rechts = nach rechts laufen
            if (rightArrow && !leftArrow) matMovement.x = 1f;
            // shift + vorne = nach oben schauen
            if (forwardArrow) matLookMovement.y = 1f;
            // shift + hinten = nach unten schauen
            if (backwardArrow) matLookMovement.y = -1f;
        }
        else
        {
            // vorne = vorwärts laufen
            if (forwardArrow) matMovement.y = 1f;
            // hinten = rückwärts laufen
            if (backwardArrow) matMovement.y = -1f;
            // links/rechts = schauen (außer beim jump)
            if (!matJumped)
            {
                if (leftArrow) matLookMovement.x = -1f;
                if (rightArrow) matLookMovement.x = 1f;
            }
        }

        return matMovement != Vector2.zero || matLookMovement != Vector2.zero || matJumped
            || matInteracted || matSpellCast || matMapToggled || matDialogNext || matDialogBack;
    }
}