using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DoorState { Open, Animating, Closed};
public class DoorFunctionality : MonoBehaviour
{
    public Transform DoorTop;
    public Transform DoorBottom;

    private Transform mtransform = null;
    private Transform mtransform2 = null;

    private DoorState doorState = DoorState.Closed;
    private Vector3 openPos = Vector3.zero;
    private Vector3 closedPos = Vector3.zero;

    private Vector3 openPos2 = Vector3.zero;
    private Vector3 closedPos2 = Vector3.zero;

    private float timer = 0.0f;

    public float SlidingDistance =0.0f;
    public float Duration = 0.0f;
    public AnimationCurve JumpCurve = new AnimationCurve();

    // Start is called before the first frame update
    void Start()
    {
        mtransform = DoorTop;
        closedPos = DoorTop.position;
        openPos = closedPos + (mtransform.up * SlidingDistance);

        mtransform2 = DoorBottom;
        closedPos2 = DoorBottom.position;
        openPos2 = closedPos2 + (mtransform2.up *  SlidingDistance);
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
