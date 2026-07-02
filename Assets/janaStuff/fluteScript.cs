using UnityEngine;
using UnityEngine.InputSystem;

public class fluteScript : MonoBehaviour
{
    public fluteScriptPlayer fluteScriptPlayer;
    private DancematTranslater danceMat;

    void Start()
    {
        danceMat = FindObjectOfType<DancematTranslater>();
    }

    public void OnTriggerStay(Collider other)
    {
        //Debug.Log("got into trigger zone");
        bool pressed = Keyboard.current.eKey.wasPressedThisFrame
            || (danceMat != null && danceMat.PlayerSelectedThisFrame());
        if (pressed)
        {
            //Debug.Log("pressed correct key, should pick up flute");
            fluteScriptPlayer.pickedUpFlute = true;
        }
    }
}
