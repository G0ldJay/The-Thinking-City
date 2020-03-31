using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravGenerator : MonoBehaviour
{
    [SerializeField] private SphereCollider _generatorHitbox;
    [SerializeField] private GravityRoomScript _gravRoomScript;
    [SerializeField] private GameObject _gravityGeneratorEffect;
    private bool _eventTriggered = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Gen Smasher") && !_eventTriggered)
        {
            _eventTriggered = true;

            StartCoroutine(GeneratorEvent());
        }
    }

    private IEnumerator GeneratorEvent()
    {
        yield return new WaitForSeconds(1.0f);
        _gravityGeneratorEffect.SetActive(false);
        yield return new WaitForSeconds(0.1f);
        _gravityGeneratorEffect.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        _gravityGeneratorEffect.SetActive(false);
        yield return new WaitForSeconds(0.1f);
        _gravityGeneratorEffect.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        _gravityGeneratorEffect.SetActive(false);

        _gravRoomScript.gravity_on = true;
    }
}
