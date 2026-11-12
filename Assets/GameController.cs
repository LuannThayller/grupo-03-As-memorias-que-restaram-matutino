
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    [Header("Progress")]
    int progressAmount;
    public Slider progressSlider;

    [Header("Player")]
    public GameObject player;

    [Header("UI")]
    public GameObject LoadCanvas;

    [Header("Levels")]
    public List<GameObject> levels;

    private int currentLevelIndex = 0;

    private void Awake()
    {
        progressAmount = 0;
    }

    private void OnEnable()
    {
        Gem.OnGemCollect += IncreaseProgressAmount;
        HoldToLoadLevel.OnHoldComplete += LoadNextLevel;
    }

    private void Start()
    {
        LoadCanvas.SetActive(false);

        if (progressSlider != null)
        {
            progressSlider.value = progressAmount;
        }
    }

    private void OnDisable()
    {
        Gem.OnGemCollect -= IncreaseProgressAmount;
        HoldToLoadLevel.OnHoldComplete -= LoadNextLevel;
    }

    private void IncreaseProgressAmount(int amount)
    {
        progressAmount += amount;

        if (progressSlider != null)
        {
            progressSlider.value = progressAmount;
        }

        if (progressAmount >= 100)
        {
            LoadCanvas.SetActive(true);
            Debug.Log("Level Complete");
        }
    }

    private void LoadNextLevel()
    {
        if (levels == null || levels.Count == 0)
        {
            Debug.LogWarning("No levels assigned!");
            return;
        }

        int nextLevelIndex =
            (currentLevelIndex == levels.Count - 1)
            ? 0
            : currentLevelIndex + 1;

        LoadCanvas.SetActive(false);

        levels[currentLevelIndex].SetActive(false);
        levels[nextLevelIndex].SetActive(true);

        currentLevelIndex = nextLevelIndex;

        player.transform.position = Vector3.zero;

        progressAmount = 0;

        if (progressSlider != null)
        {
            progressSlider.value = 0;
        }

        Debug.Log("Loaded Level: " + currentLevelIndex);
    }
}
