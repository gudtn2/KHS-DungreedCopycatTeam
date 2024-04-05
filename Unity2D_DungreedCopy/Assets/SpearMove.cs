using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpearMove : MonoBehaviour
{
    [SerializeField]
    private AnimationCurve  returnCurve;
    [SerializeField]
    private float           returnTime;

    private Vector2         start; 
    private Vector2         target;

    private void Start()
    {
        start   = new Vector2(transform.localPosition.x, transform.localPosition.y);
        target  = new Vector2(transform.localPosition.x + 0.3f, transform.localPosition.y);
    }

    public void AttackMove()
    {
        transform.localPosition = target;

        StartCoroutine(ReturnMove());
    }

    private IEnumerator ReturnMove()
    {
        float time = 0;

        while(time < returnTime)
        {
            time += Time.deltaTime;

            float t = time / returnTime;
            float curve = returnCurve.Evaluate(t);

            transform.localPosition = Vector2.Lerp(target, start, curve);

            yield return null;
        }
        transform.localPosition = start;
    }
}
