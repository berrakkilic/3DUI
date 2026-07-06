using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class introScript : MonoBehaviour
{
    private DancematTranslater danceMat;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        danceMat = FindObjectOfType<DancematTranslater>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Keyboard.current.nKey.wasPressedThisFrame
            || (danceMat != null && danceMat.RestartPressedThisFrame()))
        {
            SceneManager.LoadScene("Berrak");
        }
    }
}
