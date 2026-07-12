using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class potionScript : MonoBehaviour
{
    public bool pickedUpPotion;
    public GameObject potion;
    public GameObject portalEntrance;
    public GameObject carryPotion;
    public bool dropOnce;
    private BreadcrumbsPath breadcrumbsPath;

    [Header("Drink Message UI")]
    [SerializeField] private GameObject drinkMessagePanel;
    [SerializeField] private TMP_Text drinkMessageText;
    [SerializeField] private string drinkMessage = "Ahh... tastes bad.";
    [SerializeField] private float messageDuration = 2.5f;

    private DancematTranslater danceMat;
    private BeaconManager beaconManager;
    private Coroutine messageRoutine;

    void Start()
    {
        pickedUpPotion = false;
        potion.SetActive(true);
        carryPotion.SetActive(false);
        portalEntrance.SetActive(false);
        drinkMessagePanel.SetActive(false);
        dropOnce = false;

        danceMat = FindObjectOfType<DancematTranslater>();
        beaconManager = FindObjectOfType<BeaconManager>();
        breadcrumbsPath = FindObjectOfType<BreadcrumbsPath>();
    }

    void Update()
    {
        if (pickedUpPotion)
        {
            potion.SetActive(false);
            carryPotion.SetActive(true);
        }


        if (((Keyboard.current.eKey.wasPressedThisFrame) || (danceMat != null && danceMat.PlayerSelectedThisFrame())) && !dropOnce && pickedUpPotion)
        {
            DrinkPotion();
        }
    }

    private void DrinkPotion()
    {
        dropOnce = true;
        pickedUpPotion = false;

        if (beaconManager != null)
        {
            beaconManager.OnPotionDrunk();
        }

        if (breadcrumbsPath != null)
        {
            breadcrumbsPath.DisableBreadcrumbs();
        }
            

        ShowDrinkMessage();

        //Vector3 portalPosition = transform.position + transform.rotation * new Vector3(2.0f, 0.0f, 1.0f);

        carryPotion.SetActive(false);
        portalEntrance.SetActive(true);
            //portalEntrance.transform.position = portalPosition;
        
    }

    private void ShowDrinkMessage()
    {
        if (drinkMessagePanel == null || drinkMessageText == null)
            return;

        drinkMessageText.text = drinkMessage;
        drinkMessagePanel.SetActive(true);

        if (messageRoutine != null)
            StopCoroutine(messageRoutine);

        messageRoutine = StartCoroutine(HideDrinkMessageAfterDelay());
    }

    private IEnumerator HideDrinkMessageAfterDelay()
    {
        yield return new WaitForSeconds(messageDuration);

        if (drinkMessagePanel != null)
            drinkMessagePanel.SetActive(false);

        messageRoutine = null;
    }
}