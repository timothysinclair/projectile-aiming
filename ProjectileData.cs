using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ProjectileData", menuName = "ScriptableObjects/Projectile Data", order = 2)]
public class ProjectileData : ScriptableObject
{
    public string m_impactSound;
    public float m_damage;
    public GameObject m_impactSpawnPrefab;
}
