using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("SFX")]
    public AudioClip enemyHitSound;
    public AudioClip shootSound;
    public AudioClip hitSound;
    public AudioClip coinSound;

    [Header("Boss SFX")]
    public AudioClip bossHitSound;
    public AudioClip bossDeathSound;

    [Header("Music")]
    public AudioClip winMusic;

    private AudioSource audioSource;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        audioSource = GetComponent<AudioSource>();
        DontDestroyOnLoad(gameObject);
    }

    public void PlaySFX(AudioClip clip)
    {
        if (clip == null) return;
        audioSource.PlayOneShot(clip);
    }

    public void PlayMusic(AudioClip musicClip, bool loop = true)
    {
        if (musicClip == null) return;
        audioSource.clip = musicClip;
        audioSource.loop = loop;
        audioSource.Play();
    }

    public void StopMusic()
    {
        audioSource.Stop();
    }
}
