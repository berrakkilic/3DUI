using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class RestartScript : MonoBehaviour
{
    private DancematTranslater danceMat;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        danceMat = FindObjectOfType<DancematTranslater>();
    }

    void Update()
    {
        bool restartPressed = (Keyboard.current != null && Keyboard.current.nKey.wasPressedThisFrame)
            || (danceMat != null && danceMat.RestartPressedThisFrame());

        if (restartPressed)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
