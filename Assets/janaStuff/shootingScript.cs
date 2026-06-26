using System;
using UnityEngine;
using static Cinemachine.CinemachineTargetGroup;

using UnityEngine.InputSystem;

public class shootingScript : MonoBehaviour
{
    //partially using brackeys shooting with raycasts tutorial
    public GameObject spellToShoot;
    public float damage = 20f;
    public float range = 100f;

    public Camera fpsCam;
    public CharacterController controller;
    private InputManager inputManager;
    private DancematTranslater danceMat;
    public bool isOutsideVillage;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        inputManager = InputManager.Instance;
        danceMat = FindObjectOfType<DancematTranslater>();
        isOutsideVillage = false;
    }

    // Update is called once per frame
    void Update()
    {
        bool spellPressed = (Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
            || (danceMat != null && danceMat.SpellCastThisFrame());

        if (spellPressed && isOutsideVillage)
        {
            shoot();
        }
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
