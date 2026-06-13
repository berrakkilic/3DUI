using UnityEngine;
using static Cinemachine.CinemachineTargetGroup;

public class shootingScript : MonoBehaviour
{
    //partially using brackeys shooting with raycasts tutorial
    public GameObject spellToShoot;
    public float damage = 20f;
    public float range = 100f;

    public Camera fpsCam;
    public CharacterController controller;
    private InputManager inputManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        inputManager = InputManager.Instance;
    }

    // Update is called once per frame
    void Update()
    {

        //if (inputManager.) {
        
        /*if(Input.GetButtonDown("1"))
        {
            shoot();
        }*/ //no clue how the new input system package works and I hate it
    }

    void shoot()
    {
        RaycastHit hit;
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
        {
            Debug.Log("you hit: " + hit.transform.name);
            targetScript target = hit.transform.GetComponent<targetScript>();
            if (target != null)
            {
                target.takeDamage(damage);
            }
        }
    }
}
