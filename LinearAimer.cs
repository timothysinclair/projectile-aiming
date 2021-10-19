using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinearAimer : MonoBehaviour
{
    public IAimTarget m_target;
    public GameObject m_projectilePrefab;
    public float m_projectileSpeed;
    public float m_fireDelay;
    public Vector2 m_aimVariation; // +- (x,y) degrees on either axis

    private void OnEnable()
    {
        StartCoroutine(FireOnDelay());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    private IEnumerator FireOnDelay()
    {
        yield return new WaitForSeconds(0.1f);
        FireAtTarget();

        yield return new WaitForSeconds(m_fireDelay);

        StartCoroutine(FireOnDelay());
    }

    private void FireAtTarget()
    {
        Vector3 aimVelocity;

        // If possible to hit
        if (AimHelper.VelocityToHitLinear(m_target.GetVelocity(), m_target.GetPosition(), transform.position, m_projectileSpeed, out aimVelocity))
        {
            // Add slight variation to aim
            aimVelocity = VaryAim(aimVelocity);

            // Create bullet
            GameObject newProjectile = Instantiate(m_projectilePrefab, transform.position, transform.rotation, null);
            newProjectile.transform.LookAt(transform.position + aimVelocity);
            newProjectile.GetComponent<Rigidbody>().velocity = aimVelocity;

            // Destroy bullet after 30 seconds
            Destroy(newProjectile, 30.0f);
        }
    }

    private Vector3 VaryAim(Vector3 _aimVector)
    {
        float xVar = Random.Range(-m_aimVariation.y, m_aimVariation.y);
        float yVar = Random.Range(-m_aimVariation.x, m_aimVariation.x);

        Quaternion rotator = Quaternion.Euler(xVar, yVar, 0.0f);
        return (rotator * _aimVector);
    }
}