using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempAI : MonoBehaviour
{
    [SerializeField] private GameObject _ai = null;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.Four))
        {
            _ai.SetActive(true);
        }
    }
}
