using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : Player
{
    public float maxMadness;
    public float currentMadness;
    

    private bool canClap;
    private bool isAttacking;
    private bool isAlreadyAngry = false;
    private bool isAngry = false;
    private float soundDamage;

    #region INPUT_SYSTEM
    public override void Interact(InputAction.CallbackContext ctx)
    {
        if (deviceToSwitch != null)
        {
            deviceToSwitch.SwitchOff();
        }
    }

    public void Attack(InputAction.CallbackContext ctx)
    {
        isAttacking = false;
        if (ctx.phase == InputActionPhase.Started)
        {
            isAttacking = true;
            canClap = true;
        }
    }
    #endregion

    #region TRIGGERS
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<IInteractable>(out IInteractable hit))
        {
            deviceToSwitch = hit;
        }
    }

    #endregion

    public void GetBitten(float biteStrenght)
    {
        currentMadness += biteStrenght;
    }

    private bool GettingAngry()
    {
        return currentMadness >= maxMadness / 2 && !isAlreadyAngry;
    }

    private void GetDamageFromSound()
    {
        float noise = AudioManager.Instance.PollNoiseAmount();
        soundDamage = AudioManager.Instance.ConvertNoiseToDamage();
        if (soundDamage > 0)
        {
            if (currentMadness <= maxMadness)
            {
                currentMadness += soundDamage * Time.deltaTime;
            }

        }
        else
        {
            currentMadness -= Time.deltaTime;
            if (currentMadness < 0)
            {
                currentMadness = 0;
            }
        }
        MainMenuManager.Instance.UpdatePlayerMadnessSlider(currentMadness);
    }

    private IEnumerator BlockPlayerMovement(float time)
    {
        canMove = false;
        inputMovement = new Vector2(0f, 0f);
        yield return new WaitForSeconds(time);
        canMove = true;
        yield break;
    }

    #region UNITY_METHODS
    public override void Awake()
    {
        base.Awake();
        GameManager.Instance.player = this;
    }
    private void Start()
    {
        MainMenuManager.Instance.SetPlayerMadness();
    }

    public override void Update()
    {
        base.Update();
        if (currentMadness > maxMadness)
        {
            currentMadness = maxMadness;
            animator.SetTrigger("Is Dead");
            print(GameManager.Instance.mosquito.roundWon);
            MainMenuManager.Instance.AssignRound("Mosquito", GameManager.Instance.mosquitoRoundWon);
        }
        if (isAttacking && canClap)
        {
            canClap = false;
            animator.SetTrigger("Clap");
            StartCoroutine(BlockPlayerMovement(1.35f));
        }
        if (GettingAngry())
        {
            print("GETTING ANGRY");
            animator.SetBool("GetCrazy", true);
            StartCoroutine(BlockPlayerMovement(3.533f));
            isAngry = true;
            isAlreadyAngry = true;
        }
        if (!isAngry)
        {
            if (inputMovement.x == 0f && inputMovement.y == 0)
            {
                animator.SetBool("IsRunning", false);
            }
            else
            {
                animator.SetBool("IsRunning", true);
            }
        }
        else
        {
            if (inputMovement.x == 0f && inputMovement.y == 0)
            {
                animator.SetBool("IsAngryRunning", false);
            }
            else
            {
                animator.SetBool("IsAngryRunning", true);
            }
        }
        GetDamageFromSound();
    }
    #endregion
}
