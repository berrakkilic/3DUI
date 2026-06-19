using UnityEngine;
using UnityEngine.InputSystem;

public class MonsterFight : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private GameObject fluteDrop;
    [SerializeField] private float interactionDistance = 3f;

    private bool defeated = false;

    private void Start()
    {
        if (fluteDrop != null)
            fluteDrop.SetActive(false);
    }

    private void Update()
    {
        if (defeated || player == null)
            return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= interactionDistance && Keyboard.current.fKey.wasPressedThisFrame)
        {
            DefeatMonster();
        }
    }

    private void DefeatMonster()
    {
        defeated = true;

        if (fluteDrop != null)
            fluteDrop.SetActive(true);

        gameObject.SetActive(false);
    }
}