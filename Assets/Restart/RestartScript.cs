using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class RestartScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Update()
    {
        if (Keyboard.current.nKey.wasPressedThisFrame)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
