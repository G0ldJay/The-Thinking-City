using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KlaxonPodLightSpin : MonoBehaviour
{
    [SerializeField] private GameObject _klaxonLightOne = null;
    [SerializeField] private GameObject _klaxonLightTwo = null;
    [SerializeField] private GameObject _klaxonSound = null;

    [SerializeField] private float _speed = 0.0f;

    private RobotPod    _robotPodScript         = null;
    private IEnumerator _orbitKlaxonCoroutine   = null;

    public RobotPod robotPodScript { set { _robotPodScript = value; } }

    // Update is called once per frame
    void Update()
    {
        if (_robotPodScript != null)
        {
            if (_robotPodScript.triggerKlaxon)
            {
                if (_orbitKlaxonCoroutine == null)
                {
                    _orbitKlaxonCoroutine = OrbitKlaxon();
                    _klaxonLightOne.SetActive(true);
                    _klaxonLightTwo.SetActive(true);

                    if(_klaxonSound!=null)
                        _klaxonSound.SetActive(true);

                    StartCoroutine(_orbitKlaxonCoroutine);
                }
            }
        }

    }

    public void StartOrbit()
    {
        _klaxonLightOne.SetActive(true);
        _klaxonLightTwo.SetActive(true);
        _klaxonSound.SetActive(true);
        StartCoroutine(OrbitKlaxon());
    }

    private IEnumerator OrbitKlaxon()
    {
        while (_robotPodScript.triggerKlaxon)
        {
            _klaxonLightOne.transform.RotateAround(gameObject.transform.position, Vector3.forward, _speed * Time.deltaTime);
            _klaxonLightTwo.transform.RotateAround(gameObject.transform.position, Vector3.forward, _speed * Time.deltaTime);
            yield return null;
        }

        _klaxonLightOne.gameObject.SetActive(false);
        _klaxonLightTwo.gameObject.SetActive(false);
    }
}
