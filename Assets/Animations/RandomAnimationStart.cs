using UnityEngine;

public class RandomAnimationStart : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private string stateName = "Talking";

    private void Start()
    {
        if (animator == null)
            animator = GetComponent<Animator>();

        if (animator == null)
            return;

        animator.Play(stateName, 0, Random.value);
    }
}