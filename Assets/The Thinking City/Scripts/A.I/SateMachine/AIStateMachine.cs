using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;

// Public Enums of the AI System
public enum AIStateType { None, Idle, Alerted, Patrol, Attack, Feeding, Pursuit, Dead } //Enum for the state types the A.I can move between
public enum AITargetType { None, Waypoint, Visual_Player, Visual_Light, Visual_Food, Audio } //Enums for the current target type 
public enum AITriggerEventType { Enter, Stay, Exit } //Enums for the targets location relevant to the sensor radius 

// ----------------------------------------------------------------------
// Class	:	AITarget
// Desc		:	Describes a potential target to the AI System
// ----------------------------------------------------------------------
public struct AITarget
{
    private AITargetType _type;     // The type of target
    private Collider _collider;     // The collider
    private Vector3 _position;      // Current position in the world
    private float _distance;        // Distance from player
    private float _time;            // Time the target was last ping'd

    public AITargetType type { get { return _type; } }                              //Returns AITargetType 
    public Collider collider { get { return _collider; } }                          //Returns the collider 
    public Vector3 position { get { return _position; } }                           //Returns position 
    public float distance { get { return _distance; } set { _distance = value; } }  //Get and set distance
    public float time { get { return _time; } }                                     //Returns time

    // -----------------------------------------------------------------
    // Name	:	Set
    // Desc	:	Sets up AITargets location 
    // -----------------------------------------------------------------
    public void Set(AITargetType t, Collider c, Vector3 p, float d)
    {
        _type = t; //Type of target 
        _collider = c; //Collider of target 
        _position = p; //Position of target 
        _distance = d; //Distance to target 
        _time = Time.time; //Time target was set 
    }

    // -----------------------------------------------------------------
    // Name	:	Clear
    // Desc	:	Clears AITarget Values and sets them to default
    // -----------------------------------------------------------------
    public void Clear()
    {
        _type = AITargetType.None; //Sets target type to none
        _collider = null; //Clears collider
        _position = Vector3.zero; //Sets position to (0,0,0)
        _time = 0.0f; //Sets time to 0
        _distance = Mathf.Infinity; //Sets distance to infinity 
    }
}

// ----------------------------------------------------------------------
// Class	:	AIStateMachine
// Desc		:	Base class for all AI State Machines
// ----------------------------------------------------------------------
public abstract class AIStateMachine : MonoBehaviour
{
    // Public
    public AITarget VisualThreat = new AITarget();  //Set up visual threat 
    public AITarget AudioThreat = new AITarget();   //Set up audio threat 

    // Protected
    protected AIState _currentState = null;                                                         //Current state of state machine
    protected Dictionary<AIStateType, AIState> _states = new Dictionary<AIStateType, AIState>();    //Disctionary of state types and state
    protected AITarget _target = new AITarget();                                                    //Set up target
    protected int _rootPositionRefCount = 0;                                                        //Used for determining when to use root motion
    protected int _rootRotationRefCount = 0;                                                        //Used for determining when to use root rotation
    protected bool _isTargetReached = false;                                                        //Detects if we have reached the target

    // Protected Inspector Assigned
    [SerializeField] protected AIStateType _currentStateType = AIStateType.Idle;    //Sets current state type to idle 
    [SerializeField] protected SphereCollider _targetTrigger = null;                //Target trigger GameObject (Location to move to, waypoint, player, noise, etc)
    [SerializeField] protected SphereCollider _sensorTrigger = null;                //Sensor trigger GameObject (Hearing and Sight)
    [SerializeField] protected AIWaypointNetwork _waypointNetwork = null;           //The AIs waypoint network for patrolling
    [SerializeField] protected bool _randomPatrol = false;                          //Make it patrol points randomly or in order 
    [SerializeField] protected int _currentWaypoint = -1;                           //Holds current waypoint
    [SerializeField] [Range(0, 15)] protected float _stoppingDistance = 1.0f;       //Acceptable margin or error to target (if <=1 meter from target you have arrived)

    // Component Cache
    protected Animator _animator = null;        //Holds AIs animator
    protected NavMeshAgent _navAgent = null;    //Holds AIs NavAgent
    protected Collider _collider = null;        //Holds AIs Collider
    protected Transform _transform = null;      //Holds AIs transform

    // Public Properties
    public bool isTargetReached { get { return _isTargetReached; } }    //Returns if target is reached
    public bool inMeleeRange { get; set; }                              //Returns if target is in melee range of player
    public Animator animator { get { return _animator; } }              //Returns the animtor of the AI
    public NavMeshAgent navAgent { get { return _navAgent; } }          //Returns the NavAgent of the AI
    public Vector3 sensorPosition                                       //Gets position of the senor trigger
    {
        get
        {
            if (_sensorTrigger == null) return Vector3.zero;                                //If theres none attached then return 0
            Vector3 point = _sensorTrigger.transform.position;                              //Get position of sensor
            point.x += _sensorTrigger.center.x * _sensorTrigger.transform.lossyScale.x;     //Adjust to global scale since its a child
            point.y += _sensorTrigger.center.y * _sensorTrigger.transform.lossyScale.y;     //Adjust to global scale since its a child
            point.z += _sensorTrigger.center.z * _sensorTrigger.transform.lossyScale.z;     //Adjust to global scale since its a child
            return point;                                                                   //Return the new vector 3 position
        }
    }

    public float sensorRadius //Gets the radius of the senor trigger
    {
        get
        {
            if (_sensorTrigger == null) return 0.0f;                                                                                                                    //If theres none attached then return 0
            float radius = Mathf.Max(_sensorTrigger.radius * _sensorTrigger.transform.lossyScale.x, _sensorTrigger.radius * _sensorTrigger.transform.lossyScale.y);     //Gets x and y radius and returns the max of the two 

            return Mathf.Max(radius, _sensorTrigger.radius * _sensorTrigger.transform.lossyScale.z);                                                                    //Returns the max of either the above result or the z radius
        }
    }

    public bool useRootPosition { get { return _rootPositionRefCount > 0; } }   //Returns if root motion should be used or not 
    public bool useRootRotation { get { return _rootRotationRefCount > 0; } }   //Returns if root rotation should be used or not 
    public AITargetType targetType { get { return _target.type; } }             //Returns the target type (e.g Player, Waypoint, etc)
    public Vector3 targetPosition { get { return _target.position; } }          //Returns position of its current target 
    public int targetColliderID                                                 //Returns the ID attached to the targets collider
    {
        get
        {
            if (_target.collider)                           //If there is a collider 
                return _target.collider.GetInstanceID();    //Get and return its ID
            else
                return -1;                                  //Otherwise return -1
        }
    }

    // -----------------------------------------------------------------
    // Name	:	Awake
    // Desc	:	Cache Components
    // -----------------------------------------------------------------
    protected virtual void Awake()
    {
        // Cache all frequently accessed components
        _transform = transform;                     //Holds reference to Transform of AI
        _animator = GetComponent<Animator>();       //Holds reference to AIs animator
        _navAgent = GetComponent<NavMeshAgent>();   //Holds reference to AIs NavMeshAgent
        _collider = GetComponent<Collider>();       //Holds reference to AIs collider 

        // Do we have a valid Game Scene Manager
        if (GameManager.instance != null)                                                                               //If the GameManager exist in the scene
        {
            // Register State Machines with Scene Database
            if (_collider) GameManager.instance.RegisterAIStateMachine(_collider.GetInstanceID(), this);                //Registers AI capsule collider in dictionary if there is one      
            if (_sensorTrigger) GameManager.instance.RegisterAIStateMachine(_sensorTrigger.GetInstanceID(), this);      //Registers AI sensor trigger in dictionary if there is one
        }

    }

    // -----------------------------------------------------------------
    // Name	:	Start
    // Desc	:	Called by Unity prior to first update to setup the object
    // -----------------------------------------------------------------
    protected virtual void Start()
    {
        // Set the sensor trigger's parent to this state machine
        if (_sensorTrigger != null)
        {
            AISensor script = _sensorTrigger.GetComponent<AISensor>();  //Gets script attachedd to the sensor trigger
            if (script != null)                                         //If there is one
            {
                script.parentStateMachine = this;                       //Assign its parent state machine to this one
            }
        }


        // Fetch all states on this game object
        AIState[] states = GetComponents<AIState>();

        // Loop through all states and add them to the state dictionary
        foreach (AIState state in states)
        {
            if (state != null && !_states.ContainsKey(state.GetStateType()))    //If state dictionary exists and doesn't already have a state of that type
            {
                // Add this state to the state dictionary
                _states[state.GetStateType()] = state;

                // And set the parent state machine of this state
                state.SetStateMachine(this);
            }
        }

        // Set the current state
        if (_states.ContainsKey(_currentStateType))
        {
            _currentState = _states[_currentStateType];     //Set current state to state of state type
            _currentState.OnEnterState();                   //Call it's on enter state function 
        }
        else
        {
            _currentState = null;                           //Else state can't be found
        }

        // Fetch all AIStateMachineLink derived behaviours from the animator
        // and set their State Machine references to this state machine
        if (_animator)
        {
            AIStateMachineLink[] scripts = _animator.GetBehaviours<AIStateMachineLink>();
            foreach (AIStateMachineLink script in scripts)
            {
                script.stateMachine = this;
            }
        }
    }


    // -----------------------------------------------------------------------------
    // Name	:	GetWaypointPosition
    // Desc	:	Fetched the world space position of the state machine's currently
    //			set waypoint with optional increment
    // -----------------------------------------------------------------------------
    public Vector3 GetWaypointPosition(bool increment)
    {
        if (_currentWaypoint == -1)                                                     //If current waypoint == -1 (e.g first time in this function)
        {
            if (_randomPatrol)                                                          //Check if random                                                    
                _currentWaypoint = Random.Range(0, _waypointNetwork.Waypoints.Count);   //If yes pick a random waypoint
            else
                _currentWaypoint = 0;                                                   //Else start at beginning
        }
        else if (increment)                                                             //If not first time in this function 
            NextWaypoint();                                                             //Get next waypoint

        // Fetch the new waypoint from the waypoint list
        if (_waypointNetwork.Waypoints[_currentWaypoint] != null)                       //If waypoint is not null
        {
            Transform newWaypoint = _waypointNetwork.Waypoints[_currentWaypoint];       //Get its transform

            // This is our new target position
            SetTarget(AITargetType.Waypoint,null,newWaypoint.position,Vector3.Distance(newWaypoint.position, transform.position));  //Set it up as the next target 

            return newWaypoint.position;    //Return its position
        }

        return Vector3.zero;    //If null return 0 vector
    }

    // -------------------------------------------------------------------------
    // Name	:	NextWaypoint
    // Desc	:	Called to select a new waypoint. Either randomly selects a new
    //			waypoint from the waypoint network or increments the current
    //			waypoint index (with wrap-around) to visit the waypoints in
    //			the network in sequence. Sets the new waypoint as the the
    //			target and generates a nav agent path for it
    // -------------------------------------------------------------------------
    private void NextWaypoint()
    {
        // Increase the current waypoint with wrap-around to zero (or choose a random waypoint)
        if (_randomPatrol && _waypointNetwork.Waypoints.Count > 1)
        {
            // Keep generating random waypoint until we find one that isn't the current one
            // NOTE: Very important that waypoint networks do not only have one waypoint :)
            int oldWaypoint = _currentWaypoint;
            while (_currentWaypoint == oldWaypoint)
            {
                _currentWaypoint = Random.Range(0, _waypointNetwork.Waypoints.Count); //Gets random waypoint
            }
        }
        else
            _currentWaypoint = _currentWaypoint == _waypointNetwork.Waypoints.Count - 1 ? 0 : _currentWaypoint + 1; //Else iterates to next point


    }
    // -------------------------------------------------------------------
    // Name :	SetTarget (Overload)
    // Desc	:	Sets the current target and configures the target trigger
    // -------------------------------------------------------------------
    public void SetTarget(AITargetType t, Collider c, Vector3 p, float d)
    {
        // Set the target info
        _target.Set(t, c, p, d);

        // Configure and enable the target trigger at the correct
        // position and with the correct radius
        if (_targetTrigger != null)
        {
            _targetTrigger.radius = _stoppingDistance;
            _targetTrigger.transform.position = _target.position;
            _targetTrigger.enabled = true;
        }
    }

    // --------------------------------------------------------------------
    // Name :	SetTarget (Overload)
    // Desc	:	Sets the current target and configures the target trigger.
    //			This method allows for specifying a custom stopping
    //			distance.
    // --------------------------------------------------------------------
    public void SetTarget(AITargetType t, Collider c, Vector3 p, float d, float s)
    {
        // Set the target Data
        _target.Set(t, c, p, d);

        // Configure and enable the target trigger at the correct
        // position and with the correct radius
        if (_targetTrigger != null)
        {
            _targetTrigger.radius = s;
            _targetTrigger.transform.position = _target.position;
            _targetTrigger.enabled = true;
        }
    }

    // -------------------------------------------------------------------
    // Name :	SetTarget (Overload)
    // Desc	:	Sets the current target and configures the target trigger
    // -------------------------------------------------------------------
    public void SetTarget(AITarget t)
    {
        // Assign the new target
        _target = t;

        // Configure and enable the target trigger at the correct
        // position and with the correct radius
        if (_targetTrigger != null)
        {
            _targetTrigger.radius = _stoppingDistance;
            _targetTrigger.transform.position = t.position;
            _targetTrigger.enabled = true;
        }
    }

    // -------------------------------------------------------------------
    // Name :	ClearTarget 
    // Desc	:	Clears the current target
    // -------------------------------------------------------------------
    public void ClearTarget()
    {
        _target.Clear();
        if (_targetTrigger != null)
        {
            _targetTrigger.enabled = false;
        }
    }

    // -------------------------------------------------------------------
    // Name :	FixedUpdate
    // Desc	:	Called by Unity with each tick of the Physics system. It
    //			clears the audio and visual threats each update and
    //			re-calculates the distance to the current target
    // --------------------------------------------------------------------
    protected virtual void FixedUpdate()
    {
        VisualThreat.Clear(); //Clears visual threats
        AudioThreat.Clear(); //Clears audio threats

        if (_target.type != AITargetType.None) //if target type is not empty 
        {
            _target.distance = Vector3.Distance(_transform.position, _target.position); //Re-calculate distnace to target 
        }

        _isTargetReached = false; //Sets reached target bool to false
    }

    // -------------------------------------------------------------------
    // Name :	Update
    // Desc	:	Called by Unity each frame. Gives the current state a
    //			chance to update itself and perform transitions.
    // -------------------------------------------------------------------
    protected virtual void Update()
    {
        if (_currentState == null) return; //If theres no state

        AIStateType newStateType = _currentState.OnUpdate(); //Call onUpdate function in the states script

        if (newStateType != _currentStateType)  //If its a new state 
        {
            AIState newState = null; //Set up a state

            if (_states.TryGetValue(newStateType, out newState)) //Searches dictionary for that state 
            {
                _currentState.OnExitState(); //Exits the current state 
                newState.OnEnterState(); //Enters the new state
                _currentState = newState; //Sets new state to the current one 
            }
            else if (_states.TryGetValue(AIStateType.Idle, out newState)) //If the state cant be found it defaults to idle
            {
                _currentState.OnExitState(); //Exits the current state 
                newState.OnEnterState(); //Enters the new state
                _currentState = newState; //Sets new state to the current one 
            }

            _currentStateType = newStateType; //Sets current state type enum to the new state type enum (e.g patrol, idle, etc)
        }
    }

    // --------------------------------------------------------------------------
    //	Name	:	OnTriggerEnter
    //	Desc	:	Called by Physics system when the AI's Main collider enters
    //				its trigger. This allows the child state to know when it has 
    //				entered the sphere of influence	of a waypoint or last player 
    //				sighted position.
    // --------------------------------------------------------------------------
    protected virtual void OnTriggerEnter(Collider other)
    {
        if (_targetTrigger == null || other != _targetTrigger) return; //If target trigger is null or collider is not target trigger

        _isTargetReached = true; //Else target is reached 

        // Notify Child State
        if (_currentState)
            _currentState.OnDestinationReached(true); //Set destination reached on state machine
    }

    protected virtual void OnTriggerStay(Collider other)
    {
        if (_targetTrigger == null || other != _targetTrigger) return; //If target trigger is null or collider is not target trigger

        _isTargetReached = true; //Else target is reached 
    }

    // --------------------------------------------------------------------------
    //	Name	:	OnTriggerExit
    //	Desc	:	Informs the child state that the AI entity is no longer at
    //				its destination (typically true when a new target has been
    //				set by the child.
    // --------------------------------------------------------------------------
    protected void OnTriggerExit(Collider other)
    {
        if (_targetTrigger == null || _targetTrigger != other) return; //If target trigger is null or collider is not target trigger

        _isTargetReached = false; //Else target isn't reached 

        if (_currentState != null)
            _currentState.OnDestinationReached(false); //Set destination reached to false on state machine
    }

    // ------------------------------------------------------------
    // Name	:	OnTriggerEvent
    // Desc	:	Called by our AISensor component when an AI Aggravator
    //			has entered/exited the sensor trigger.
    // -------------------------------------------------------------
    public virtual void OnTriggerEvent(AITriggerEventType type, Collider other)
    {
        if (_currentState != null)
            _currentState.OnTriggerEvent(type, other); //Call trigger event in state script
    }

    // -----------------------------------------------------------
    // Name	:	OnAnimatorMove
    // Desc	:	Called by Unity after root motion has been
    //			evaluated but not applied to the object.
    //			This allows us to determine via code what to do
    //			with the root motion information
    // -----------------------------------------------------------
    protected virtual void OnAnimatorMove()
    {
        if (_currentState != null)
            _currentState.OnAnimatorUpdated();
    }

    // ----------------------------------------------------------
    // Name	: OnAnimatorIK
    // Desc	: Called by Unity just prior to the IK system being
    //		  updated giving us a chance to setup up IK Targets
    //		  and weights.
    // ----------------------------------------------------------
    protected virtual void OnAnimatorIK(int layerIndex)
    {
        if (_currentState != null)
            _currentState.OnAnimatorIKUpdated();
    }

    // ----------------------------------------------------------
    // Name	:	NavAgentControl
    // Desc	:	Configure the NavMeshAgent to enable/disable auto
    //			updates of position/rotation to our transform
    // ----------------------------------------------------------
    public void NavAgentControl(bool positionUpdate, bool rotationUpdate)
    {
        if (_navAgent)
        {
            _navAgent.updatePosition = positionUpdate;
            _navAgent.updateRotation = rotationUpdate;
        }
    }

    // ----------------------------------------------------------
    // Name	:	AddRootMotionRequest
    // Desc	:	Called by the State Machine Behaviours to
    //			Enable/Disable root motion
    // ----------------------------------------------------------
    public void AddRootMotionRequest(int rootPosition, int rootRotation)
    {
        _rootPositionRefCount += rootPosition;
        _rootRotationRefCount += rootRotation;
    }
}
