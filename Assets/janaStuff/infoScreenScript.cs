using UnityEngine;
using UnityEngine.InputSystem;

public class infoScreenScript : MonoBehaviour
{
    public GameObject infoScreen;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            infoScreen.SetActive(false);
        }
    }
}
