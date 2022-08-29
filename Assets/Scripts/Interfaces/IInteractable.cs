using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable 
{
    public void StartSwitchingOn();
    public void SwitchOff();

    public void InterruptSwitchOn();
}
