using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class BeaconManager : MonoBehaviour
{
    [SerializeField] private Transform villagerTarget;
    [SerializeField] private Transform wizardTarget;
    [SerializeField] private Transform potionTarget;
    [SerializeField] private Transform monsterTarget;
    [SerializeField] private float beaconDuration = 5f;

    private enum BeaconQuestStep { ToVillager, ToWizard, ToPotion, ToMonster }
    private BeaconQuestStep currentQuestStep = BeaconQuestStep.ToVillager;

    private GameObject beacon;
    private Coroutine beaconRoutine;

    private void Start()
    {
        CreateBeacon();
    }

    public void OnNPCDialogueCompleted(NPCDialogue npc)
    {
        if (npc == null) return;

        if (npc.breadcrumbStoryRole == NPCDialogue.BreadcrumbStoryRole.Villager
            && currentQuestStep == BeaconQuestStep.ToVillager)
        {
            currentQuestStep = BeaconQuestStep.ToWizard;
        }
        else if (npc.breadcrumbStoryRole == NPCDialogue.BreadcrumbStoryRole.Wizard
            && currentQuestStep == BeaconQuestStep.ToWizard)
        {
            currentQuestStep = BeaconQuestStep.ToPotion;
        }
    }

    private void Update()
    {
        if (Keyboard.current != null && Keyboard.current.kKey.wasPressedThisFrame)
            ShowBeacon();
    }

    private void CreateBeacon()
    {
        beacon = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        beacon.transform.localScale = new Vector3(0.5f, 500f, 0.5f);
        Destroy(beacon.GetComponent<Collider>());

        var mat = new Material(Shader.Find("Universal Render Pipeline/Unlit"));
        mat.SetColor("_BaseColor", Color.yellow);
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

    public void OnPotionDrunk()
    {
        if (currentQuestStep == BeaconQuestStep.ToPotion)
            currentQuestStep = BeaconQuestStep.ToMonster;
    }

    private Transform GetCurrentTarget()
    {
        return currentQuestStep switch
        {
            BeaconQuestStep.ToVillager => villagerTarget,
            BeaconQuestStep.ToWizard => wizardTarget,
            BeaconQuestStep.ToPotion => potionTarget,
            _ => monsterTarget,
        };
    }

    private IEnumerator ShowBeaconRoutine(Transform target)
    {
        beacon.SetActive(true);
        float timer = 0f;

        while (timer < beaconDuration)
        {
            if (target != null)
                beacon.transform.position = target.position + Vector3.up * 500f;
            timer += Time.deltaTime;
            yield return null;
        }

        beacon.SetActive(false);
        beaconRoutine = null;
    }
}
