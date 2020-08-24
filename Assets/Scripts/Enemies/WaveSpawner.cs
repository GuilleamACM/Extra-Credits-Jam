using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WaveSpawner : MonoBehaviour
{
    public static WaveSpawner Instance;

    public static int enemiesAlive = 0;

    public Wave[] waves;
    public Wave[] waves2;


    private List<Enemy> enemies;
    private List<Transform> enemiesTransform;

    public UnityEvent enemysAlive;
    bool enemysAliveLastFrame = false;
    public UnityEvent NoEnemysAlive;

    public Transform spawnPoint;
    public Transform spawnPoint2;

    public float timeBetweenWaves = 5f;
    private float countdown = 2f;

    private int waveIndex;
    private int waveIndex2;
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
            if (!enemysAliveLastFrame) 
            {
                enemysAliveLastFrame = true;
                enemysAlive.Invoke();
            }
            return;
        }
        enemysAliveLastFrame = false;
        NoEnemysAlive.Invoke();

        if (waveIndex == waves.Length)
        {
            PlayerStatus ps = PlayerStatus.Instance;
            if (ps.TotalMemory >= ps.UsedMemory + ps.BlockedMemory) 
            {
                GameManager.Instance.LevelComplete();
            }
            this.enabled = false;
        }

        if (countdown <= 0f)
        {
            StartCoroutine(SpawnWave(0));
            if(waves2 != null && waves2.Length > 0)
            {
                StartCoroutine(SpawnWave(1));
            }
            countdown = timeBetweenWaves;
            return;
        }

        countdown -= Time.deltaTime;
    }

    IEnumerator SpawnWave(int waveIdx)
    {
        if(waveIdx == 0)
        {
            Wave wave = waves[waveIndex];

            for (int i = 0; i < wave.enemies.Length; i++)
            {
                SpawnEnemy(wave.enemies[i], spawnPoint);
                yield return new WaitForSeconds(1f / wave.spawnRate);
            }

            waveIndex++;
        }
        if (waveIdx == 1)
        {
            Wave wave = waves2[waveIndex2];

            for (int i = 0; i < wave.enemies.Length; i++)
            {
                SpawnEnemy(wave.enemies[i], spawnPoint2);
                yield return new WaitForSeconds(1f / wave.spawnRate);
            }

            waveIndex2++;
        }
    }

    private void SpawnEnemy(Enemy enemy, Transform point)
    {
        GameObject spawnedEnemy = Instantiate(enemy.gameObject, point.position, point.rotation);
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
