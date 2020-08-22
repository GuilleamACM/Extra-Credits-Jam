using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    public static WaveSpawner Instance;

    public static int EnemiesAlive = 0;

    public Wave[] waves;
    List<Transform> enemys;

    public Transform spawnPoint;

    public float timeBetweenWaves = 5f;
    private float countdown = 2f;

    private int waveIndex;

    public List<Transform> GetEnemys() => enemys;

    private void Awake()
    {
        if (Instance) 
        {
            Destroy(Instance.gameObject);
        }
        Instance = this;
        this.enemys = new List<Transform>();
    }

    private void OnDestroy()
    {
        Instance = null;
    }

    private void Update()
    {
        if (EnemiesAlive > 0)
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

        countdown = Mathf.Clamp(countdown, 0f, Mathf.Infinity);
    }

    IEnumerator SpawnWave()
    {
        Wave wave = waves[waveIndex];

        EnemiesAlive = wave.enemies.Length;

        for (int i = 0; i < wave.enemies.Length; i++)
        {
            SpawnEnemy(wave.enemies[i]);
            yield return new WaitForSeconds(1f / wave.spawnRate);
        }

        waveIndex++;
    }

    private void SpawnEnemy(GameObject enemy)
    {
        enemys.Add(Instantiate(enemy, spawnPoint.position, spawnPoint.rotation).transform);
    }
}
