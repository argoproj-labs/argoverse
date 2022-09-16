using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using AlmenaraGames;

public class Player : MonoBehaviour
{
    //this class is a mess. needs to be refactored in a better design pattern
    public static Player Instance { get; private set; }

    [SerializeField] private Camera playerCam;

    [Header("Movement")]
    public float maxSpeed = 1720f;
    public float PlayerSpeed => maxSpeed;
    public const float FootstepThreshold = 5f;
    [SerializeField] private float turnRate = 15.0f;
    [SerializeField] [Range(0.5f, 2.5f)] private float turnSens = 1.0f;
    private float targetTurnRate;
    private float speed;
    private Vector3 resultDirection;
    private Vector2 inputDirection;
    private Rigidbody rb;
    private Vector2 inputLookDirectionDelta;
    private bool onGround = false;
    private bool onAir = false;
    private float displacementForFootstep;
    private Vector3 previousPosition;
    private int runMod = 1;

    [Header("Hand Mechanic")]
    [SerializeField] private Transform handTransform;
    public Transform screenProjectionTransform;
    private InteractableObject targetedInteractable;
    private InteractableObject currentlyHeldItem;
    [SerializeField] private Image aimImg;
    [SerializeField] private GameObject aimCanvas;
    [Tooltip("Aim sprites. 0: transparent, 1: hand, 2: eye, 3: unlock")]
    [SerializeField] private Sprite[] aimSprites;
    private Coroutine constantRefreshCoroutine;
    private Coroutine coroutineMovingItemToHand;
    const float INTERACT_DISTANCE = 3.5f;
    public float throwStrength = 7.0f;

    [Header("Other")]
    public float jumpForce = 5f;
    public bool doubleJumpAllowed = false;
    private bool doubleJumpPerformed = false;

    public bool cinematicCameraDisplay { get; private set; } = false;
    private bool _clickingBoard = false;
    private InteractableObject cinematicCameraActive;

    private float _landSoundTimestamp;

    private GravitySubject _gravity;
    private float underwaterModifier = 1.0f;
    //private MultiAudioListener _listener;

    private float _splashTimestamp = -1f;

    #region UnityMethods
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        _gravity = GetComponent<GravitySubject>();
        //_listener = GetComponent<MultiAudioListener>();
        InputManager.Instance.OnMove.AddListener(Moving);
        InputManager.Instance.OnLook.AddListener(Look);
        InputManager.Instance.OnStopMoving.AddListener(CancelMovement);
        InputManager.Instance.OnStopLooking.AddListener(CancelLook);
        InputManager.Instance.OnInteract.AddListener(Interact);
        InputManager.Instance.OnThrow.AddListener(ThrowItem);
        InputManager.Instance.OnJump.AddListener(Jump);
        InputManager.Instance.OnRun.AddListener(Run);
        InputManager.Instance.OnStopRun.AddListener(CancelRun);

        aimImg.sprite = aimSprites[0];
        Raycaster temp = gameObject.AddComponent<Raycaster>();
        temp.InitializeRaycaster(this, playerCam.transform);

        previousPosition = transform.position;
    }

    private void OnDestroy()
    {
        if (InputManager.Instance != null)
        {
            InputManager.Instance.OnJump.RemoveListener(Jump);
            InputManager.Instance.OnThrow.RemoveListener(ThrowItem);
            InputManager.Instance.OnStopLooking.RemoveListener(CancelLook);
            InputManager.Instance.OnLook.RemoveListener(Look);
            InputManager.Instance.OnInteract.RemoveListener(Interact);
            InputManager.Instance.OnStopMoving.RemoveListener(CancelMovement);
            InputManager.Instance.OnMove.RemoveListener(Moving);
            InputManager.Instance.OnRun.RemoveListener(Run);
            InputManager.Instance.OnStopRun.RemoveListener(CancelRun);
        }
        if (Instance == this)
            Instance = null;
    }

    private void LateUpdate()
    {
        if (!cinematicCameraDisplay)
            TurnPlayer();
    }
    private void Update()
    {
        if (inputDirection == Vector2.zero)
            Deccelerate();
        else
            Accelerate();
        onGround = Physics.Raycast(transform.position, -transform.up, 1.2f, 8); //layer 8 = ground
        if (!onGround)
            onGround = Physics.Raycast(transform.position, (-transform.up + transform.forward * 0.5f).normalized, 1.2f, 8);

        if (doubleJumpPerformed && onGround)
            doubleJumpPerformed = false;

        if (onAir && onGround)
        {
            displacementForFootstep = 0;
            if (_landSoundTimestamp <= Time.time - 0.21f)
            {
                _landSoundTimestamp = Time.time;
                //MultiAudioManager.PlayAudioObjectByIdentifier("footstep", transform);//TODO sfx footstep
                //MultiAudioManager.PlayAudioObjectByIdentifier("jumpland", transform); //TODO sfx landing from jump
            }
        }
        onAir = !onGround;
    }

    private void FixedUpdate()
    {
        if (inputDirection != Vector2.zero)
            MovePlayer();
        if (!onGround)
            return;
        displacementForFootstep += Vector2.Distance(new Vector2(previousPosition.x, previousPosition.z), new Vector2(transform.position.x, transform.position.z));
        if (displacementForFootstep > FootstepThreshold)
        {
            displacementForFootstep -= FootstepThreshold;
            //MultiAudioManager.PlayAudioObjectByIdentifier("footstep", transform);//TODO implement footstep
        }
        previousPosition = transform.position;
    }
    #endregion

    #region Movement
    private void TurnPlayer()
    {
        targetTurnRate = turnRate;// Mathf.SmoothStep(targetTurnRate, turnRate, 0.65f);
        transform.Rotate(0, turnSens * targetTurnRate * inputLookDirectionDelta.x, 0);
        Quaternion tempRotation = playerCam.transform.rotation;
        playerCam.transform.Rotate(-inputLookDirectionDelta.y * turnSens * targetTurnRate * 0.85f, 0, 0);
        if (Vector3.Dot(playerCam.transform.forward, transform.forward) < 0.1f)
            playerCam.transform.rotation = tempRotation;
        transform.rotation = Quaternion.LookRotation(Vector3.ProjectOnPlane(transform.forward, transform.position - GravitySource.Instance.transform.position), transform.position);
    }

    private void MovePlayer()
    {
        float jumpMod = 1.0f;
        if (!onGround)
            jumpMod = 0.75f;
        resultDirection = (transform.forward * inputDirection.y) + (transform.right * inputDirection.x);
        resultDirection = Vector3.ClampMagnitude(resultDirection, 1f);
        rb.AddForce(resultDirection * speed * runMod * jumpMod * underwaterModifier * Time.fixedDeltaTime, ForceMode.Acceleration);
    }

    private void Accelerate() => speed = Mathf.Clamp(speed += maxSpeed * 5f * inputDirection.magnitude * Time.deltaTime, maxSpeed / 3, maxSpeed);

    private void Deccelerate() => speed = Mathf.MoveTowards(speed, 0, maxSpeed * 5f * 10 * Time.deltaTime);
    #endregion

    //these come from the inputmanager
    #region Input Callbacks
    private void Run() => runMod = 2;
    private void CancelRun() => runMod = 1;
    private void Moving(Vector2 inputVector)
    {
        if (cinematicCameraDisplay)
            return;
        inputDirection = inputVector;
    }

    private void CancelMovement()
    {
        if (cinematicCameraDisplay)
            return;
        inputDirection = Vector2.zero;
    }

    private void Look(Vector2 inputVector)
    {
        if (cinematicCameraDisplay)
            return;
        inputLookDirectionDelta = inputVector;
    }

    private void CancelLook()
    {
        if (cinematicCameraDisplay)
            return;
        inputLookDirectionDelta = Vector2.zero;
    }

    private void Interact()
    {
        //if (cinematicCameraDisplay)
        //{
        //    //MultiAudioManager.PlayAudioObjectByIdentifier("", transform); //TODO sfx zoom back?
        //    DeactivateCamera();
        //    return;
        //}


        if (targetedInteractable == null)
        {
            if (currentlyHeldItem != null)
                DropHeldItem(2.2f);

            if (_clickingBoard)
                _clickingBoard = !BoardPopup.Instance.Unclick();

            return;
        }
        if (targetedInteractable.inactive)
            return;
        switch (targetedInteractable.type)
        {
            case InteractableObject.InteractableType.Pickable:
                if (Vector3.Distance(transform.position, targetedInteractable.transform.position) <= INTERACT_DISTANCE)
                {
                    if (currentlyHeldItem != null)
                        DropHeldItem(2.2f);
                    PickItem();
                }
                break;
            case InteractableObject.InteractableType.BoardClick:
                if (_clickingBoard)
                {
                    _clickingBoard = !BoardPopup.Instance.Unclick();
                    break;
                }
                if (Vector3.Distance(transform.position, targetedInteractable.transform.position) <= INTERACT_DISTANCE * 2)
                {
                    _clickingBoard = true;
                    targetedInteractable.CLickOnThis();
                }
                break;
            case InteractableObject.InteractableType.Other:
                if (Vector3.Distance(transform.position, targetedInteractable.transform.position) <= INTERACT_DISTANCE)
                {
                    targetedInteractable.CLickOnThis();
                }
                break;
            case InteractableObject.InteractableType.NPC:
                if (Vector3.Distance(transform.position, targetedInteractable.transform.position) <= INTERACT_DISTANCE)
                {
                    //TODO interact with NPC
                    var temp = targetedInteractable as ArgoNPC;
                    if (temp.wasClicked)
                        return;
                    temp.Poke();
                }
                break;
            default:
                break;
        }
    }

    private void ThrowItem()
    {
        if (cinematicCameraDisplay)
            return;
        if (currentlyHeldItem != null)
        {
            //MultiAudioManager.PlayAudioObjectByIdentifier("throw", currentlyHeldItem.transform);//TODO implement THROWING sfx
            DropHeldItem(throwStrength);
        }
    }

    private void Jump()
    {
        if (cinematicCameraDisplay)
            return;
        if (!onGround)
        {
            if (!doubleJumpAllowed)
                return;
            if (doubleJumpPerformed)
                return;

            if (underwaterModifier == 1f)
                doubleJumpPerformed = true;
        }
        //if (underwaterModifier == 1f)
        //    MultiAudioManager.PlayAudioObjectByIdentifier("jump", transform);
        //else
        //    MultiAudioManager.PlayAudioObjectByIdentifier("waterjump", transform);
        rb.AddForce(transform.up * jumpForce * underwaterModifier, ForceMode.Impulse);
    }
    #endregion

    #region Interacting
    //this was discontinued, it would used for the camera to move from the player perspective to a different transform temporarily (a viewpoint for the Leaderboard, for example. I'm keeping it here for now.)
    //private void ActivateCamera()
    //{
    //    CancelLook();
    //    CancelMovement();
    //    cinematicCameraActive = targetedInteractable;
    //    aimCanvas.SetActive(false);
    //    cinematicCameraDisplay = true;
    //    StartCoroutine(AnimationEnterCamera());
    //}

    //private IEnumerator AnimationEnterCamera()
    //{
    //    float f = 0;
    //    float increment = 0;
    //    WaitForFixedUpdate waitfor = new WaitForFixedUpdate();
    //    Vector3 initialPos = playerCam.transform.position;
    //    Quaternion initialRot = playerCam.transform.rotation;
    //    //MultiAudioManager.PlayAudioObjectByIdentifier("", currentCamera.transform); //TODO sound for zoom out?
    //    if (currentlyHeldItem != null)
    //        currentlyHeldItem.transform.SetParent(null);
    //    while (f < 1)
    //    {
    //        f += Time.fixedDeltaTime * increment;
    //        increment += 0.1f;
    //        playerCam.transform.position = Vector3.Lerp(initialPos, cinematicCameraActive.transform.position, f);
    //        playerCam.transform.rotation = Quaternion.Lerp(initialRot, cinematicCameraActive.transform.rotation, f);
    //        yield return waitfor;
    //    }
    //}

    //public void DeactivateCamera()
    //{
    //    playerCam.transform.localPosition = Vector3.zero;
    //    if (currentlyHeldItem != null)
    //        currentlyHeldItem.transform.SetParent(handTransform);
    //    aimCanvas.SetActive(true);
    //    cinematicCameraDisplay = false;
    //}


    private void PickItem()
    {
        targetedInteractable.transform.SetParent(handTransform);
        currentlyHeldItem = targetedInteractable;
        if (coroutineMovingItemToHand != null)
            StopCoroutine(coroutineMovingItemToHand);
        currentlyHeldItem.rb.isKinematic = true;
        currentlyHeldItem.col.enabled = false;
        currentlyHeldItem.PlaySound();
        currentlyHeldItem.itemHitNoise?.Grab();
        coroutineMovingItemToHand = StartCoroutine(MovingItemToHand(targetedInteractable.transform));
    }

    private IEnumerator MovingItemToHand(Transform trToMove)
    {
        WaitForFixedUpdate waitforit = new WaitForFixedUpdate();
        while (trToMove.localPosition != Vector3.zero)
        {
            trToMove.localPosition = Vector3.MoveTowards(trToMove.localPosition, Vector3.zero, Time.fixedDeltaTime * 12f);
            yield return waitforit;
        }
        coroutineMovingItemToHand = null;
    }

    private void DropHeldItem(float force)
    {
        currentlyHeldItem.transform.SetParent(null);
        if (currentlyHeldItem.rb != null)
        {
            currentlyHeldItem.rb.isKinematic = false;
            currentlyHeldItem.col.enabled = true;
            currentlyHeldItem.rb.AddForce(((playerCam.transform.forward * 2.6f) + (transform.up * 0.45f)) * force, ForceMode.Impulse);
        }
        currentlyHeldItem = null;
    }
    #endregion

    #region Raycaster Callbacks
    public void ClearTargetedInteractable()
    {
        aimImg.sprite = aimSprites[0];
        targetedInteractable = null;
        if (constantRefreshCoroutine != null)
        {
            StopCoroutine(constantRefreshCoroutine);
            constantRefreshCoroutine = null;
        }
    }

    public void AimAtInteractableObject(GameObject targetedObject)
    {
        targetedInteractable = targetedObject.GetComponent<InteractableObject>();
        if (targetedInteractable == null)
        {
            Debug.LogError("Targeted object is missing it's InteractableObject component.");
            return;
        }
        switch (targetedInteractable.type)
        {
            //// If needed to skip the distance check:
            //case InteractableObject.InteractableType:
            //    if (constantRefreshCoroutine != null)
            //    {
            //        StopCoroutine(constantRefreshCoroutine);
            //        constantRefreshCoroutine = null;
            //    }
            //    aimImg.sprite = aimSprites[2];
            //    break;
            case InteractableObject.InteractableType.BoardClick:
                if (constantRefreshCoroutine != null)
                    StopCoroutine(constantRefreshCoroutine);
                constantRefreshCoroutine = StartCoroutine(ConstantSpriteRefresh(INTERACT_DISTANCE * 2, targetedObject.transform, 2));
                break;
            case InteractableObject.InteractableType.NPC:
                //var temp = targetedInteractable as ArgoNPC;
                //if (temp.wasClicked)
                //    return;
                if (constantRefreshCoroutine != null)
                    StopCoroutine(constantRefreshCoroutine);
                constantRefreshCoroutine = StartCoroutine(ConstantSpriteRefresh(INTERACT_DISTANCE, targetedObject.transform, 4));
                break;
            default:
                if (constantRefreshCoroutine != null)
                    StopCoroutine(constantRefreshCoroutine);
                constantRefreshCoroutine = StartCoroutine(ConstantSpriteRefresh(INTERACT_DISTANCE, targetedObject.transform, 1));
                break;
        }
    }

    private IEnumerator ConstantSpriteRefresh(float allowedDistance, Transform targetTransform, int targetSpriteIndex)
    {
        WaitForFixedUpdate waitfor = new WaitForFixedUpdate();
        while (true)
        {

            if (Vector3.Distance(transform.position, targetTransform.position) <= allowedDistance && _clickingBoard == false)
                aimImg.sprite = aimSprites[targetSpriteIndex];
            else
                aimImg.sprite = aimSprites[0];

            yield return waitfor;
        }
    }
    #endregion

    #region Other Callbacks
    //not used but can be used to change the mouse/controller sensitivity
    public void SetCamSens(float value) => turnSens = value;

    public void Underwater(bool isUnder)
    {
        _gravity.Underwater(isUnder);
        //_listener.Underwater(isUnder);
        if (isUnder)
        {
            doubleJumpPerformed = false;
            underwaterModifier = 0.3f;
            //MultiAudioManager.PlayAudioObjectByIdentifier("underwater", 4, transform);
        }
        else
        {
            //MultiAudioManager.StopAudioSource(4); //stops channel 4 which is the underwater ambience above
            underwaterModifier = 1.0f;
        }
        //cooldown
        if (_splashTimestamp <= Time.time - 0.35f)
        {
            _splashTimestamp = Time.time;
            //MultiAudioManager.PlayAudioObjectByIdentifier("splash", transform);
        }
    }

    //this was made to make the player forcedly look to a target (while interacting with something) but was never used
    //public void ForceLookAt(Transform trToLookAt)
    //{
    //    StartCoroutine(ForcedLook(trToLookAt));
    //}
    //private IEnumerator ForcedLook(Transform target)
    //{
    //    float f = 0;
    //    WaitForFixedUpdate waitfor = new WaitForFixedUpdate();
    //    while (f < 35f)
    //    {
    //        f += Time.fixedDeltaTime;
    //        playerCam.transform.rotation = Quaternion.Slerp(playerCam.transform.rotation, Quaternion.LookRotation(target.position - playerCam.transform.position), 0.038f);
    //        yield return waitfor;
    //    }
    //}

    //part of the discontinued camera switching to a fixed position and back to the player.
    //public void ResetCam() => StartCoroutine(ResettingCamera());

    //private IEnumerator ResettingCamera()
    //{
    //    WaitForFixedUpdate waitfor = new WaitForFixedUpdate();
    //    while (playerCam.transform.localRotation != Quaternion.identity)
    //    {
    //        playerCam.transform.localRotation = Quaternion.Slerp(playerCam.transform.localRotation, Quaternion.identity, 0.05f);
    //        yield return waitfor;
    //    }
    //}
    #endregion
}