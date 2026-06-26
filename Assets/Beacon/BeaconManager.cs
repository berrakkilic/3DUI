using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class BeaconManager : MonoBehaviour
{
    [SerializeField] private Transform wizardTarget;
    [SerializeField] private Transform monsterTarget;
    [SerializeField] private float beaconHeight = 3f;
    [SerializeField] private float beaconDuration = 5f;

    private shootingScript shootingScript;
    private GameObject beacon;
    private Coroutine beaconRoutine;

    private void Start()
    {
        shootingScript = FindObjectOfType<shootingScript>();
        CreateBeacon();
    }

    private void Update()
    {
        if (Keyboard.current != null && Keyboard.current.kKey.wasPressedThisFrame)
            ShowBeacon();
    }

    private void CreateBeacon()
    {
        beacon = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        beacon.transform.localScale = new Vector3(0.5f, 1.5f, 0.5f);
        Destroy(beacon.GetComponent<Collider>());

        var mat = new Material(Shader.Find("Universal Render Pipeline/Unlit"));
        mat.SetColor("_BaseColor", new Color(1f, 0.55f, 0f));
        beacon.GetComponent<Renderer>().material = mat;
        beacon.SetActive(false);
    }

    public void ShowBeacon()
    {
        Transform target = GetCurrentTarget();
        if (target == null) return;

        if (beaconRoutine != null) StopCoroutine(beaconRoutine);
        beaconRoutine = StartCoroutine(ShowBeaconRoutine(target));
    }

    private Transform GetCurrentTarget()
    {
        bool outsideVillage = shootingScript != null && shootingScript.isOutsideVillage;
        return outsideVillage ? monsterTarget : wizardTarget;
    }

    private IEnumerator ShowBeaconRoutine(Transform target)
    {
        beacon.SetActive(true);
        float timer = 0f;

        while (timer < beaconDuration)
        {
            if (target != null)
                beacon.transform.position = target.position + Vector3.up * beaconHeight;
            timer += Time.deltaTime;
            yield return null;
        }

        beacon.SetActive(false);
        beaconRoutine = null;
    }
}
