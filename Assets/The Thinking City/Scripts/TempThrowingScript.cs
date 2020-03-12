using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempThrowingScript : MonoBehaviour
{
    //private Rigidbody rb = null;
    //private float speed = 0;
    //private bool _hitFloor = false;
    //private string _objectTag = "";
    //[SerializeField] private string _soundTag = "AI Sound Emitter";
    //[SerializeField] private int _soundAgroLayer = 14;
    //private int _startLayer = 0;

    [SerializeField] private GameObject _soundObject = null;


    // Start is called before the first frame update
    void Start()
    {
        //rb = GetComponent<Rigidbody>();
        _soundObject.SetActive(false);
    }

    // Update is called once per frame
    //void Update()
    //{
    //    speed = rb.velocity.magnitude;

    //    if (speed > 1 && _hitFloor)
    //    {
    //        Debug.Log(speed);
    //    }
    //}

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Floor"))
        {
            SetLayerAndTag();
        }
    }

    IEnumerator SetLayerAndTag()
    {
        _soundObject.SetActive(true);

        yield return new WaitForSeconds(.5f);

        _soundObject.SetActive(false);
    }

    //private void OnTriggerExit(Collider other)
    //{
    //    if(other.CompareTag("Floor") && _hitFloor)
    //    {
    //        _hitFloor = false;
    //        Debug.Log(_hitFloor + " not on floor");
    //    }
    //}
}
