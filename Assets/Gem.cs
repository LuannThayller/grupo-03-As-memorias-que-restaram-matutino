
using System;
using UnityEngine;

public class Gem : MonoBehaviour, Item
{
    public static event Action<int> OnGemCollect;

    public int worth = 5;

    public void Collect()
    {
        OnGemCollect?.Invoke(worth);

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayGemCollect();
        }

        Destroy(gameObject);
    }
}
