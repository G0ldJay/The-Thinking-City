using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
