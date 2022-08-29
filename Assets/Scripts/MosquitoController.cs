using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MosquitoController : Player
{
    [SerializeField] private float biteStrenght; 
    public float life;
    public float visibilityDuration;
    public float visibilityRechargeSpeed;

    private MeshRenderer mr;
    private float visibilityAmount = 1;
    private bool isVisibilityRecharging;
    private bool isVisibilityUsed;
    private bool canBite;
    private bool isBiting;

    private Coroutine visibilityUsageCoroutine;
    private Coroutine visibilityRechargeCoroutine;
    private Coroutine switchingOnCoroutine;


    #region INPUT_SYSTEM

    public override void Interact(InputAction.CallbackContext ctx)
    {
        if (ctx.phase == InputActionPhase.Performed)
        {
            if (deviceToSwitch != null)
            {
                deviceToSwitch.StartSwitchingOn();
            }
        }
        if (ctx.phase == InputActionPhase.Canceled)
        {
            if (deviceToSwitch != null)
            {
                deviceToSwitch.InterruptSwitchOn();
            }
        }
    }

    public void Attack(InputAction.CallbackContext ctx)
    {
        if (ctx.phase == InputActionPhase.Performed)
        {
            if (canBite)
            {
                isBiting = true;
                Debug.Log("BITING");
            }
        }

        if (ctx.phase == InputActionPhase.Canceled)
        {
            isBiting = false;
        }
    }

    public void ToggleVisibility(InputAction.CallbackContext ctx)
    {
        if (ctx.phase == InputActionPhase.Performed)
        {
            visibilityUsageCoroutine = StartCoroutine(UseVisibility());
            //Debug.Log("PERFORMED");
        }
        if (ctx.phase == InputActionPhase.Canceled)
        {
            //Debug.Log("CANCELED");
            mr.enabled = false;
            visibilityRechargeCoroutine = StartCoroutine(RechargeVisibility());
        }

    }
    #endregion

    private void Bite()
    {
        life += biteStrenght * Time.deltaTime;
        //GameManager.Instance.player.GetBitten(biteStrenght * Time.deltaTime);
    }

    private void SetMeshRenderer(bool visibility)
    {
        mr.enabled = visibility;
        foreach (Transform item in transform)
        {
            item.GetComponent<MeshRenderer>().enabled = visibility;
        }
    }

    #region TRIGGERS
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<IInteractable>(out IInteractable hit))
        {
            deviceToSwitch = hit;
            StartCoroutine(HapticInteractables());
        }
        if (other.gameObject.TryGetComponent<PlayerController>(out PlayerController player))
        {
            canBite = true;
            print("TRIGGER PLAYER CONTROLLER");
        }
    }

    public override void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent<IInteractable>(out IInteractable hit))
        {
            hit.InterruptSwitchOn();
            deviceToSwitch = null;
        }
        if (other.gameObject.TryGetComponent<PlayerController>(out PlayerController player))
        {
            canBite = false;
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        StartCoroutine(HapticWalls());
    }
    #endregion

    #region COROUTINES
    private IEnumerator HapticInteractables()
    {
        Gamepad.current.SetMotorSpeeds(0.1f, 0.1f);
        Gamepad.current.ResumeHaptics();
        yield return new WaitForSeconds(0.2f);
        Gamepad.current.SetMotorSpeeds(0.0f, 0.0f);
        Gamepad.current.PauseHaptics();
        yield return new WaitForSeconds(0.05f);
        Gamepad.current.SetMotorSpeeds(0.03f, 0.03f);
        Gamepad.current.ResumeHaptics();
        yield return new WaitForSeconds(0.2f);
        Gamepad.current.SetMotorSpeeds(0.0f, 0.0f);
        Gamepad.current.PauseHaptics();
    }

    private IEnumerator HapticWalls()
    {
        Gamepad.current.SetMotorSpeeds(0.002f, 0.005f);
        Gamepad.current.ResumeHaptics();
        yield return new WaitForSeconds(0.15f);
        Gamepad.current.SetMotorSpeeds(0.0f, 0.0f);
        Gamepad.current.PauseHaptics();
    }

    private IEnumerator UseVisibility()
    {
        while (visibilityAmount > 0)
        {
            if (visibilityRechargeCoroutine != null) StopCoroutine(visibilityRechargeCoroutine);
            isVisibilityUsed = true;
            SetMeshRenderer(true);
            visibilityAmount -= Time.deltaTime * visibilityRechargeSpeed;
            visibilityAmount = Mathf.Clamp(visibilityAmount, 0, visibilityDuration);
            MainMenuManager.Instance.UpdateMosquitoVisibility(visibilityAmount);
            Debug.Log($"USAGE: {visibilityAmount}");
            yield return new WaitForSeconds(.1f);
        }
        isVisibilityUsed = false;
        SetMeshRenderer(false);
        yield return new WaitForSeconds(.1f);

    }

    private IEnumerator RechargeVisibility()
    {
        while(visibilityAmount < 1)
        {
            StopCoroutine(visibilityUsageCoroutine);
            isVisibilityRecharging = true;
            visibilityAmount += Time.deltaTime * visibilityRechargeSpeed;
            visibilityAmount = Mathf.Clamp(visibilityAmount, 0, visibilityDuration);
            MainMenuManager.Instance.UpdateMosquitoVisibility(visibilityAmount);
            yield return new WaitForSeconds(.1f);
            Debug.Log($"RECHARGE: {visibilityAmount}");
        }
        isVisibilityRecharging = false;
        yield return new WaitForSeconds(.1f);
    }

    #endregion

    #region UNITY_METHODS
    public override void Awake()
    {
        base.Awake();
        mr = GetComponent<MeshRenderer>();
        SetMeshRenderer(false);
        GameManager.Instance.mosquito = this;
    }

    private void Start()
    {
        StartCoroutine(HapticWalls());
        MainMenuManager.Instance.SetMosquitosLife();
        animator.Play(0);
    }

    public override void Update()
    {
        base.Update();
        if (life != 0)
        {
            life -= Time.deltaTime;
        }
        MainMenuManager.Instance.UpdateMosquitosLife(life);
        if (life < 0)
        {
            life = 0;
            print(GameManager.Instance.player.roundWon);
            MainMenuManager.Instance.AssignRound("Player", GameManager.Instance.playerRoundWon);
        }
        if (canBite && isBiting)
        {
            Bite();
        }
    }
    #endregion

}
