using UnityEngine;

public class potionScript : MonoBehaviour
{
    public bool pickedUpPotion;
    public GameObject potion;
    public GameObject portalEntrance;
    public GameObject carryPotion;
    public bool dropForTest;
    public bool dropOnce;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        pickedUpPotion = false;
        potion.SetActive(true);
        carryPotion.SetActive(false);
        portalEntrance.SetActive(false);
        dropForTest = false;
        dropOnce = false;
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

        if (dropForTest && !dropOnce) //Input.GetKeyDown(KeyCode.E)
        {
            dropOnce = true;
            pickedUpPotion = false;
            carryPotion.SetActive(false);
            portalEntrance.SetActive(true);
            portalEntrance.transform.position = carryPotion.transform.position + new Vector3(0.0f, -1.0f, 0.0f);
        }
    }

    /*public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "potion")
        {
            pickedUpPotion = true;
        }
    }*/
}
