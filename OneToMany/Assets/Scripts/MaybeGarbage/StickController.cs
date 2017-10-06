using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickController : MonoBehaviour {

    public Transform target;
    public bool moveToTarget;
    public bool loop;
    public float speed = 1f;
    public float pauseTime = 1f;

    Vector3 originalPos;
    Vector3 nextTarget;
    int counter = 0;

	void Awake ()
    {
        originalPos = transform.position;
	}

    void OnEnable()
    {
        transform.position = originalPos;
        nextTarget = target.position;
        if (moveToTarget)
        {
            StartCoroutine(MoveToTarget(nextTarget));
        }
    }

    void OnDisable()
    {
        StopAllCoroutines();
    }

    IEnumerator MoveToTarget(Vector3 targetPos)
    {
        var start = transform.position;
        var dist = Vector3.Distance(start, targetPos);
        float step = 1f;

        if (dist > 0f)
        {
            step = speed * Time.fixedDeltaTime / dist;
        }
        else
        {
            yield return null;
        }
        
        var t = 0f;
        while (t < 1f)
        {
            t += step;
            var newPos = Vector3.Lerp(start, targetPos, t);
            transform.position = newPos;
            yield return new WaitForFixedUpdate();
        }
        transform.position = targetPos;

        if (loop && moveToTarget)
        {
            if (pauseTime > 0f)
            {
                yield return new WaitForSeconds(pauseTime);
            }
            nextTarget = (++counter % 2) == 0 ? target.position : originalPos;
            StartCoroutine(MoveToTarget(nextTarget));
        }
    }
}
