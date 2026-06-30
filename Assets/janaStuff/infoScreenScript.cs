using UnityEngine;
using UnityEngine.InputSystem;

public class infoScreenScript : MonoBehaviour
{
    public GameObject infoScreen;
    private DancematTranslater danceMat;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        danceMat = FindObjectOfType<DancematTranslater>();
    }

    // Update is called once per frame
    void Update()
    {
        bool pressed = Keyboard.current.eKey.wasPressedThisFrame
            || (danceMat != null && danceMat.PlayerSelectedThisFrame());
        if (pressed)
        {
            infoScreen.SetActive(false);
        }
    }
}
