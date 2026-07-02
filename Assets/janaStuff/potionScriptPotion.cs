using UnityEngine;
using UnityEngine.InputSystem;

public class potionScriptPotion : MonoBehaviour
{
    public potionScript potionScript;
    private DancematTranslater danceMat;

    [SerializeField] private Transform player;

    private bool potionWasTaken = false;

    private void Start()
    {
        danceMat = FindObjectOfType<DancematTranslater>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (Keyboard.current.eKey.wasPressedThisFrame
            || (danceMat != null && danceMat.PlayerSelectedThisFrame()))
        {
            TryPickup(other);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(Keyboard.current.eKey.wasPressedThisFrame
            || (danceMat != null && danceMat.PlayerSelectedThisFrame()))
        { 
            TryPickup(other);
        }
    }

    private void TryPickup(Collider other)
    {
        if (potionWasTaken)
            return;

        if (player != null && other.transform.root != player)
            return;

        potionWasTaken = true;

        if (potionScript != null)
        {
            potionScript.pickedUpPotion = true;
        }
    }
}