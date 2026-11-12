
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    // Permite acessar o AudioManager
    // de qualquer outro script.
    public static AudioManager Instance { get; private set; }

    [Header("Audio Sources")]

    [SerializeField]
    private AudioSource musicSource;

    [SerializeField]
    private AudioSource sfxSource;

    [Header("Music")]

    [SerializeField]
    private AudioClip menuMusic;

    [SerializeField]
    private AudioClip gameplayMusic;

    [SerializeField]
    private AudioClip cityMusic;

    [Header("Sound Effects")]

    [SerializeField]
    private AudioClip shootSound;

    [SerializeField]
    private AudioClip playerDamageSound;

    [SerializeField]
    private AudioClip playerDeathSound;

    [SerializeField]
    private AudioClip enemyDeathSound;

    [SerializeField]
    private AudioClip gemCollectSound;

    [SerializeField]
    private AudioClip buttonClickSound;

    private void Awake()
    {
        // Impede a criação de múltiplos managers.
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        // Mantém o AudioManager entre cenas.
        DontDestroyOnLoad(gameObject);

        // Configura as fontes de áudio.
        musicSource.loop = true;
        musicSource.playOnAwake = false;

        sfxSource.loop = false;
        sfxSource.playOnAwake = false;
    }

    // Reproduz uma música sem reiniciá-la
    // caso já esteja tocando.
    public void PlayMusic(AudioClip music)
    {
        if (music == null)
            return;

        if (musicSource.clip == music &&
            musicSource.isPlaying)
            return;

        musicSource.Stop();
        musicSource.clip = music;
        musicSource.Play();
    }

    public void PlayMenuMusic()
    {
        PlayMusic(menuMusic);
    }

    public void PlayGameplayMusic()
    {
        PlayMusic(gameplayMusic);
    }

    public void PlayCityMusic()
    {
        PlayMusic(cityMusic);
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }

    // Reproduz efeitos sem interromper outros sons.
    public void PlaySFX(AudioClip clip)
    {
        if (clip == null)
            return;

        sfxSource.PlayOneShot(clip);
    }

    // Métodos específicos para cada evento.
    public void PlayShoot()
    {
        PlaySFX(shootSound);
    }

    public void PlayPlayerDamage()
    {
        PlaySFX(playerDamageSound);
    }

    public void PlayPlayerDeath()
    {
        PlaySFX(playerDeathSound);
    }

    public void PlayEnemyDeath()
    {
        PlaySFX(enemyDeathSound);
    }

    public void PlayGemCollect()
    {
        PlaySFX(gemCollectSound);
    }

    public void PlayButtonClick()
    {
        PlaySFX(buttonClickSound);
    }

    // Controle de volume entre 0 e 1.
    public void SetMusicVolume(float volume)
    {
        musicSource.volume = Mathf.Clamp01(volume);
    }

    public void SetSFXVolume(float volume)
    {
        sfxSource.volume = Mathf.Clamp01(volume);
    }
}
