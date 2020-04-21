﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityRoomScript : MonoBehaviour
{
    public GameObject[] grav;
    public bool gravity_on = false;
    private bool _trishAudioPlayed = false;

    [SerializeField] private GameObject _trishGravityGeneratorLine = null;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
        {
            if (other.GetComponent<Rigidbody>() != null)
            {
                other.GetComponent<Rigidbody>().useGravity = false;
            }
        }

        if (other.CompareTag("Player") && !_trishAudioPlayed && !gravity_on)
        {
            _trishAudioPlayed = true;
            _trishGravityGeneratorLine.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player"))
        {
            if (other.GetComponent<Rigidbody>()!=null)
            {
                other.GetComponent<Rigidbody>().useGravity = true;
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (gravity_on)
        {
            if (!other.CompareTag("Player"))
            {
                if (other.GetComponent<Rigidbody>().useGravity == false)
                {
                    ObjectSlightRotation script = other.GetComponentInParent<ObjectSlightRotation>();

                    if (script != null)
                    {
                        script._gravity = true;
                    }

                    if (other.GetComponent<Rigidbody>() != null)
                    {
                        other.GetComponent<Rigidbody>().useGravity = true;
                    }
                } 
            }
                
            //for (int i = 0; i < grav.Length; i++)
            //{
            //    grav[i].GetComponent<Rigidbody>().useGravity = true;
            //}

            //gameObject.GetComponent<BoxCollider>().enabled = false;
        }
    }
}
