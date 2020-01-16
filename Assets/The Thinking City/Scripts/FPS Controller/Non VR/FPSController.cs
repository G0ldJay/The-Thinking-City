using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// Public Enun containing player controllers status
public enum PlayerMoveStatus { NotMoving, Walking, Running, NotGrounded, Landing, Crouching }

// Public Enum for callback type 
public enum CurveControlledBobCallBackType { Horizontal, Verticle }

//Callback for 
public delegate void CurveControlledBobCallBack();

// ----------------------------------------------------------------------
// Class	:	CurveControlledBobEvent
// Desc		:	Holds info related to registeredd event
// ----------------------------------------------------------------------
[System.Serializable]
public class CurveControlledBobEvent
{
    public float Time = 0.0f;
    public CurveControlledBobCallBack Function = null;
    public CurveControlledBobCallBackType Type = CurveControlledBobCallBackType.Verticle;
}

// ----------------------------------------------------------------------
// Class	:	CurveControlledHeadBob
// Desc		:	Controls the head bobbing functionailty of FPS Controller camera
// ----------------------------------------------------------------------
[System.Serializable]
public class CurveControlledHeadBob
{
    [SerializeField] AnimationCurve _headBobCurve = new AnimationCurve(new Keyframe(0f, 0f), new Keyframe(0.5f, 1f), new Keyframe(1f, 0f), new Keyframe(1.5f, -1f), new Keyframe(2f, 0f)); //Sets up curve graph in editor window with keyframes provided
    [SerializeField] private float _horizontalMultiplier = 0.02f; //Multiplier for horizontal camera bob
    [SerializeField] private float _verticleMultiplier = 0.03f; //Multiplier for verticle camera bob
    [SerializeField] private float _verticleToHorizontalSpeedRatio = 2.0f; //Speed ratio between the two
    [SerializeField] private float _baseInterval = 1.0f; //

    private float _prevXPlayHead; //Previous x value of camera 
    private float _prevYPlayHead; //Previous y value of camera
    private float _xPlayHead; //Current x value of camera
    private float _yPlayHead; //Current y value of camera 
    private float _curveEndTime; //End time of graph curve
    private List<CurveControlledBobEvent> _events = new List<CurveControlledBobEvent>(); //List to hold events related to the curve graph

    // ---------------------------------------------------------
    // Name	:	Initialize
    // Desc	:	Sets up start values for variables
    // ---------------------------------------------------------
    public void Initialize()
    {
        _curveEndTime = _headBobCurve[_headBobCurve.length - 1].time;
        _prevXPlayHead = 0.0f;
        _prevYPlayHead = 0.0f;
        _xPlayHead = 0.0f;
        _yPlayHead = 0.0f;
    }

    // ---------------------------------------------------------
    // Name	:	RegisterEventCallBack
    // Desc	:	Registers a CurveControlledBobEvent and adds it to the list of events
    // ---------------------------------------------------------
    public void RegisterEventCallBack(float time, CurveControlledBobCallBack function, CurveControlledBobCallBackType type)
    {
        CurveControlledBobEvent ccbeEvent = new CurveControlledBobEvent(); //Create new event
        ccbeEvent.Time = time; //Set its activation time 
        ccbeEvent.Function = function; //Set function to call on event
        ccbeEvent.Type = type; //Set type of event
        _events.Add(ccbeEvent); //Add event to the event list
        _events.Sort(delegate (CurveControlledBobEvent t1, CurveControlledBobEvent t2) { return (t1.Time.CompareTo(t2.Time)); }); //Sort event list by order of time. Had to provide sort functionality by means of anonymous function
    }

    // ---------------------------------------------------------
    // Name	:	GetVectorOffset
    // Desc	:	Returns the position of the camera 
    // ---------------------------------------------------------
    public Vector3 GetVectorOffset(float speed)
    {
        _xPlayHead += (speed * Time.deltaTime) / _baseInterval;
        _yPlayHead += ((speed * Time.deltaTime) / _baseInterval) * _verticleToHorizontalSpeedRatio;

        if (_xPlayHead > _curveEndTime) _xPlayHead -= _curveEndTime;

        if (_yPlayHead > _curveEndTime) _yPlayHead -= _curveEndTime;

        for (int i = 0; i < _events.Count; i++) //Iterate through events
        {
            CurveControlledBobEvent e = _events[i]; //Pick out event

            if (e != null) //Check to make sure its not null
            {
                if (e.Type == CurveControlledBobCallBackType.Verticle) //If the type of event is a verticle event 
                {
                    if ((_prevYPlayHead < e.Time && _yPlayHead >= e.Time) || (_prevYPlayHead > _yPlayHead && (e.Time > _prevYPlayHead || e.Time <= _yPlayHead))) //
                    {
                        e.Function(); //Call associated function
                    }
                }
                else //Else must be horizontal 
                {
                    if ((_prevXPlayHead < e.Time && _xPlayHead >= e.Time) || (_prevXPlayHead > _xPlayHead && (e.Time > _prevXPlayHead || e.Time <= _xPlayHead)))
                    {
                        e.Function(); //Call associated function
                    }
                }
            }
        }

        float xPos = _headBobCurve.Evaluate(_xPlayHead) * _horizontalMultiplier; //Returns a position in the graph curve for x
        float yPos = _headBobCurve.Evaluate(_yPlayHead) * _verticleMultiplier; //Returns position on graph curve for y

        _prevXPlayHead = _xPlayHead; //Set previous x position to current 
        _prevYPlayHead = _yPlayHead; //Set previous y position to current

        return new Vector3(xPos, yPos, 0f); //Return new (x,y) position for camera
    }

}

// ----------------------------------------------------------------------
// Class	:	FPSController
// Desc		:	Controls the player character from keyboard input
// ----------------------------------------------------------------------
[RequireComponent(typeof(CharacterController))]
public class FPSController : MonoBehaviour
{
    public List<AudioSource> AudioSources = new List<AudioSource>(); //List of audio for event testing
    private int _audioToUse = 0; //Start of list
    //DISPOSE OF ABOVE AFTER SOUND SYSTEM IS CONSTRUCTED!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

    // Inspector Assigned Locomotion Settings
    [SerializeField] private float _walkSpeed = 2.0f; //The players walk speed
    [SerializeField] private float _runSpeed = 4.5f; //The players run speed
    [SerializeField] private float _jumpSpeed = 7.5f; //The players jump speed
    [SerializeField] private float _crouchSpeed = 1.0f; //The players crouch speed
    [SerializeField] private float _stickToGroundForce = 5.0f; //gravity to affect player since character controller doesn't use physics engine built in
    [SerializeField] private float _gravityMultiplier = 2.5f; //Mutiplier for gravities effect on player 
    [SerializeField] private float _runStepLengthen = 0.75f; //The step lengthen while running. Coz when running you take bigger steps
    [SerializeField] private CurveControlledHeadBob _headBob = new CurveControlledHeadBob(); //Instance of CurveControlledHeadBob class 
    [SerializeField] private GameObject _flashLight = null; //Flashlight component. NOTE : May or may not use. Currently poorly built for quick testing.

    // Use Standard Assets Mouse Look class for mouse input -> Camera Look Control
    [SerializeField] private UnityStandardAssets.Characters.FirstPerson.MouseLook _mouseLook = new UnityStandardAssets.Characters.FirstPerson.MouseLook(); //Instance of MouseLook script for player movement. Standard Unity asset script.

    // Private internals
    private Camera _camera = null; //Holds player camera 
    private bool _jumpButtonPressed = false; //Holds value for if jump button is pressed. Stop infinite jumping.
    private Vector2 _inputVector = Vector2.zero; //Input from mouse
    private Vector3 _moveDirection = Vector3.zero; //Direction of player movement
    private bool _previouslyGrounded = false; //Holds value for if player is touching ground
    private bool _isWalking = true; //Holds value for if player is walking
    private bool _isJumping = false; //Holds value for if player is jumping
    private bool _isCrouching = false; //Holds value for if player is crouching
    private Vector3 _localSpaceCameraPos = Vector3.zero; //Cameras position in local space relative to parent gameobject 
    private float _controllerHeight = 0.0f; //Height of player controller capsule

    // Timers
    private float _fallingTimer = 0.0f; //Timer of how long player has been falling for

    private CharacterController _characterController = null; //Holds a reference to the character controller object
    private PlayerMoveStatus _movementStatus = PlayerMoveStatus.NotMoving; //Movement ststus of the character

    // Public Properties
    public PlayerMoveStatus movementStatus { get { return _movementStatus; } } //Getter for the movement status of player
    public float walkSpeed { get { return _walkSpeed; } } //Getter for the walking speed
    public float runSpeed { get { return _runSpeed; } } //Getter for the run speed

    protected void Start()
    {
        // Cache component references
        _characterController = GetComponent<CharacterController>();

        _controllerHeight = _characterController.height;

        // Get the main camera and cache local position within the FPS rig 
        _camera = Camera.main;

        //Stores local space of camera game object
        _localSpaceCameraPos = _camera.transform.localPosition;

        // Set initial to not jumping and not moving
        _movementStatus = PlayerMoveStatus.NotMoving;

        // Reset timers
        _fallingTimer = 0.0f;

        // Setup Mouse Look Script
        _mouseLook.Init(transform, _camera.transform);

        //Call initialize function to set up head bob
        _headBob.Initialize();

        //Register sound to verticle movement event 
        _headBob.RegisterEventCallBack(1.5f, PlayFootStepSound, CurveControlledBobCallBackType.Verticle);

        //If flashlight is attached turn off gameobject
        if (_flashLight) _flashLight.SetActive(false);
    }

    protected void Update()
    {
        // If on ground set timer to 0 else increment timer
        if (_characterController.isGrounded) _fallingTimer = 0.0f;
        else _fallingTimer += Time.deltaTime;

        // Allow Mouse Look a chance to process mouse and rotate camera
        if (Time.timeScale > Mathf.Epsilon)
            _mouseLook.LookRotation(transform, _camera.transform);

        //If flashlight button is pressed "F"
        if (Input.GetButtonDown("Flashlight"))
        {
            if (_flashLight) _flashLight.SetActive(!_flashLight.activeSelf); //Either turn it on or off
        }

        // Process the Jump Button
        // the jump state needs to read here to make sure it is not missed
        if (!_jumpButtonPressed && !_isCrouching)
            _jumpButtonPressed = Input.GetButtonDown("Jump");

        //If crouch button is pressed "left ctrl"
        if (Input.GetButtonDown("Crouch"))
        {
            _isCrouching = !_isCrouching; //Whatever it is set it to opposite
            _characterController.height = _isCrouching == true ? _controllerHeight / 2.0f : _controllerHeight; //If crouching is true half player capsule else make it full height
        }

        // Calculate Character Status
        if (!_previouslyGrounded && _characterController.isGrounded)
        {
            if (_fallingTimer > 0.5f)
            {
                // TODO: Play Landing Sound
            }

            _moveDirection.y = 0f; //Set movement in y direction to 0
            _isJumping = false; //Jumping value to false
            _movementStatus = PlayerMoveStatus.Landing; //Player status to landing
        }
        else if (!_characterController.isGrounded) //If we are not grounded 
            _movementStatus = PlayerMoveStatus.NotGrounded; //Set status to in the air 
        else if (_characterController.velocity.sqrMagnitude < 0.01f) //If our velocity is < 0.01
            _movementStatus = PlayerMoveStatus.NotMoving; //Set status to not moving
        else if (_isCrouching) //If crouching is true
            _movementStatus = PlayerMoveStatus.Crouching; //Set status to crouching
        else if (_isWalking) //If walking is true 
            _movementStatus = PlayerMoveStatus.Walking; //Set status to walking 
        else //Else we have to be running
            _movementStatus = PlayerMoveStatus.Running; //Set status to running

        _previouslyGrounded = _characterController.isGrounded; //Set previously grounded value to current is grounded value
    }

    protected void FixedUpdate()
    {
        // Read input from axis
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        bool waswalking = _isWalking;
        _isWalking = !Input.GetKey(KeyCode.LeftShift);

        // Set the desired speed to be either our walking speed or our running speed
        float speed = _isCrouching ? _crouchSpeed : _isWalking ? _walkSpeed : _runSpeed;
        _inputVector = new Vector2(horizontal, vertical);

        // normalize input if it exceeds 1 in combined length:
        if (_inputVector.sqrMagnitude > 1) _inputVector.Normalize();

        // Always move along the camera forward as it is the direction that it being aimed at
        Vector3 desiredMove = transform.forward * _inputVector.y + transform.right * _inputVector.x;

        // Get a normal for the surface that is being touched to move along it
        RaycastHit hitInfo;
        if (Physics.SphereCast(transform.position, _characterController.radius, Vector3.down, out hitInfo, _characterController.height / 2f, 1))
            desiredMove = Vector3.ProjectOnPlane(desiredMove, hitInfo.normal).normalized;

        // Scale movement by our current speed (walking value or running value)
        _moveDirection.x = desiredMove.x * speed;
        _moveDirection.z = desiredMove.z * speed;

        // If grounded
        if (_characterController.isGrounded)
        {
            // Apply severe down force to keep control sticking to floor
            _moveDirection.y = -_stickToGroundForce;

            // If the jump button was pressed then apply speed in up direction
            // and set isJumping to true. Also, reset jump button status
            if (_jumpButtonPressed)
            {
                _moveDirection.y = _jumpSpeed;
                _jumpButtonPressed = false;
                _isJumping = true;
                // TODO: Play Jumping Sound
            }
        }
        else
        {
            // Otherwise we are not on the ground so apply standard system gravity multiplied
            // by our gravity modifier
            _moveDirection += Physics.gravity * _gravityMultiplier * Time.fixedDeltaTime;
        }

        // Move the Character Controller
        _characterController.Move(_moveDirection * Time.fixedDeltaTime);

        Vector3 speedXZ = new Vector3(_characterController.velocity.x, 0.0f, _characterController.velocity.z);

        if (speedXZ.magnitude > 0.01f)
            _camera.transform.localPosition = _localSpaceCameraPos + _headBob.GetVectorOffset(speedXZ.magnitude * (_isCrouching||_isWalking ? 1 : _runStepLengthen));
        else
            _camera.transform.localPosition = _localSpaceCameraPos;

        //BROKEN : causes footsteps to play on crouch. CAUSE : Magnitudes Y velocity needs to be flattened to stop velocity cosideration on Y-Axis.
        //if (_characterController.velocity.magnitude > 0.01f)
        //    _camera.transform.localPosition = _localSpaceCameraPos + _headBob.GetVectorOffset(_characterController.velocity.magnitude * (_isWalking ? 1 : _runStepLengthen));
        //else
        //    _camera.transform.localPosition = _localSpaceCameraPos;
    }

    void PlayFootStepSound()
    {
        AudioSources[_audioToUse].Play();
        _audioToUse = (_audioToUse == 0) ? 1 : 0;
    }
}
