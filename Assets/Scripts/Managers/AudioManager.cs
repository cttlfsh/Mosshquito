using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public List<InteractableForniture> fornitures = new List<InteractableForniture>();
    public float damageMultiplier;

    private float maxNoise;
    private float minNoise = 2;
    private float currentNoise;
    private float damage;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            return;
        }
    }

    public float GetMaxNoise()
    {
        return maxNoise;
    }

    public void SetMaxNoise()
    {
        foreach (InteractableForniture item in fornitures)
        {
            maxNoise += item.timeToSwitchOn;
        }
    }

    public float PollNoiseAmount()
    {
        currentNoise = 0;
        foreach (InteractableForniture item in fornitures)
        {
            if (item.source.isPlaying)
            {
                currentNoise += item.timeToSwitchOn;
            }
        }
        return currentNoise;
    }

    public float ConvertNoiseToDamage()
    {
        damage = currentNoise * damageMultiplier;
        return damage;
    }
    private void Start()
    {
        SetMaxNoise();
    }
}
