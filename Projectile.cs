using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour
{
    public bool m_ballistic;
    public ProjectileData m_data;
    private Rigidbody m_rigidBody;

    private void Awake()
    {
        m_rigidBody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (!m_ballistic)
        {
            return;
        }

        if (m_rigidBody.velocity.magnitude > 0.001f)
        {
            Vector3 inFront = transform.position + m_rigidBody.velocity.normalized;
            transform.LookAt(inFront);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (m_data.m_impactSound != null && m_data.m_impactSound != "")
        {
            AudioManager.Instance.PlaySound(m_data.m_impactSound);
        }

        HealthComponent healthComp = collision.gameObject.GetComponent<HealthComponent>();

        if (healthComp)
        {
            healthComp.Health -= m_data.m_damage;
        }

        if (m_data.m_impactSpawnPrefab)
        {
            Instantiate(m_data.m_impactSpawnPrefab, collision.GetContact(0).point, Quaternion.identity, null);
            
        }

        Destroy(this.gameObject);
    }
}
