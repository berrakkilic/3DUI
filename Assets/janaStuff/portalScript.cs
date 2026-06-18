using UnityEngine;

public class portalScript : MonoBehaviour
{
    public GameObject player;
    public GameObject portalExit;

    public void OnTriggerStay(Collider other)
    {
        player.transform.position = portalExit.transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("made it to method");
        Debug.Log(portalExit.transform.position);
        CharacterController cc = player.GetComponent<CharacterController>();
        cc.enabled = false;
        player.transform.position = portalExit.transform.position;
        //player.transform.position.Set(portalExit.transform.position.x, portalExit.transform.position.y, portalExit.transform.position.z);
        cc.enabled = true;
    }
}
