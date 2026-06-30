using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class MapToggle : MonoBehaviour
{
    [SerializeField] private GameObject mapPanel;
    [SerializeField] private MonoBehaviour[] disableWhileMapOpen;
    [SerializeField] private CalibratedMap calibratedMap;

    private bool isOpen;
    private DancematTranslater danceMat;

    private void Start()
    {
        SetMapOpen(false);
        danceMat = FindObjectOfType<DancematTranslater>();
    }

    private void Update()
    {
        bool toggleMap = (Keyboard.current != null && Keyboard.current.mKey.wasPressedThisFrame)
                       || (danceMat != null && danceMat.MapDirectThisFrame());

        if (toggleMap)
        {
            SetMapOpen(!isOpen);
        }

        bool closeMap = (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame);
        if (isOpen && closeMap)
        {
            SetMapOpen(false);
        }
    }

    public void SetMapOpen(bool open)
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

        if (open && calibratedMap != null)
            StartCoroutine(RefreshMapAfterOneFrame());
    }

    private IEnumerator RefreshMapAfterOneFrame()
    {
        yield return null;
        calibratedMap.RefreshMap();
    }
}