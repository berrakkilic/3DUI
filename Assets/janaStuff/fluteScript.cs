using UnityEngine;
using UnityEngine.InputSystem;

public class fluteScript : MonoBehaviour
{
    public fluteScriptPlayer fluteScriptPlayer;

    public void OnTriggerStay(Collider other)
    {
        Debug.Log("got into trigger zone");
        if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            Debug.Log("pressed correct key, should pick up flute");
            fluteScriptPlayer.pickedUpFlute = true;
        }
    }
}
