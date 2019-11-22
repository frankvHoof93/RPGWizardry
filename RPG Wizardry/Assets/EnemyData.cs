using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "ScriptableObjects/EnemyData", order = 1)]
public class EnemyData : ScriptableObject
{
    [SerializeField]
    private string enemyName;
    [SerializeField]
    public int enemyHealth;
    [SerializeField]
    public int enemyAttack;
    [SerializeField]
    public float enemySpeed;
}