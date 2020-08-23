using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    public static WaveSpawner Instance;

    public static int enemiesAlive = 0;

    public Wave[] waves;
    private List<Enemy> enemies;
    private List<Transform> enemiesTransform;

    public Transform spawnPoint;

    public float timeBetweenWaves = 5f;
    private float countdown = 2f;

    private int waveIndex;
    public List<Transform> GetEnemiesTransform() => enemiesTransform;

    private void Awake()
    {
        if (Instance) 
        {
            Destroy(Instance.gameObject);
        }
        Instance = this;
        this.enemies = new List<Enemy>();
        this.enemiesTransform = new List<Transform>();
    }

    private void OnDestroy()
    {
        enemiesAlive = 0;
        Instance = null;
    }

    private void Update()
    {
        if (enemiesAlive > 0)
        {
            return;
        }

        if (waveIndex == waves.Length)
        {
            //Level Completed
            this.enabled = false;
        }

        if (countdown <= 0f)
        {
            StartCoroutine(SpawnWave());
            countdown = timeBetweenWaves;
            return;
        }

        countdown -= Time.deltaTime;
    }

    IEnumerator SpawnWave()
    {
        Wave wave = waves[waveIndex];

        for (int i = 0; i < wave.enemies.Length; i++)
        {
            SpawnEnemy(wave.enemies[i]);
            yield return new WaitForSeconds(1f / wave.spawnRate);
        }

        waveIndex++;
    }

    private void SpawnEnemy(Enemy enemy)
    {
        GameObject spawnedEnemy = Instantiate(enemy.gameObject, spawnPoint.position, spawnPoint.rotation);
        enemies.Add(spawnedEnemy.GetComponent<Enemy>());
        enemiesTransform.Add(spawnedEnemy.transform);
        enemiesAlive = enemies.Count;
    }

    public void RemoveEnemy(Enemy enemy)
    {
        enemiesTransform.Remove(enemy.transform);
        enemies.Remove(enemy);
        enemiesAlive = enemies.Count;
    }
}
