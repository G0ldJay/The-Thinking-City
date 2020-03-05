using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionMilestoneTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (GameManager.instance != null && other.CompareTag("Player"))
        {
            GameManager.instance.NextObjective();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (GameManager.instance != null && other.CompareTag("Player"))
        {
            GameManager.instance.NextObjective();
        }
    }
}
