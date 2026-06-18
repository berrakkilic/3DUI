using UnityEngine;
using System.Collections;

public class hintSelectionScript : MonoBehaviour
{
    public GameObject breadcrumbsHintSelect; // =0
    public GameObject beaconsHintSelect; // =1
    public GameObject mapMarkerHintSelect; // =2

    public int selection = 0;
    int keyCounter = 0;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)) //need to replace here with appropriate InputManager Sys thingy
        {
            selection++;
            selection = selection % 3;
            StartCoroutine(selectionTimer(keyCounter));
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
                //enable breadcrumbs here
                break;
            case 1:
                //enable beacons here
                break;
            case 2:
                //enable marker on map here
                break;
            default:
                Debug.Log("sth went wrong in the hint selection process");
                break;
        }
    }
}
