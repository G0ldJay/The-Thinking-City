using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorePowerUpQuest : MonoBehaviour
{
    [SerializeField] private GameObject _greenPowerUnit = null;
    [SerializeField] private GameObject _redPowerUnit   = null;
    [SerializeField] private GameObject _bluePowerUnit  = null;

    [SerializeField] private Material _greenPowerUnitOnMaterial = null;
    [SerializeField] private Material _redPowerUnitOnMaterial   = null;
    [SerializeField] private Material _bluePowerUnitOnMaterial  = null;

    [SerializeField] private GameObject _staticGreenCard    = null;
    [SerializeField] private GameObject _staticRedCard      = null;
    [SerializeField] private GameObject _staticBlueCard     = null;

    [SerializeField] private GameObject _cardInsertSoundA = null;
    [SerializeField] private GameObject _cardInsertSoundB = null;
    [SerializeField] private GameObject _cardInsertSoundC = null;

    [SerializeField] private GameObject _powerUpSound = null;

    [SerializeField] private BoxCollider _greenCardSlot    = null;
    [SerializeField] private BoxCollider _redCardSlot      = null;
    [SerializeField] private BoxCollider _blueCardSlot     = null;

    [SerializeField] private GameObject _coreReactorEffect  = null;
    [SerializeField] private GameObject[] _lightningEffects = null;
    [SerializeField] private GameObject _coreKlaxon         = null;

    [SerializeField] private BoxCollider _securityRoomShutterTrigger    = null;
    [SerializeField] private GameObject _securityRoomShutters           = null;

    [SerializeField] private float _shutterUpMoveDistance = 1.5f;
    [SerializeField] private float _shutterDuration = 2.0f;

    [SerializeField] private DoorFunctionality[] _securityDoors = null;

    public AnimationCurve JumpCurve = new AnimationCurve();

    private MeshRenderer _greenPowerUnitMaterial    = null;
    private MeshRenderer _redPowerUnitMaterial      = null;
    private MeshRenderer _bluePowerUnitMaterial     = null;

    private bool _greenCardOnline   = false;
    private bool _redCardOnline     = false;
    private bool _blueCardOnline    = false;
    private bool _coreOnline        = false;

    private Vector3 _shutterStartPosition = Vector3.zero;
    private Vector3 _shutterEndPosition = Vector3.zero;

    public bool coreOnline { get { return _coreOnline; } }

    // Start is called before the first frame update
    void Start()
    {
        _greenPowerUnitMaterial = _greenPowerUnit.GetComponent<MeshRenderer>();
        _redPowerUnitMaterial   = _redPowerUnit.GetComponent<MeshRenderer>();
        _bluePowerUnitMaterial  = _bluePowerUnit.GetComponent<MeshRenderer>();

        if (_greenCardSlot != null)
        {
            CoreCardInput script = _greenCardSlot.GetComponent<CoreCardInput>();

            if (script != null)
            {
                script.corePowerUpScript = this;
            }
        }

        if (_redCardSlot != null)
        {
            CoreCardInput script = _redCardSlot.GetComponent<CoreCardInput>();

            if (script != null)
            {
                script.corePowerUpScript = this;
            }
        }

        if (_blueCardSlot != null)
        {
            CoreCardInput script = _blueCardSlot.GetComponent<CoreCardInput>();

            if (script != null)
            {
                script.corePowerUpScript = this;
            }
        }

        if (_coreKlaxon != null)
        {
            KlaxonLightSpin script = _coreKlaxon.GetComponent<KlaxonLightSpin>();

            if (script != null)
            {
                script.corePowerUpScript = this;
            }
        }

        if (_securityRoomShutterTrigger != null)
        {
            ShutterTrigger script = _securityRoomShutterTrigger.GetComponent<ShutterTrigger>();

            if (script != null)
            {
                script.corePowerUpScript = this;
            }
        }

        _shutterStartPosition = _securityRoomShutters.transform.position;
        _shutterEndPosition = _shutterStartPosition + new Vector3(0, _shutterUpMoveDistance, 0);
    }

    // Update is called once per frame
    //void Update()
    //{
    //    //if(!_coreOnline && _greenCardOnline && _redCardOnline && _blueCardOnline)
    //    //{
    //    //    _coreOnline = true;
    //    //    _coreReactorEffect.SetActive(true);
    //    //}

    //    //if (Input.GetKeyDown(KeyCode.M))
    //    //{
    //    //    StartCoroutine(AnimateShutters());
    //    //}
    //}

    public void LeverPulled()
    {
        if (!_coreOnline && _greenCardOnline && _redCardOnline && _blueCardOnline)
        {
            _coreOnline = true;
            _powerUpSound.SetActive(true);
            StartCoroutine(ActivateReactorEffect());

            for(int i =0; i<_lightningEffects.Length; i++)
            {
                _lightningEffects[i].SetActive(true);
            }

            for(int i = 0; i<_securityDoors.Length; i++)
            {
                _securityDoors[i].isDoorLocked = false;
            }
        }
    }

    public void OnTriggerEvent(GameObject slot, Collider other)
    {
        if(slot.CompareTag("Blue Card Slot") && other.CompareTag("Blue Keycard"))
        {
            Destroy(other.gameObject);
            _staticBlueCard.SetActive(true);
            _cardInsertSoundA.SetActive(true);
            _bluePowerUnitMaterial.material = _bluePowerUnitOnMaterial;
            _blueCardOnline = true;
        }

        if(slot.CompareTag("Red Card Slot") && other.CompareTag("Red Keycard"))
        {
            Destroy(other.gameObject);
            _staticRedCard.SetActive(true);
            _cardInsertSoundB.SetActive(true);
            _redPowerUnitMaterial.material = _redPowerUnitOnMaterial;
            _redCardOnline = true;
        }

        if (slot.CompareTag("Green Card Slot") && other.CompareTag("Green Keycard"))
        {
            Destroy(other.gameObject);
            _staticGreenCard.SetActive(true);
            _cardInsertSoundC.SetActive(true);
            _greenPowerUnitMaterial.material = _greenPowerUnitOnMaterial;
            _greenCardOnline = true;
        }
    }

    public void ShutterRelease()
    {
        StartCoroutine(AnimateShutters());
    }

    IEnumerator AnimateShutters()
    {
        float time = 0.0f;

        while (time <= _shutterDuration)
        {
            float t = time / _shutterDuration;
            _securityRoomShutters.transform.position = Vector3.Lerp(_shutterStartPosition, _shutterEndPosition, JumpCurve.Evaluate(t));
            time += Time.deltaTime;
            yield return null;
        }
    }

    IEnumerator ActivateReactorEffect()
    {
        yield return new WaitForSeconds(4.0f);
        _coreReactorEffect.SetActive(true);
    }
}
