using UnityEngine;

public class FightWeaponVisibility : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Transform monster;
    [SerializeField] private GameObject crossbow;
    [SerializeField] private GameObject crosshair;
    [SerializeField] private float activationDistance = 8f;

    void Start()
    {
        if (crossbow != null) crossbow.SetActive(false);
        if (crosshair != null) crosshair.SetActive(false);
    }

    void Update()
    {
        if (player == null || monster == null) return;

        bool fightActive = Vector3.Distance(player.position, monster.position) <= activationDistance;

        if (crossbow != null) crossbow.SetActive(fightActive);
        if (crosshair != null) crosshair.SetActive(fightActive);
    }
}