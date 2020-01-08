using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public List<AudioSource> AIAudio = new List<AudioSource>(); //REMOVE at a later date

    private Dictionary<int, AIStateMachine> _stateMachines = new Dictionary<int, AIStateMachine>();
    
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

    public void PlayAISound(int audio)
    {
        AIAudio[audio].Play();
    }
}
