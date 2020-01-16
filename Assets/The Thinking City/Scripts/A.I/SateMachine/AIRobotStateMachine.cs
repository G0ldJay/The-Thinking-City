using UnityEngine;
using System.Collections;

// --------------------------------------------------------------------------
// CLASS	:	AIRobotStateMachine
// DESC		:	State Machine used by our Robot characters
// --------------------------------------------------------------------------
public class AIRobotStateMachine : AIStateMachine
{
    // Inspector Assigned
    [SerializeField] [Range(10.0f, 360.0f)] float _fov = 50.0f;    //Controls the A.Is field of view
    [SerializeField] [Range(0.0f, 1.0f)] float _sight = 0.5f;     //Controls the characters sight 
    [SerializeField] [Range(0.0f, 1.0f)] float _hearing = 1.0f;     //Controls the characters hearing range
    [SerializeField] [Range(0.0f, 1.0f)] float _aggression = 0.5f;     //Controls the A.Is level of aggression
    [SerializeField] [Range(0, 100)] int _health = 100;      //Controls the A.Is health
    [SerializeField] [Range(0.0f, 1.0f)] float _intelligence = 0.5f;     //Controls the A.Is intelligence
    [SerializeField] [Range(0.0f, 1.0f)] float _satisfaction = 1.0f;     //Satisfaction level of A.I (Testing with a feeding state but can be modified later)
    [SerializeField] float _replenishRate = 0.5f;     //Satisfaction replenish rate
    [SerializeField] float _depletionRate = 0.1f;     //Depletion of satisfaction (need to put in later)

    // Private
    private int _seeking = 0;        //Turn value sent to animation state machine 
    private bool _feeding = false;    //Bool for feeding
    private bool _crawling = false;    //If A.I is crawling (for robots with not legs, maybe?)
    private int _attackType = 0;        //Attack animation to use in animation state machine 
    private float _speed = 0.0f;     //Speed of A.I

    // Hashes
    private int _speedHash = Animator.StringToHash("Speed");     //Gets hash code of speed variable in animator
    private int _seekingHash = Animator.StringToHash("Seeking");   //Gets hash code of seeking variable in animator
    private int _feedingHash = Animator.StringToHash("Feeding");   //Gets hash code of feeding variable in animator
    private int _attackHash = Animator.StringToHash("Attack");    //Gets hash code of attack variable in animator


    // Public Properties
    public float replenishRate { get { return _replenishRate; } }                              //Gets replenished value of robot 
    public float fov { get { return _fov; } }                                     //Gets robots field of view 
    public float hearing { get { return _hearing; } }                                     //Gets robots hearing range
    public float sight { get { return _sight; } }                                     //Gets robots sight range
    public bool crawling { get { return _crawling; } }                                     //Gets if robot is a crawling type
    public float intelligence { get { return _intelligence; } }                                  //Gets intelligence of robot
    public float satisfaction { get { return _satisfaction; } set { _satisfaction = value; } }   //Gets and sets satisfaction level of A.I 
    public float aggression { get { return _aggression; } set { _aggression = value; } }     //Gets and sets the aggression level of A.I 
    public int health { get { return _health; } set { _health = value; } }       //Gets and sets the Health of the A.I
    public int attackType { get { return _attackType; } set { _attackType = value; } }     //Gets and sets the attack type
    public bool feeding { get { return _feeding; } set { _feeding = value; } }       //Gets and sets if feeding 
    public int seeking { get { return _seeking; } set { _seeking = value; } }       //Gets and sets seeking (Turn value)
    public float speed       //Gets and sets speed 
    {
        get { return _speed; }
        set { _speed = value; }
    }

    // ---------------------------------------------------------
    // Name	:	Update
    // Desc	:	Refresh the animator with up-to-date values for
    //			its parameters
    // ---------------------------------------------------------
    protected override void Update()
    {
        base.Update();

        if (_animator != null)
        {
            _animator.SetFloat(_speedHash, _speed);
            _animator.SetBool(_feedingHash, _feeding);
            _animator.SetInteger(_seekingHash, _seeking);
            _animator.SetInteger(_attackHash, _attackType);
        }
    }

    public override void TakeDamage(Vector3 position, Vector3 force, int damage, Rigidbody bodyPart, CharacterManager characterManager, int hitDirection = 0)
    {
        Debug.Log("OUCH!");

        if(GameManager.instance!=null && GameManager.instance.bloodParticles != null)
        {
            ParticleSystem sys = GameManager.instance.bloodParticles;
            sys.transform.position = position;
            var settings = sys.main;
            settings.simulationSpace = ParticleSystemSimulationSpace.World;
            sys.Emit(60);
        }

        health -= damage;

        float hitStrength = force.magnitude;
        bool shouldRagDoll = (hitStrength > 1.0f);
        if (health <= 0) shouldRagDoll = true;

        if (_navAgent) _navAgent.speed = 0;

        if (shouldRagDoll)
        {
            if (_navAgent) _navAgent.enabled = false;
            if (_animator) _animator.enabled = false;
            if (_collider) _collider.enabled = false;

            inMeleeRange = false;

            foreach (Rigidbody body in _bodyParts)
            {
                if (body)
                {
                    body.isKinematic = false;
                }
            }

            if (hitStrength > 1.0f) bodyPart.AddForce(force, ForceMode.Impulse);
        }
    }
}
