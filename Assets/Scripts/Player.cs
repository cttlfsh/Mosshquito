using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

//[RequireComponent(typeof(CharacterController))]
public abstract class Player : MonoBehaviour
{
    [SerializeField] private float speed;
    public int roundWon = 0;

    protected IInteractable deviceToSwitch;
    protected Vector2 inputMovement;
    protected CharacterController cc;
    protected Animator animator;
    protected bool canMove = true;

    private float turnSmoothTime = 0.05f;

    #region INPUT_SYSTEM
    public void Move(InputAction.CallbackContext ctx)
    {
        if (canMove)
        {
            inputMovement = ctx.ReadValue<Vector2>();
        }

    }

    public virtual void Interact(InputAction.CallbackContext ctx) { }
    #endregion

    #region TRIGGERS
    public virtual void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent<IInteractable>(out IInteractable hit))
        {
            deviceToSwitch = null;
        }
    }
    #endregion

    #region UNITY_METHODS
    public virtual void Awake()
    {
        cc = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }
    public virtual void Update()
    {
        float move = Mathf.Abs(inputMovement.y) + Mathf.Abs(inputMovement.x);
        Vector3 finalMovement = new Vector3(inputMovement.x, 0f, inputMovement.y).normalized;
        float turnSmoothVelocity = 0f;

        if (finalMovement.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(finalMovement.x, finalMovement.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
            cc.Move(finalMovement * speed * Time.deltaTime);

        }
    }
    #endregion
}
