using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotPod : MonoBehaviour
{
    [SerializeField] private GameObject             _podDoorKlaxon          = null;
    [SerializeField] private GameObject             _podDoor                = null;
    [SerializeField] private BoxCollider            _podDoorTrigger         = null;
    [SerializeField] private float                  _podDoorMoveDistance    = 1.5f;
    [SerializeField] private float                  _podDoorDuration        = 2.0f;
    [SerializeField] public FMODUnity.StudioEventEmitter doorOpeningEmitter;

    private bool    _triggerKlaxon      = false;
    private Vector3 _podDoorStartPos    = Vector3.zero;
    private Vector3 _podDoorEndPos      = Vector3.zero;


    public AnimationCurve PodDoorAnimationCurve = new AnimationCurve();

    public bool triggerKlaxon { get { return _triggerKlaxon; } }

    // Start is called before the first frame update
    void Start()
    {
        if(_podDoorKlaxon!= null)
        {
            KlaxonPodLightSpin script = _podDoorKlaxon.GetComponent<KlaxonPodLightSpin>();

            if (script != null)
            {
                script.robotPodScript = this;
            }
        }

        if(_podDoorTrigger != null)
        {
            RobotPodDoorTrigger script = _podDoorTrigger.GetComponent<RobotPodDoorTrigger>();

            if(script != null)
            {
                script.robotPodScript = this;
            }
        }

        _podDoorStartPos    = _podDoor.transform.position;
        _podDoorEndPos      = _podDoorStartPos - new Vector3(0, _podDoorMoveDistance, 0);
    }

    public void OpenRobotPodDoor()
    {

        StartCoroutine(AnimateRobotPodDoor());
        doorOpeningEmitter.Play();
    }

    IEnumerator AnimateRobotPodDoor()
    {
        _triggerKlaxon = true;

        yield return new WaitForSeconds(1.0f);

        float time = 0.0f;

        while (time <= _podDoorDuration)
        {
            float t = time / _podDoorDuration;
            _podDoor.transform.position = Vector3.Lerp(_podDoorStartPos, _podDoorEndPos, PodDoorAnimationCurve.Evaluate(t));
            time += Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(3.0f);

        _triggerKlaxon = false;
    }
}
