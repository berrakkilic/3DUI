using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.SceneManagement;

public class introScript : MonoBehaviour
{
    void Update()
    {
        if (Keyboard.current.nKey.wasPressedThisFrame || StartButtonPressed())
        {
            SceneManager.LoadScene("Berrak");
        }
    }

    private bool StartButtonPressed()
    {
        foreach (var device in InputSystem.devices)
        {
            foreach (var control in device.allControls)
            {
                if (control is not ButtonControl button) continue;
                // Linux: Gamepad-Layout
                if (button.path.EndsWith("/start") && button.wasPressedThisFrame) return true;
                // Windows: Joystick-Layout
                if (button.name == "button10" && button.wasPressedThisFrame) return true;
            }
        }
        return false;
    }
}
