using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(CharacterController))]
public class AimTarget : MonoBehaviour, IAimTarget
{
    public int m_velocityHistoryFrames = 1;
    public AnimationCurve m_velocityHistoryWeighting;
    public Vector3 m_aimPosOffset;

    private CharacterController m_controller;
    private List<Vector3> m_velocityHistory = new List<Vector3>();
    private Collider m_collider;

    private void Awake()
    {
        m_controller = GetComponent<CharacterController>();
        m_collider = GetComponent<Collider>();
    }

    private void FixedUpdate()
    {
        m_velocityHistory.Add(m_controller.velocity);

        if (m_velocityHistory.Count > m_velocityHistoryFrames)
        {
            m_velocityHistory.RemoveAt(0);
        }
    }

    public Vector3 GetVelocity()
    {
        Vector3 totalVelocity = Vector3.zero;
        Vector3 weightedTotal = Vector3.zero;
        float totalWeighting = 0.0f;

        // Return a weighted velocity, based on velocity in recent frames, and the history weighting curve
        for (int i = 0; i < m_velocityHistory.Count; i++)
        {
            totalVelocity += m_velocityHistory[i];
            float weight = m_velocityHistoryWeighting.Evaluate(i + 1 / m_velocityHistory.Count);
            weightedTotal += m_velocityHistory[i] * weight;
            totalWeighting += weight;
        }

        totalVelocity /= (float)m_velocityHistory.Count;
        weightedTotal /= totalWeighting;

        return weightedTotal;
    }

    public Vector3 GetPosition()
    {
        // Return center of bounding box
        return m_collider.bounds.center;
    }
}
