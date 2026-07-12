using UnityEngine;
using UnityEngine.InputSystem;

public class fluteScript : MonoBehaviour
{
    public fluteScriptPlayer fluteScriptPlayer;
    private DancematTranslater danceMat;

    private bool playerInZone;

    void Start()
    {
        danceMat = FindObjectOfType<DancematTranslater>();
        playerInZone = false;
    }

    public void OnTriggerEnter(Collider other)
    {
        playerInZone = true;
    }

    public void OnTriggerExit(Collider other)
    {
        playerInZone = false;
    }

    private void Update()
    {
        bool pressed = Keyboard.current.eKey.wasPressedThisFrame || (danceMat != null && danceMat.PlayerSelectedThisFrame());
        if (pressed && playerInZone)
        {
            //Debug.Log("pressed correct key, should pick up flute");
            fluteScriptPlayer.pickedUpFlute = true;
        }
    }

}
