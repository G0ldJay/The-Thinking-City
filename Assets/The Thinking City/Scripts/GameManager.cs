using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerInfo
{
    public Collider collider = null;
    public CharacterManager characterManager = null;
    public Camera camera = null;
    public CapsuleCollider meleeTrigger = null;
}

// -------------------------------------------------------------------------
// CLASS	:	GameSceneManager
// Desc		:	Singleton class that acts as the scene database
// -------------------------------------------------------------------------
public class GameManager : MonoBehaviour
{
    [SerializeField] private ParticleSystem _bloodParticles = null;
    [SerializeField] private TextMeshProUGUI _objectiveDisplay = null;
    [SerializeField] private BoxCollider[] _missionMilestoneColliders;

    [SerializeField] private GameObject[] _keycards = null;

    private Dictionary<int, string> _objectives;
    private int _currentObjective = 0;

    private static GameManager _instance = null;
    public static GameManager instance
    {
        get
        {
            if (_instance == null)
                _instance = (GameManager)FindObjectOfType(typeof(GameManager));

            return _instance;
        }
    }

    // Private
    //public List<AudioSource> AIAudio = new List<AudioSource>(); //REMOVE at a later date

    private Dictionary<int, AIStateMachine> _stateMachines = new Dictionary<int, AIStateMachine>();
    private Dictionary<int, PlayerInfo> _playerInfos = new Dictionary<int, PlayerInfo>();

    public ParticleSystem bloodParticles { get { return _bloodParticles; } }

    private void Start()
    {
        _objectives = new Dictionary<int, string>();
        _objectives.Add(0, "Turn on the power");
        _objectives.Add(1, "Search the labs for keycards");
        _objectives.Add(2, "Pull power lever");
        _objectives.Add(3, "Lift lockdown from security");
        _objectives.Add(4, "Escape!");
    }
    public void RegisterAIStateMachine(int key, AIStateMachine stateMachine)
    {
        if (!_stateMachines.ContainsKey(key))
            _stateMachines[key] = stateMachine;
    }

    public AIStateMachine GetAIStateMachine(int key)
    {
        AIStateMachine machine = null;

        if (_stateMachines.TryGetValue(key, out machine))
            return machine;

        return null;
    }

    private void DisplayObjective(string text)
    {
        
        _objectiveDisplay.text = text;
        ++_currentObjective;
    }

    private void DisableMilestoneCollider(int _currentMilestone)
    {
        _missionMilestoneColliders[_currentMilestone].enabled = false;
    }

    private void EnableNextMilestoneCollider(int _enableMilestoneCollider)
    {
        _missionMilestoneColliders[_enableMilestoneCollider].enabled = true;
    }

    public void NextObjective()
    {
        if(_currentObjective!=1 && _currentObjective != 2 && _currentObjective != 3)
        {
            DisableMilestoneCollider(_currentObjective);
            DisplayObjective(_objectives[_currentObjective]);
            EnableNextMilestoneCollider(_currentObjective);
        }
        else if(_currentObjective == 1 && _keycards[0].gameObject.activeSelf && _keycards[1].gameObject.activeSelf && _keycards[2].gameObject.activeSelf)
        {
            DisableMilestoneCollider(_currentObjective);
            DisplayObjective(_objectives[_currentObjective]);
            EnableNextMilestoneCollider(_currentObjective);
        }
        
    }

    public GameObject GetCurrentObjective()
    {
        return _missionMilestoneColliders[_currentObjective].gameObject;
    }

    // --------------------------------------------------------------------
    // Name	:	RegisterPlayerInfo
    // Desc	:	Stores the passed PlayerInfo in the dictionary with
    //			the supplied key
    // --------------------------------------------------------------------
    public void RegisterPlayerInfo(int key, PlayerInfo playerInfo)
    {
        if (!_playerInfos.ContainsKey(key))
        {
            _playerInfos[key] = playerInfo;
        }
    }

    // --------------------------------------------------------------------
    // Name	:	GetPlayerInfo
    // Desc	:	Returns a PlayerInfo reference searched on by the
    //			instance ID of an object
    // --------------------------------------------------------------------
    public PlayerInfo GetPlayerInfo(int key)
    {
        PlayerInfo info = null;
        if (_playerInfos.TryGetValue(key, out info))
        {
            return info;
        }

        return null;
    }

    //public void PlayAISound(int audio)
    //{
    //    AIAudio[audio].Play();
    //}
}
