using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallisticAimer : MonoBehaviour
{
    public IAimTarget m_target;
    public GameObject m_projectilePrefab;
    public float m_projectileSpeed;
    public float m_fireDelay;
    public Vector2 m_aimVariation; // +- (x,y) degrees on either axis
    public float m_gravitySpeed;

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

        if (AimHelper.VelocityToHitBallistic(m_target.GetVelocity(), m_target.GetPosition(), transform.position, m_projectileSpeed, m_gravitySpeed, out aimVelocity))
        {
            GameObject newProjectile = Instantiate(m_projectilePrefab, transform.position, transform.rotation, null);
            newProjectile.transform.LookAt(transform.position + aimVelocity);
            newProjectile.GetComponent<Rigidbody>().velocity = aimVelocity;
            Destroy(newProjectile, 30.0f);
        }
    }


}
