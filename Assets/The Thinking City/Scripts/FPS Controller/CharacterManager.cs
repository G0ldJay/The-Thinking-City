using UnityEngine;
using System.Collections;

public class CharacterManager : MonoBehaviour
{
    // Inspector Assigned
    [SerializeField] private GameObject _vrPlayer = null;  
    [SerializeField] private CapsuleCollider _meleeTrigger = null;
    //[SerializeField] private CameraBloodEffect	_cameraBloodEffect 	= null; TODO look into blood effect on HDRP
    [SerializeField] private Camera _camera = null;
    [SerializeField] private float _health = 100.0f;

    // Private
    private Collider _collider = null;
    private FPSController _fpsController = null;
    private CharacterController _characterController = null;
    private GameManager _gameManager = null;
    private int _aiBodyPartLayer = -1;

    // Use this for initialization
    void Start()
    {
        _collider = _vrPlayer.GetComponent<Collider>();
        _fpsController = _vrPlayer.GetComponent<FPSController>();
        _characterController = _vrPlayer.GetComponent<CharacterController>();
        _gameManager = GameManager.instance;

        _aiBodyPartLayer = LayerMask.NameToLayer("AI Body Part");

        if (_gameManager != null)
        {
            PlayerInfo info = new PlayerInfo();
            info.camera = _camera;
            info.characterManager = this;
            info.collider = _collider;
            info.meleeTrigger = _meleeTrigger;

            _gameManager.RegisterPlayerInfo(_collider.GetInstanceID(), info);
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            DoDamage();
        }
    }

    public void TakeDamage(float amount)
    {
        _health = Mathf.Max(_health - (amount * Time.deltaTime), 0.0f);
        //if (_cameraBloodEffect!=null)
        //{
        //	_cameraBloodEffect.minBloodAmount = (1.0f - _health/100.0f);
        //	_cameraBloodEffect.bloodAmount = Mathf.Min(_cameraBloodEffect.minBloodAmount + 0.3f, 1.0f);	
        //}
        //      else
        //      {
        //          Debug.Log("Missing camera blood effect script");
        //      }
    }

    public void DoDamage(int hitDirection = 0)
    {
        if (_camera      == null) return;
        if (_gameManager == null) return;

        Ray ray;
        RaycastHit hit;
        bool isSomethingHit = false;

        ray = _camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));

        isSomethingHit = Physics.Raycast(ray, out hit, 1000.0f, 1 << _aiBodyPartLayer);

        if (isSomethingHit)
        {
            AIStateMachine statemachine = _gameManager.GetAIStateMachine(hit.rigidbody.GetInstanceID());

            if (statemachine)
            {
                statemachine.TakeDamage(hit.point, ray.direction * 1.0f, 25, hit.rigidbody, this, 0); //TODO : PHYSICS Change hard coded values
            }
        }
    }
}
