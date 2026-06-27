using UnityEngine;

public class fluteScriptPlayer : MonoBehaviour
{
    public bool pickedUpFlute;
    public GameObject spinningFlute;
    public GameObject handFlute;
    public GameObject winScreen;
    public GameObject confetti;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        pickedUpFlute = false;
        handFlute.SetActive(false);
        winScreen.SetActive(false);
        confetti.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (pickedUpFlute)
        { 
            spinningFlute.SetActive(false);
            handFlute.SetActive(true);
            winScreen.SetActive(true);
            confetti.SetActive(true);
        }
    }
}
