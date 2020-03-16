using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSlightRotation : MonoBehaviour
{
    public float _rotationSpeed = 0.1f;
    public bool _gravity = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!_gravity)
        {
            transform.Rotate(new Vector3(.5f, 1f, .3f) * (_rotationSpeed * Time.deltaTime));
        }
    }
}
