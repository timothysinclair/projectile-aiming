using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AimHelper
{
    public static bool PredictPositionLinear(Vector3 _targetVelocity, Vector3 _targetPosition, Vector3 _projectilePosition, float _projectileSpeed, out Vector3 _finalTargetPosition)
    {
        Vector3 deltaPosition = _targetPosition - _projectilePosition;

        float a = Vector3.Dot(_targetVelocity, _targetVelocity) - _projectileSpeed * _projectileSpeed;
        float b = 2.0f * Vector3.Dot(deltaPosition, _targetVelocity);
        float c = Vector3.Dot(deltaPosition, _targetPosition - _projectilePosition);

        List<float> tValues;

        // Avoid divide by zero
        if (a == 0.0f)
        {
            tValues = new List<float>();
            tValues.Add(-c / b);
        }
        else
        {
            tValues = MathHelper.QuadraticFormula(a, b, c);
        }

        float finalT;

        // Choose lesser of two solutions
        if (tValues.Count == 2)
        {
            // Both positive
            if (tValues[0] >= 0.0f && tValues[1] >= 0.0f)
            {
                finalT = (tValues[0] < tValues[1]) ? tValues[0] : tValues[1];
            }
            // One positive
            else if (tValues[0] >= 0.0f)
            {
                finalT = tValues[0];
            }
            else if (tValues[1] >= 0.0f)
            {
                finalT = tValues[1];
            }
            else
            {
                finalT = 0.0f;
                Debug.LogError("Both t values were negative");
                _finalTargetPosition = _targetPosition;
                return false;
            }

            _finalTargetPosition = _targetPosition + _targetVelocity * finalT;
            return true;
        }
        else if (tValues.Count == 1)
        {
            finalT = tValues[0];
            _finalTargetPosition = _targetPosition + _targetVelocity * finalT;
            return true;
        }
        else
        {
            _finalTargetPosition = Vector3.zero;
            return false;
        }
    }

    public static bool VelocityToHitLinear(Vector3 _targetVelocity, Vector3 _targetPosition, Vector3 _projectilePosition, float _projectileSpeed, out Vector3 _aimVelocity)
    {
        Vector3 targetFinalPosition;
        if (AimHelper.PredictPositionLinear(_targetVelocity, _targetPosition, _projectilePosition, _projectileSpeed, out targetFinalPosition))
        {
            Vector3 directionToTarget = (targetFinalPosition - _projectilePosition).normalized;
            _aimVelocity = directionToTarget * _projectileSpeed;
            return true;
        }
        else
        {
            _aimVelocity = Vector3.zero;
            return false;
        }
    }

    public static bool PredictAngleBallistic(Vector3 _targetPosition, Vector3 _projectilePosition, float _projectileSpeed, float _gravity, out float _fireAngle)
    {
        Vector3 deltaPosition = _targetPosition - _projectilePosition;

        float deltaY = deltaPosition.y;
        deltaPosition.y = 0.0f;
        float deltaHor = deltaPosition.magnitude;

        float a = -_gravity * deltaHor * deltaHor / (2.0f * _projectileSpeed * _projectileSpeed);
        float b = deltaHor;
        float c = a - deltaY;

        List<float> angleValues;

        // Avoid divide by zero
        if (a == 0.0f)
        {
            angleValues = new List<float>();
            angleValues.Add(-c / b);
        }
        else
        {
            angleValues = MathHelper.QuadraticFormula(a, b, c);
        }

        // Since x = tan(theta), we must atan the results to find theta
        for (int i = 0; i < angleValues.Count; i++)
        {
            angleValues[i] = Mathf.Atan(angleValues[i]);
        }
        
        if (angleValues.Count == 2)
        {
            float absAngleOne = Mathf.Abs(angleValues[0]);
            float absAngleTwo = Mathf.Abs(angleValues[1]);

            _fireAngle = (absAngleOne < absAngleTwo) ? angleValues[0] : angleValues[1];
            return true;
        }
        else if (angleValues.Count == 1)
        {
            _fireAngle = angleValues[0];
            return true;
        }
        else
        {
            _fireAngle = 0.0f;
            return false;
        }
    }

    public static bool VelocityToHitBallistic(Vector3 _targetVelocity, Vector3 _targetPosition, Vector3 _projectilePosition, float _projectileSpeed, float _gravity, out Vector3 _aimVelocity)
    {
        Vector3 linearVelocity;
        float aimAngle;

        if (VelocityToHitLinear(_targetVelocity, _targetPosition, _projectilePosition, _projectileSpeed, out linearVelocity))
        {
            if (PredictAngleBallistic(_targetPosition, _projectilePosition, _projectileSpeed, _gravity, out aimAngle))
            {
                float xSpeed = _projectileSpeed * Mathf.Cos(aimAngle);
                float ySpeed = _projectileSpeed * Mathf.Sin(aimAngle);

                linearVelocity.y = 0.0f;
                linearVelocity = linearVelocity.normalized * xSpeed;
                _aimVelocity = linearVelocity + Vector3.up * ySpeed;

                return true;
            }
            else
            {
                _aimVelocity = Vector3.zero;
                return false;
            }
        }
        else
        {
            _aimVelocity = Vector3.zero;
            return false;
        }
    }
}
