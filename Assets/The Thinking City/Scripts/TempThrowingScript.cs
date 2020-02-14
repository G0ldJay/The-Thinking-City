using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempThrowingScript : MonoBehaviour
{
    private Rigidbody rb = null;
    private float speed = 0;
    private bool _hitFloor = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        speed = rb.velocity.magnitude;

        if (speed > 1 && _hitFloor)
        {
            Debug.Log(speed);
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Floor"))
        {
            _hitFloor = true;
            Debug.Log(_hitFloor + " it hit the floor");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Floor") && _hitFloor)
        {
            _hitFloor = false;
            Debug.Log(_hitFloor + " not on floor");
        }
    }
}
