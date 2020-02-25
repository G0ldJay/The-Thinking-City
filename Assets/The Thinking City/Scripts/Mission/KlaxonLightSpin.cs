using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KlaxonLightSpin : MonoBehaviour
{
    [SerializeField] private GameObject _klaxonLightOne;
    [SerializeField] private GameObject _klaxonLightTwo;

    [SerializeField] private float _speed = 0.0f;

    private CorePowerUpQuest _corePowerUpScript = null;
    private IEnumerator _orbitKlaxonCoroutine = null;

    public CorePowerUpQuest corePowerUpScript { set { _corePowerUpScript = value; } }

    // Update is called once per frame
    void Update()
    {
        if (_corePowerUpScript != null)
        {
            if (_corePowerUpScript.coreOnline)
            {
                if (_orbitKlaxonCoroutine == null)
                {
                    _orbitKlaxonCoroutine = OrbitKlaxon();
                    _klaxonLightOne.SetActive(true);
                    _klaxonLightTwo.SetActive(true);
                    StartCoroutine(_orbitKlaxonCoroutine);
                }    
            }
        }

    }

    private IEnumerator OrbitKlaxon()
    {
        while (true)
        {
            _klaxonLightOne.transform.RotateAround(gameObject.transform.position, Vector3.forward, _speed * Time.deltaTime);
            _klaxonLightTwo.transform.RotateAround(gameObject.transform.position, Vector3.forward, _speed * Time.deltaTime);
            yield return null;
        }
    }
}
