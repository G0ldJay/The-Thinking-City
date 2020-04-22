using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UiObjectiveList : MonoBehaviour
{
    //[SerializeField] private GameObject _objectiveArrow = null;
    [SerializeField] private TextMeshProUGUI _objectiveList = null;
    [SerializeField] private TextMeshProUGUI _objectiveName = null;
    [SerializeField] private GameObject _powerGeneratorHologram = null;
    [SerializeField] private GameObject _keycardHologram = null;
    [SerializeField] private GameObject _lever = null;
    [SerializeField] private GameObject _exitBulkheads = null;

    [SerializeField] private GameObject _powerGenerator = null;
    [SerializeField] private GameObject _yellowKeycard = null;
    [SerializeField] private GameObject _redKeycard = null;
    [SerializeField] private GameObject _blueKeycard = null;
    [SerializeField] private GameObject _powerGeneratorLever = null;
    [SerializeField] private GameObject _securityRoomLever = null;
    [SerializeField] private GameObject _exitTransform = null;

    private bool _foundPowerGenerator = false;
    private bool _foundYellowCard = false;
    private bool _foundRedCard = false;
    private bool _foundBlueCard = false;
    private bool _triggeredPower = false;
    private bool _liftedLockdown = false;

    private static UiObjectiveList _instance = null;

    public bool foundPowerGenerator { get { return _foundPowerGenerator; } set { _foundPowerGenerator = value; } }
    public bool foundYellowCard { get { return _foundYellowCard; } set { _foundYellowCard = value; } }
    public bool foundRedCard { get { return _foundRedCard; } set { _foundRedCard = value; } }
    public bool foundBlueCard { get { return _foundBlueCard; } set { _foundBlueCard = value; } }
    public bool triggeredPower { get { return _triggeredPower; } set { _triggeredPower = value; } }
    public bool liftedLockdown { get { return _liftedLockdown; } set { _liftedLockdown = value; } }
    public static UiObjectiveList instance
    {
        get
        {
            if (_instance == null)
                _instance = (UiObjectiveList)FindObjectOfType(typeof(UiObjectiveList));

            return _instance;
        }
    }

    public GameObject GetCurrentObjective()
    {
        if (!_foundPowerGenerator)
        {
            _objectiveList.SetText("Find the power generator");
            _objectiveName.SetText("Power Generator");

            if(!_powerGeneratorHologram.activeInHierarchy)
                _powerGeneratorHologram.SetActive(true);

            return _powerGenerator;
        }
        else if (!_foundYellowCard)
        {
            _objectiveList.SetText("Find the yellow keycard");
            _objectiveName.SetText("Keycard");
            _powerGeneratorHologram.SetActive(false);
            _keycardHologram.SetActive(true);

            return _yellowKeycard;
        }
        else if (!_foundRedCard)
        {
            _objectiveList.SetText("Find the red keycard");
            _objectiveName.SetText("Keycard");
            _powerGeneratorHologram.SetActive(false);
            _keycardHologram.SetActive(true);

            return _redKeycard;
        }
        else if (!_foundBlueCard)
        {
            _objectiveList.SetText("Find the blue keycard");
            _objectiveName.SetText("Keycard");
            _powerGeneratorHologram.SetActive(false);
            _keycardHologram.SetActive(true);

            return _blueKeycard;
        }
        else if (!_triggeredPower)
        {
            _objectiveList.SetText("Restore the power");
            _objectiveName.SetText("Power Lever");
            _powerGeneratorHologram.SetActive(false);
            _keycardHologram.SetActive(false);
            _lever.SetActive(true);

            return _powerGeneratorLever;
        }
        else if (!_liftedLockdown)
        {
            _objectiveList.SetText("Lift the lockdown");
            _objectiveName.SetText("Security Lever");
            _powerGeneratorHologram.SetActive(false);
            _keycardHologram.SetActive(false);
            _lever.SetActive(true);

            return _securityRoomLever;
        }
        else
        {
            _objectiveList.SetText("Escape!");
            _objectiveName.SetText("Exit Doors");
            _powerGeneratorHologram.SetActive(false);
            _keycardHologram.SetActive(false);
            _lever.SetActive(false);
            _exitBulkheads.SetActive(true);

            return _exitTransform;
        }
    }
}
