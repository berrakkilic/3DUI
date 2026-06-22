using UnityEngine;

public class potionScriptPotion : MonoBehaviour
{
    public potionScript potionScript;

    // Update is called once per frame
    private void Start()
    {

    }
    public void OnTriggerEnter(Collider other)
    {
        potionScript.pickedUpPotion = true;
    }
}
