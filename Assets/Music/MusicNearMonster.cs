using UnityEngine;

public class MusicNearMonster : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Transform monster;
    [SerializeField] private AudioSource musicSource;

    [SerializeField] private float stopDistance = 8f;
    [SerializeField] private float fadeSpeed = 2f;
    [SerializeField] private float normalVolume = 0.4f;

    private void Start()
    {
        if (musicSource == null)
            musicSource = GetComponent<AudioSource>();

        if (musicSource != null)
        {
            musicSource.volume = normalVolume;

            if (!musicSource.isPlaying)
                musicSource.Play();
        }
    }

    private void Update()
    {
        if (player == null || monster == null || musicSource == null)
            return;

        float distance = Vector3.Distance(player.position, monster.position);

        float targetVolume = distance <= stopDistance ? 0f : normalVolume;

        musicSource.volume = Mathf.MoveTowards(
            musicSource.volume,
            targetVolume,
            fadeSpeed * Time.deltaTime
        );
    }
}