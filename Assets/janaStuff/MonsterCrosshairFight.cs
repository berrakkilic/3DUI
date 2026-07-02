using UnityEngine;
using UnityEngine.InputSystem;

public class MonsterCrosshairFight : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform player;
    [SerializeField] private Camera fpsCam;
    [SerializeField] private GameObject crosshair;

    [Header("Settings")]
    [SerializeField] private float activationDistance = 8f;
    [SerializeField] private float range = 100f;
    [SerializeField] private float damage = 20f;
    [SerializeField] private GameObject crossbow;

    private targetScript monsterTarget;
    private DancematTranslater danceMat;

    void Start()
    {
        monsterTarget = GetComponent<targetScript>();
        danceMat = FindObjectOfType<DancematTranslater>();

        if (crosshair != null)
            crosshair.SetActive(false);
    }

    void Update()
{
    if (player == null || fpsCam == null || monsterTarget == null)
        return;

    if (monsterTarget.health <= 0)
    {
        if (crosshair != null)
            crosshair.SetActive(false);

        if (crossbow != null)
            crossbow.SetActive(false);

        enabled = false;
        return;
    }

    float distance = Vector3.Distance(player.position, transform.position);
    bool fightActive = distance <= activationDistance;

    if (crosshair != null)
        crosshair.SetActive(fightActive);

    if (crossbow != null)
        crossbow.SetActive(fightActive);

    if (!fightActive)
        return;

    bool shootPressed = Keyboard.current.eKey.wasPressedThisFrame
        || (danceMat != null && danceMat.PlayerSelectedThisFrame());
    if (shootPressed)
    {
        Shoot();
    }
}

    void Shoot()
    {
        Ray ray = fpsCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));

        if (Physics.Raycast(ray, out RaycastHit hit, range))
        {
            Debug.Log("Hit: " + hit.transform.name);

            targetScript target = hit.transform.GetComponentInParent<targetScript>();

            if (target != null)
            {
                target.takeDamage(damage);
                Debug.Log("Monster took damage");
                if (target.health <= 0 && crosshair != null)
                {
                    crosshair.SetActive(false);
                }
            }
            else
            {
                Debug.Log("Hit something, but not monster.");
            }
        }
        else
        {
            Debug.Log("Missed.");
        }
    }
}