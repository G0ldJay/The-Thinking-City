using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KlaxonExit : MonoBehaviour
{
    [SerializeField] private GameObject _klaxonLightOne;
    [SerializeField] private GameObject _klaxonLightTwo;
    [SerializeField] private GameObject _klaxonSoundEffect;

    [SerializeField] private float _speed = 0.0f;

    private CorePowerUpQuest _corePowerUpScript = null;
    private IEnumerator _orbitKlaxonCoroutine = null;

    public CorePowerUpQuest corePowerUpScript { set { _corePowerUpScript = value; } }

    public void ActivateKlaxons()
    {
        _klaxonLightOne.SetActive(true);
        _klaxonLightTwo.SetActive(true);

        if(_klaxonSoundEffect!=null)
            _klaxonSoundEffect.SetActive(true);

        StartCoroutine(OrbitKlaxon());
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


