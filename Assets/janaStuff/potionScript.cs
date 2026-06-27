using UnityEngine;
using UnityEngine.InputSystem;

public class potionScript : MonoBehaviour
{
    public bool pickedUpPotion;
    public GameObject potion;
    public GameObject portalEntrance;
    public GameObject carryPotion;
    public bool dropOnce;
    private DancematTranslater danceMat;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        pickedUpPotion = false;
        potion.SetActive(true);
        carryPotion.SetActive(false);
        portalEntrance.SetActive(false);
        dropOnce = false;
        danceMat = FindObjectOfType<DancematTranslater>();
    }

    // Update is called once per frame
    void Update()
    {
        if (pickedUpPotion)
        {
            //potion.transform.position = transform.position + transform.rotation * new Vector3(1.0f, 1.0f);
            potion.SetActive(false);
            carryPotion.SetActive(true);
        }

        bool dropPressed = (Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
            || (danceMat != null && danceMat.PlayerSelectedThisFrame());
        if (dropPressed && !dropOnce && pickedUpPotion)
        {
            dropOnce = true;
            pickedUpPotion = false;
            carryPotion.SetActive(false);
            portalEntrance.SetActive(true);
            portalEntrance.transform.position = carryPotion.transform.position + new Vector3(-2.0f, -1.0f, 1.0f);
        }
    }
}
