using UnityEngine;

public class potionScriptPotion : MonoBehaviour
{
    public potionScript potionScript;

    [SerializeField] private Transform player;

    private bool potionWasTaken = false;

    private void OnTriggerEnter(Collider other)
    {
        TryPickup(other);
    }

    private void OnTriggerStay(Collider other)
    {
        TryPickup(other);
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