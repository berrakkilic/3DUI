using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class hintSelectionScript : MonoBehaviour
{
    public Button mapButton; // =0
    public Button beaconButton; // =1
    public Button pathButton; // =2

    public int selection = 0;
    int keyCounter = 0;
    public bool showHints;
    public bool hintsWereHidden;
    public GameObject shownHints;

    //code for changing button colors from https://discussions.unity.com/t/how-to-change-button-color/817384/4 
    ColorBlock mapColors;
    ColorBlock beaconColors;
    ColorBlock pathColors;
    Color beige;
    Color green;
    private void Start()
    {
        mapColors = mapButton.colors;
        beaconColors = beaconButton.colors;
        pathColors = pathButton.colors;
        showHints = false;
        shownHints.SetActive(false);
        hintsWereHidden = true;
        beige = new Color(0.9411765f, 0.6784314f, 0.454902f);
        green = new Color(0.2471555f, 0.7660377f, 0.3631759f);
    }

    void Update()
    {
        if (Keyboard.current.hKey.wasPressedThisFrame)
        {
            Debug.Log("pressed the key!");
            if (!showHints)
            {
                showHints = true;
                shownHints.SetActive(true);
                hintsWereHidden = false;
                selection = 0;
                mapColors.normalColor = green;
                beaconColors.normalColor = beige;
                pathColors.normalColor = beige;
            }
            else
            { 
                selection++;
                keyCounter++;
                selection = selection % 3;
                mapColors.normalColor = beige;
                beaconColors.normalColor = beige;
                pathColors.normalColor = beige;

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
                    default:
                        Debug.Log("sth went wrong in the hint selection process #1");
                        break;
                }
                mapButton.colors = mapColors;
                pathButton.colors = pathColors;
                beaconButton.colors = beaconColors;

                StartCoroutine(selectionTimer(keyCounter));
            }
        }
    }

    IEnumerator selectionTimer(int keyCounterStart)
    { 
        yield return new WaitForSeconds(5);
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
                //enable map here
                Debug.Log("link to show map!");
                break;
            case 1:
                //enable beacons here
                Debug.Log("Link to show a beacon!");
                break;
            case 2:
                //enable button on map here
                Debug.Log("Link to start breadcrumbs!");
                break;
            default:
                Debug.Log("sth went wrong in the hint selection process #2");
                break;
        }

        hintsWereHidden = true;
        showHints = false;
        shownHints.SetActive(false);
    }
}
