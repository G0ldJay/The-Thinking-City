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

    [SerializeField] private BoxCollider _greenCardSlot    = null;
    [SerializeField] private BoxCollider _redCardSlot      = null;
    [SerializeField] private BoxCollider _blueCardSlot     = null;

    [SerializeField] private GameObject _coreReactorEffect = null;
    [SerializeField] private GameObject _coreKlaxon = null;

    private MeshRenderer _greenPowerUnitMaterial    = null;
    private MeshRenderer _redPowerUnitMaterial      = null;
    private MeshRenderer _bluePowerUnitMaterial     = null;

    private bool _greenCardOnline   = false;
    private bool _redCardOnline     = false;
    private bool _blueCardOnline    = false;
    private bool _coreOnline        = false;

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
    }

    // Update is called once per frame
    void Update()
    {
        if(!_coreOnline && _greenCardOnline && _redCardOnline && _blueCardOnline)
        {
            _coreOnline = true;
            _coreReactorEffect.SetActive(true);
        }
    }

    public void OnTriggerEvent(GameObject slot, Collider other)
    {
        if(slot.CompareTag("Blue Card Slot") && other.CompareTag("Blue Keycard"))
        {
            Destroy(other.gameObject);
            _staticBlueCard.SetActive(true);
            _bluePowerUnitMaterial.material = _bluePowerUnitOnMaterial;
            _blueCardOnline = true;
        }

        if(slot.CompareTag("Red Card Slot") && other.CompareTag("Red Keycard"))
        {
            Destroy(other.gameObject);
            _staticRedCard.SetActive(true);
            _redPowerUnitMaterial.material = _redPowerUnitOnMaterial;
            _redCardOnline = true;
        }

        if (slot.CompareTag("Green Card Slot") && other.CompareTag("Green Keycard"))
        {
            Destroy(other.gameObject);
            _staticGreenCard.SetActive(true);
            _greenPowerUnitMaterial.material = _greenPowerUnitOnMaterial;
            _greenCardOnline = true;
        }
    }
}
