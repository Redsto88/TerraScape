using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CurveBrush : Brush
{
    [SerializeField] AnimationCurve curve;

    public override float Sample(float x, float y)
    {
        float curveUV = 1f - new Vector2(Mathf.Abs(x - 0.5f) * 2f, Mathf.Abs(y - 0.5f) * 2f).magnitude;
        return curve.Evaluate(curveUV);
    }
}
