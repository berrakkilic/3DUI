using UnityEngine;
using UnityEngine.InputSystem;

public class MonsterFight : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private GameObject fluteDrop;
    [SerializeField] private float interactionDistance = 3f;

    private bool defeated = false;
    private DancematTranslater danceMat;

    private void Start()
    {
        if (fluteDrop != null)
            fluteDrop.SetActive(false);
        danceMat = FindObjectOfType<DancematTranslater>();
    }

    private void Update()
    {
        if (defeated || player == null)
            return;

        float distance = Vector3.Distance(transform.position, player.position);

        bool fightPressed = (Keyboard.current != null && Keyboard.current.fKey.wasPressedThisFrame)
            || (danceMat != null && danceMat.PlayerSelectedThisFrame());

        if (distance <= interactionDistance && fightPressed)
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