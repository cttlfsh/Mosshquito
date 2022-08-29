using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class InteractableForniture : MonoBehaviour, IInteractable
{
    public float timeToSwitchOn;

    [Header("Audio")]
    public AudioSource source;
    public AudioClip loadingClip;
    public AudioClip onAudioClip;
    public AudioClip offAudioClip;
    

    [Header("VFX")]
    public VisualEffect vfx;
    public ParticleSystem soundVfx;

    [Header("Lights")]
    public Light interactLight;

    [Header("Animator")]
    public Animator animator;

    private float switchOnPercentage;
    private bool isSwitchingOn;
    private float lightIntensity;

    #region INTERFACE_IMPLEMENTATION
    public void InterruptSwitchOn()
    {
        isSwitchingOn = false;
        switchOnPercentage = 0;
    }

    public void SwitchOn()
    {
        if (animator != null)
        {
            animator.SetBool("Active", true);
        }

        if (onAudioClip != null)
        {
            source.clip = onAudioClip;
            source.Play();
            source.loop = true;
        }
        if (vfx != null)
        {
            vfx.Play();
        }
        if (interactLight != null)
        {
            interactLight.intensity = lightIntensity;
        }
        // TRIGGER EVENTUALE ANIMAZIONE
        soundVfx.Play();
        switchOnPercentage = 0;
        isSwitchingOn = false;
    }
    
    public void SwitchOff()
    {
        if (animator != null)
        {
            animator.SetBool("Active", false);
        }
        if (offAudioClip != null)
        {
            source.clip = offAudioClip;
            source.Play();
            source.loop = false;
        }
        source.Stop();
        if (vfx != null)
        {
            vfx.Stop();
        }
        if (interactLight != null)
        {
            interactLight.intensity = 0;
        }
        // EVENTUALE ANIMAZIONE AL CONTRARIO
        soundVfx.Stop();
        switchOnPercentage = 0;
        isSwitchingOn = false;
    }


    public void StartSwitchingOn()
    {
        isSwitchingOn = true;
    }

    #endregion

    #region UNITY_METHODS
    private void Awake()
    {
        if (onAudioClip != null)
        {
            source.Stop();
        }
        if (vfx != null)
        {
            vfx.Stop();
        }
        if (interactLight != null)
        {
            lightIntensity = interactLight.intensity;
            interactLight.intensity = 0;
        }
        soundVfx.Stop();

        switchOnPercentage = 0;
        isSwitchingOn = false;
    }

    private void Start()
    {
        AudioManager.Instance.fornitures.Add(this);
    }

    private void Update()
    {
        if (switchOnPercentage < timeToSwitchOn && isSwitchingOn)
        {
            switchOnPercentage += Time.deltaTime;
            switchOnPercentage = Mathf.Clamp(switchOnPercentage, 0, timeToSwitchOn);
            //Debug.Log($"SWITCH ON LOADING: {switchOnPercentage}");
            if (loadingClip != null)
            {
                source.clip = loadingClip;
                source.Play();
                source.loop = true;
            }
        }
        if (switchOnPercentage == timeToSwitchOn)
        {
            SwitchOn();
        }
    }

    #endregion
}
