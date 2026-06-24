using UnityEngine;
using UnityEngine.InputSystem;

public class MapToggle : MonoBehaviour
{
    [SerializeField] private GameObject mapPanel;
    [SerializeField] private MonoBehaviour[] disableWhileMapOpen;

    private bool isOpen;

    private void Start()
    {
        SetMapOpen(false);
    }

    private void Update()
    {
        if (Keyboard.current == null)
            return;

        if (Keyboard.current.mKey.wasPressedThisFrame)
        {
            SetMapOpen(!isOpen);
        }

        if (isOpen && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            SetMapOpen(false);
        }
    }

    private void SetMapOpen(bool open)
    {
        isOpen = open;

        if (mapPanel != null)
            mapPanel.SetActive(open);

        foreach (MonoBehaviour script in disableWhileMapOpen)
        {
            if (script != null)
                script.enabled = !open;
        }

        Cursor.visible = open;
        Cursor.lockState = open ? CursorLockMode.None : CursorLockMode.Locked;
    }
}