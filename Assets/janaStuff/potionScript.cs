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

        if (potion != null)
            potion.SetActive(true);

        if (carryPotion != null)
            carryPotion.SetActive(false);

        if (portalEntrance != null)
            portalEntrance.SetActive(false);

        if (drinkMessagePanel != null)
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
            if (potion != null)
                potion.SetActive(false);

            if (carryPotion != null)
                carryPotion.SetActive(true);
        }

        bool drinkPressed = (Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
            || (danceMat != null && danceMat.PlayerSelectedThisFrame());

        if (drinkPressed && !dropOnce && pickedUpPotion)
        {
            DrinkPotion();
        }
    }

    private void DrinkPotion()
    {
        dropOnce = true;
        pickedUpPotion = false;

        if (beaconManager != null)
            beaconManager.OnPotionDrunk();

        if (breadcrumbsPath != null)
            breadcrumbsPath.DisableBreadcrumbs();

        ShowDrinkMessage();

        Vector3 portalPosition = transform.position + transform.rotation * new Vector3(2.0f, 0.0f, 1.0f);

        if (carryPotion != null)
            carryPotion.SetActive(false);

        if (portalEntrance != null)
        {
            portalEntrance.SetActive(true);
            portalEntrance.transform.position = portalPosition;
        }
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