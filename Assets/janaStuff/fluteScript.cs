using UnityEngine;
using UnityEngine.InputSystem;

public class fluteScript : MonoBehaviour
{
    public fluteScriptPlayer fluteScriptPlayer;

    public void OnTriggerStay(Collider other)
    {
        if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            fluteScriptPlayer.pickedUpFlute = true;
        }
    }
}
