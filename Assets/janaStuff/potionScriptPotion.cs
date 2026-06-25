using UnityEngine;

public class potionScriptPotion : MonoBehaviour
{
    public potionScript potionScript;

    public void OnTriggerEnter(Collider other)
    {
        potionScript.pickedUpPotion = true;
    }
}
