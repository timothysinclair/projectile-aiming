using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MathHelper
{
    public static List<float> QuadraticFormula(float _a, float _b, float _c)
    {
        List<float> xValues = new List<float>();

        float disc = _b * _b - (4.0f * _a * _c);
        float sqrtDisc = Mathf.Sqrt(disc);

        // Two solutions
        if (disc > 0.001f)
        {
            float valueOne = (-_b + sqrtDisc) / (2.0f * _a);
            float valueTwo = (-_b - sqrtDisc) / (2.0f * _a);

            xValues.Add(valueOne);
            xValues.Add(valueTwo);
        }
        // One solution
        else if (disc <= 0.001f && disc >= 0.0f)
        {
            float valueOne = (-_b + sqrtDisc) / (2.0f * _a);

            xValues.Add(valueOne);
        }
        // Else: No solutions

        return xValues;
    }
}
