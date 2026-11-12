
using UnityEngine;

public class SceneMusic : MonoBehaviour
{
    public enum MusicType
    {
        Menu,
        Gameplay,
        City
    }

    [Header("Music Settings")]
    public MusicType musicType;

    private void Start()
    {
        if (AudioManager.Instance == null)
            return;

        switch (musicType)
        {
            case MusicType.Menu:
                AudioManager.Instance.PlayMenuMusic();
                break;

            case MusicType.Gameplay:
                AudioManager.Instance.PlayGameplayMusic();
                break;

            case MusicType.City:
                AudioManager.Instance.PlayCityMusic();
                break;
        }
    }
}
