using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableCube : MonoBehaviour, IInteractable
{
    public Material onMaterial;
    public Material offMaterial;
    public float timeToSwitchOn;

    private MeshRenderer mr;
    private float switchOnPercentage;
    private bool isSwitchingOn;
    
    public Coroutine switchingOnCoroutine;
    #region INTERFACE_IMPLEMENTATION
    
    public void InterruptSwitchOn()
    {
        Debug.Log($"SWITCH ON INTERRUPT: {switchOnPercentage}");
        isSwitchingOn = false;
        switchOnPercentage = 0;
    }

    public void SwitchOff()
    {
        mr.material = offMaterial;
    }

    public void StartSwitchingOn()
    {
        isSwitchingOn = true;
    }
    #endregion

    #region UNITY_METHODS
    private void Awake()
    {
        mr = GetComponent<MeshRenderer>();
    }

    private void Update()
    {
        if (switchOnPercentage < timeToSwitchOn && isSwitchingOn)
        {
            switchOnPercentage += Time.deltaTime;
            switchOnPercentage = Mathf.Clamp(switchOnPercentage, 0, timeToSwitchOn);
            Debug.Log($"SWITCH ON LOADING: {switchOnPercentage}");
        }
        if (switchOnPercentage == timeToSwitchOn)
        {
            mr.material = onMaterial;
            switchOnPercentage = 0;
            isSwitchingOn = false;
        }
    }

    #endregion
}
