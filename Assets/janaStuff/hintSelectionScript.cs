using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class hintSelectionScript : MonoBehaviour
{
    public Button mapButton; // =0
    public Button beaconButton; // =1
    public Button pathButton; // =2
    public GameObject buttonsExplain; //=3
    public Button explainButton;

    public int selection = 0;
    int keyCounter = 0;
    public bool showHints;
    public bool hintsWereHidden;
    public GameObject shownHints;
    [SerializeField] private MapToggle mapToggle;
    [SerializeField] private BreadcrumbsPath breadcrumbsPath;
    [SerializeField] private BeaconManager beaconManager;

    //code for changing button colors from https://discussions.unity.com/t/how-to-change-button-color/817384/4
    ColorBlock mapColors;
    ColorBlock beaconColors;
    ColorBlock pathColors;
    ColorBlock explainColors;
    Color beige;
    Color green;
    private DancematTranslater danceMat;
    private bool hintIsActive = false;
    private int activeHintIndex = -1;
    private void Start()
    {
        mapColors = mapButton.colors;
        beaconColors = beaconButton.colors;
        pathColors = pathButton.colors;
        explainColors = explainButton.colors;
        showHints = false;
        shownHints.SetActive(false);
        hintsWereHidden = true;
        beige = new Color(0.9411765f, 0.6784314f, 0.454902f);
        green = new Color(0.2471555f, 0.7660377f, 0.3631759f);
        danceMat = FindObjectOfType<DancematTranslater>();
        if (mapToggle == null)
            mapToggle = FindObjectOfType<MapToggle>();
        if (breadcrumbsPath == null)
            breadcrumbsPath = FindObjectOfType<BreadcrumbsPath>();
        if (beaconManager == null)
            beaconManager = FindObjectOfType<BeaconManager>();
    }

    void Update()
    {
        bool hintPressed = Keyboard.current.hKey.wasPressedThisFrame
            || (danceMat != null && danceMat.MapToggledThisFrame());
        if (hintPressed)
        {
            if (hintIsActive)
            {
                CloseActiveHint();
                return;
            }
            //Debug.Log("pressed the key!");
            if (!showHints)
            {
                showHints = true;
                shownHints.SetActive(true);
                hintsWereHidden = false;
                selection = 0;
                mapColors.normalColor = green;
                beaconColors.normalColor = beige;
                pathColors.normalColor = beige;
                explainColors.normalColor = beige;
                keyCounter++;
                StartCoroutine(selectionTimer(keyCounter));
            }
            else
            { 
                selection++;
                keyCounter++;
                selection = selection % 4;
                mapColors.normalColor = beige;
                beaconColors.normalColor = beige;
                pathColors.normalColor = beige;
                explainColors.normalColor = beige;

                switch (selection)
                {
                    case 0:
                        mapColors.normalColor = green;
                        break;
                    case 1:
                        beaconColors.normalColor = green;
                        break;
                    case 2:
                        pathColors.normalColor = green;
                        break;
                    case 3:
                        explainColors.normalColor = green;
                        break;
                    default:
                        Debug.Log("sth went wrong in the hint selection process #1");
                        break;
                }
                

                StartCoroutine(selectionTimer(keyCounter));
            }
            mapButton.colors = mapColors;
            pathButton.colors = pathColors;
            beaconButton.colors = beaconColors;
            explainButton.colors = explainColors;
        }
    }

    IEnumerator selectionTimer(int keyCounterStart)
    { 
        yield return new WaitForSeconds(2);
        if(keyCounter == keyCounterStart) //either I'm a goddamn genius or this is the wackiest code ever produced by (wo)man
        {
            giveHint(selection);
            keyCounter = 0;
        }
    }

    void giveHint(int whichHint)
    {
        switch (whichHint)
        {
            case 0:
                if (mapToggle != null) mapToggle.SetMapOpen(true);
                hintIsActive = true;
                activeHintIndex = 0;
                break;
            case 1:
                if (beaconManager != null) beaconManager.ShowBeacon();
                break;
            case 2:
                if (breadcrumbsPath != null) breadcrumbsPath.CastBreadcrumbSpell();
                break;
            case 3:
                buttonsExplain.SetActive(true);
                break;
            default:
                Debug.Log("sth went wrong in the hint selection process #2");
                break;
        }

        hintsWereHidden = true;
        showHints = false;
        shownHints.SetActive(false);
    }

    void CloseActiveHint()
    {
        if (activeHintIndex == 0 && mapToggle != null) mapToggle.SetMapOpen(false);
        hintIsActive = false;
        activeHintIndex = -1;
    }
}
