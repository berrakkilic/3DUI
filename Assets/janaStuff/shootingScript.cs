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
    //public LineRenderer lineRenderer;

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

        if (spellPressed)
        {
            shoot();
        }
    }

    // void HideLine()
    // {
    //     lineRenderer.enabled = false;
    // }

    void shoot()
    {
        RaycastHit hit;
        Debug.DrawRay(fpsCam.transform.position, fpsCam.transform.forward * range, Color.red, 2f);
        // lineRenderer.enabled = true;
        // lineRenderer.SetPosition(0, fpsCam.transform.position);
        // lineRenderer.SetPosition(1, fpsCam.transform.position + fpsCam.transform.forward * range);

        // Invoke(nameof(HideLine), 0.1f);
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
        {
            Debug.Log("you hit: " + hit.transform.name);
            targetScript target = hit.transform.GetComponentInParent<targetScript>();
            if (target != null)
            {
                target.takeDamage(damage);
                Debug.Log("Monster took damage");
            } else {
                Debug.Log("Hit object has no targetScript");
            }
        }
        else {
            Debug.Log("Missed");
        }
    }
}
