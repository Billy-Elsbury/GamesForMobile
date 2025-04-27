using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class BackgroundMusicController : MonoBehaviour
{
    public static BackgroundMusicController Instance { get; private set; }

    [Header("Audio Settings")]
    [SerializeField] private AudioClip backgroundMusicClip;
    [SerializeField] private bool playOnStart = true;

    private AudioSource musicSource;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            musicSource = GetComponent<AudioSource>();
            ConfigureAudioSource();
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        if (Instance == this && playOnStart && musicSource != null && !musicSource.isPlaying && musicSource.clip != null)
        {
            musicSource.Play();
        }
    }

    private void ConfigureAudioSource()
    {
        if (musicSource == null) return;

        if (backgroundMusicClip != null)
        {
            musicSource.clip = backgroundMusicClip;
        }
    }

    public void PlayMusic()
    {
        if (musicSource != null && !musicSource.isPlaying && musicSource.clip != null)
        {
            musicSource.Play();
        }
    }

    public void PauseMusic()
    {
        if (musicSource != null && musicSource.isPlaying)
        {
            musicSource.Pause();
        }
    }

    public void ToggleMusic()
    {
        if (musicSource == null) return;

        if (musicSource.isPlaying)
        {
            musicSource.Pause();
        }
        else
        {
            if (musicSource.clip != null)
            {
                musicSource.Play();
            }
        }
    }
}