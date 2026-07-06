using UnityEngine;
using UnityEngine.InputSystem;

public class infoScreenScript : MonoBehaviour
{
    public GameObject infoScreen;
    public GameObject secondScreen;
    public GameObject panel;
    private DancematTranslater danceMat;
    public bool doSecondScreen;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        danceMat = FindObjectOfType<DancematTranslater>();
        doSecondScreen = true;
    }

    // Update is called once per frame
    void Update()
    {
        bool pressed = Keyboard.current.eKey.wasPressedThisFrame
            || (danceMat != null && danceMat.PlayerSelectedThisFrame());
        if (pressed)
        {
            if (doSecondScreen)
            {
                infoScreen.SetActive(false);
                secondScreen.SetActive(true);
                doSecondScreen= false;
            }
            else
            {
                secondScreen.SetActive(false);
                panel.SetActive(false);
                doSecondScreen = true;
            }
        }
    }
}
