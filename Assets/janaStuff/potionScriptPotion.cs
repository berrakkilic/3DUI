using UnityEngine;
using UnityEngine.InputSystem;

public class potionScriptPotion : MonoBehaviour
{
    public potionScript potionScript;

    public void OnTriggerStay(Collider other)
    {
        //Debug.Log("in trigger range");
        if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            //Debug.Log("pressed correct key");
            potionScript.pickedUpPotion = true;
        }
    }

    
}
