using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterLevelIndicator : MonoBehaviour
{
    private float lerpFactor;
    private Transform activeTarget;
    [SerializeField] private Transform t1, t2;
    private void Start()
    {
        activeTarget = t1;
    }
    private void OnMouseDown()
    {

        StartCoroutine(StartMove(activeTarget));
        activeTarget = (t1 == activeTarget) ? t2 : t1;
        
    }

    public IEnumerator StartMove(Transform target)
    {

        lerpFactor = 0;
        

        Vector2 tVec = target.position;
        Vector2 originalPos = transform.position;


        while ((target.position - transform.position).magnitude > 0.5f)
        {
            yield return null;
            Move(tVec, originalPos);
        }
        
    }

    private void Move(Vector2 tVec, Vector2 originalVec)
    {
        
        lerpFactor += Time.deltaTime*(0.01f+Val(lerpFactor));
        
        Vector2 moveVec = new Vector2( Mathf.Lerp(originalVec.x, tVec.x, lerpFactor), Mathf.Lerp(originalVec.y, tVec.y, lerpFactor));
        transform.position = moveVec;

    }
    private float Val(float x)
    {
        
        float a = 0.5f;
        float b = 0.2f;
        float c = 0.5f;
        return Mathf.Pow(2.7f, ((-((x - a) * (x - a))) / (2 * b * b)) / (b * Mathf.Sqrt(2 * Mathf.PI)) * c);
    }
}
