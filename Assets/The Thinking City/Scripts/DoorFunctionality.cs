using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public enum DoorState { Open, Animating, Closed};

public class DoorFunctionality : MonoBehaviour
{
    public Transform DoorTop;
    public Transform DoorBottom;

    private InterfaceAnimManager    current;
    private int                     indexInterface    = 0;
    private Transform               startTransform    = null;
    private Transform               startTransform2   = null;
    private Transform               mtransform        = null;
    private Transform               mtransform2       = null;

    //[SerializeField] private GameObject                 _hologramLocked     = null;
    //[SerializeField] private GameObject                 _hologramUnlocked   = null;
    [SerializeField] private Material                   _doorLightClosed    = null;
    [SerializeField] private Material                   _doorLightOpen      = null;
    [SerializeField] private GameObject                 _doorlight          = null;
    [SerializeField] private InterfaceAnimManager[]     holoInterfaceList;

    private DoorState   doorState   = DoorState.Closed;
    private Vector3     openPos     = Vector3.zero;
    private Vector3     closedPos   = Vector3.zero;

    private Vector3 openPos2    = Vector3.zero;
    private Vector3 closedPos2  = Vector3.zero;

    private float timer = 0.0f;

    public float            SlidingDistance =0.0f;
    public float            Duration        = 0.0f;
    public AnimationCurve   JumpCurve       = new AnimationCurve();

    private AudioSource audio = null;

    // Start is called before the first frame update
    void Start()
    {
        startTransform  = DoorTop;
        startTransform2 = DoorBottom;

        audio = GetComponent<AudioSource>();

        mtransform  = DoorTop;
        closedPos   = DoorTop.position;
        openPos     = closedPos + (mtransform.up * SlidingDistance);

        mtransform2 = DoorBottom;
        closedPos2  = DoorBottom.position;
        openPos2    = closedPos2 + (mtransform2.up *  SlidingDistance);

        //_hologramLocked.SetActive(true);
        //_hologramUnlocked.SetActive(false);

        if (holoInterfaceList[indexInterface] != null)
        {
            CallInterface(holoInterfaceList[indexInterface]);
        }   
    }

    private void CallInterface(InterfaceAnimManager _interface)
    {
        if (current)
            current.startDisappear(true);

        current = _interface;
        if (_interface)
        {
            current.gameObject.SetActive(true);
            current.startAppear();
        }
    }

    public void DoNext()
    {
        indexInterface++;
        if (indexInterface >= holoInterfaceList.Length)
            indexInterface = 0;

        CallInterface(holoInterfaceList[indexInterface]);
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (timer > 10)
        {
            int rand = Random.Range(0, 101);

            if(rand>45)
                StartCoroutine(AnimateDoor((doorState == DoorState.Open) ? DoorState.Closed : DoorState.Open));

            timer = 0;
        }
            
    }

    IEnumerator AnimateDoor(DoorState currentState)
    {
        if(currentState == DoorState.Closed)
        {
            DoNext();
            _doorlight.GetComponent<MeshRenderer>().material = _doorLightClosed;
        }
        else
        {
            DoNext();
            yield return new WaitForSeconds(1.0f);
            _doorlight.GetComponent<MeshRenderer>().material = _doorLightOpen;
            yield return new WaitForSeconds(0.1f);
            _doorlight.GetComponent<MeshRenderer>().material = _doorLightClosed;
            yield return new WaitForSeconds(0.1f);
            _doorlight.GetComponent<MeshRenderer>().material = _doorLightOpen;
            yield return new WaitForSeconds(0.1f);
            _doorlight.GetComponent<MeshRenderer>().material = _doorLightClosed;
            yield return new WaitForSeconds(0.1f);
            _doorlight.GetComponent<MeshRenderer>().material = _doorLightOpen;
        }

        //audio.Play();
        FMODUnity.RuntimeManager.PlayOneShot("event:/DoorActivate", GetComponent<Transform>().position);

        doorState = DoorState.Animating;
        float time = 0.0f;

        Vector3 startPos = (currentState == DoorState.Open) ? closedPos : openPos;
        Vector3 endPos = (currentState == DoorState.Open) ? openPos : closedPos;

        Vector3 startPos2 = (currentState == DoorState.Open) ? closedPos2 : openPos2;
        Vector3 endPos2 = (currentState == DoorState.Open) ? openPos2 : closedPos2;

        while (time <= Duration)
        {
            float t = time / Duration;
            mtransform.position = Vector3.Lerp(startPos, endPos, JumpCurve.Evaluate(t));
            mtransform2.position = Vector3.Lerp(startPos2, endPos2, JumpCurve.Evaluate(t));
            time += Time.deltaTime;
            yield return null;
        }

        mtransform.position = endPos;
        mtransform2.position = endPos2;
        doorState = currentState;
    }
}
