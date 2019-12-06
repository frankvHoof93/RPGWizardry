using nl.SWEG.RPGWizardry.Utils.DataTypes;
using System.Collections.Generic;
using UnityEngine;

public class DEMOSpawner : MonoBehaviour
{
    #region Variables
    #region Editor
    /// <summary>
    /// Cap for Population
    /// </summary>
    private int populationCap = 10;
    /// <summary>
    /// Prefabs for Enemies
    /// </summary>
    [SerializeField]
    [Tooltip("Prefabs for Enemies")]
    private GameObject[] enemyPrefabs;
    /// <summary>
    /// Positions for Spawning
    /// </summary>
    [SerializeField]
    [Tooltip("Positions for Spawning")]
    private Transform[] spawnPositions;
    /// <summary>
    /// Minimum Timeout between Spawns
    /// </summary>
    [SerializeField]
    [Tooltip("Minimum Timeout between Spawns")]
    private float minimumSpawnTimeout;
    /// <summary>
    /// Range for TimeOut between Spawns
    /// </summary>
    [SerializeField]
    [Tooltip("Range for TimeOut between Spawns")]
    private FloatRange spawnTiming;
    #endregion

    #region Private
    /// <summary>
    /// Current TimeOut before spawning next Enemy
    /// </summary>
    private float currTimeout;
    /// <summary>
    /// List of Enemies, used to check for population-cap
    /// </summary>
    private readonly List<GameObject> enemies = new List<GameObject>();
    #endregion
    #endregion

    #region Methods
    /// <summary>
    /// Spawns first Enemy
    /// </summary>
    private void Start()
    {
        Spawn();
    }
    /// <summary>
    /// Spawns Enemies on TimeOut
    /// </summary>
    private void Update()
    {
        currTimeout -= Time.deltaTime;
        enemies.RemoveAll(g => g == null);
        if (enemies.Count < populationCap && currTimeout <= 0)
            Spawn();
    }
    /// <summary>
    /// Spawns a random Enemy
    /// </summary>
    private void Spawn()
    {
        GameObject enemy = Instantiate(enemyPrefabs[Random.Range(0, enemyPrefabs.Length)]);
        enemy.transform.position = spawnPositions[Random.Range(0, spawnPositions.Length)].position;
        currTimeout = minimumSpawnTimeout + spawnTiming.Random;
        enemies.Add(enemy);
    }
    #endregion
}