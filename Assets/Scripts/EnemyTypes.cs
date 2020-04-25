using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy Type",menuName ="EnemyType")]
public class EnemyTypes : ScriptableObject
{

    public string enemyType;
    public int damage;
    public int health;
    public float enemySpeed;
    public float projectileSpeed;
    public float shootingFrequency;
    public GameObject chargingEffect;
    public GameObject deathExplosion;
    public GameObject enemyProjectile;
}
