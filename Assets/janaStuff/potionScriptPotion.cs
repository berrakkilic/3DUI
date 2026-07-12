using UnityEngine;
using UnityEngine.InputSystem;

public class potionScriptPotion : MonoBehaviour
{
    public potionScript potionScript;
    private DancematTranslater danceMat;
    public bool playerInZone;

    //[SerializeField] private Transform player;

    private bool potionWasTaken = false;

    private void Start()
    {
        danceMat = FindObjectOfType<DancematTranslater>();
        playerInZone = false;
    }

    /*private void OnTriggerEnter(Collider other)
    {
        if (Keyboard.current.eKey.wasPressedThisFrame
            || (danceMat != null && danceMat.PlayerSelectedThisFrame()))
        {
            TryPickup(other);
        }
    }*/

    private void OnTriggerEnter(Collider other)
    {
        playerInZone = true;
    }

    private void OnTriggerExit(Collider other)
    {
        playerInZone = false;
    }

    private void Update()
    {
        bool pressed = Keyboard.current.eKey.wasPressedThisFrame || (danceMat != null && danceMat.PlayerSelectedThisFrame());
        if (pressed && playerInZone)
        {
            TryPickup();
        }
    }
    /*private void OnTriggerStay(Collider other)
    {
        Debug.Log("in trigger zone");
        
        Debug.Log("pressed: " + pressed);
        if (pressed)
        {
            Debug.Log("pressed the correct key");
            TryPickup(other);
        }
    }*/

    private void TryPickup()
    {
        if (potionWasTaken)
        {
            return;
        }

        /*if (player != null && other.transform.root != player)
            return;*/

        potionWasTaken = true;

        if (potionScript != null)
        {
            potionScript.pickedUpPotion = true;
        }
    }
}