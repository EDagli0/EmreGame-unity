using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    [Header("Prefab & Spawn Points")]
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private Transform[] spawnPoints;

    [Header("Wave Settings")]
    [SerializeField] private float firstWaveDelay = 3f;
    [SerializeField] private float timeBetweenWaves = 20f;
    [SerializeField] private int enemiesPerWave = 5;
    [SerializeField] private int enemiesPerWaveIncrease = 2;

    [Header("Win Settings")]
    [SerializeField] private int winWave = 5; // buradan deðiþtir

    [Header("Optional UI (auto-find if empty)")]
    [SerializeField] private WaveUIController waveUI;

    private int waveIndex = 0;
    private float nextWaveTime = -1f;

    // sadece final wave için
    private int aliveFinalWaveEnemies = 0;
    private bool finalWaveSpawned = false;

    private void Start()
    {
        EnsureWaveUI();
        nextWaveTime = Time.time + firstWaveDelay;
    }

    private void Update()
    {
        if (GameManager.I != null && GameManager.I.IsGameOver) return;

        EnsureWaveUI();

        // final wave spawn edildiyse artýk yeni dalga yok
        if (finalWaveSpawned)
        {
            if (waveUI != null) waveUI.ClearNextWave();
            return;
        }

        // next wave UI
        if (waveUI != null)
        {
            float remain = Mathf.Max(0f, nextWaveTime - Time.time);
            waveUI.SetNextWaveSeconds(remain);
        }

        if (Time.time >= nextWaveTime)
        {
            SpawnWave();

            // final wave deðilse bir sonraki zamaný ayarla
            if (!finalWaveSpawned)
                nextWaveTime = Time.time + timeBetweenWaves;
        }
    }

    private void SpawnWave()
    {
        waveIndex++;

        int count = enemiesPerWave + (waveIndex - 1) * enemiesPerWaveIncrease;

        bool isFinalWave = (waveIndex >= winWave);
        if (isFinalWave)
        {
            finalWaveSpawned = true;
            aliveFinalWaveEnemies = 0;
        }

        for (int i = 0; i < count; i++)
        {
            if (spawnPoints == null || spawnPoints.Length == 0) break;

            Transform p = spawnPoints[Random.Range(0, spawnPoints.Length)];
            GameObject enemyObj = Instantiate(enemyPrefab, p.position, Quaternion.identity);

            // SADECE final wave için: ölümleri say
            if (isFinalWave)
            {
                var h = enemyObj.GetComponent<Health>();
                if (h != null)
                {
                    aliveFinalWaveEnemies++;
                    h.onDied.AddListener(OnFinalWaveEnemyDied);
                }
            }
        }

        // UI + GameManager bilgilendirme
        if (waveUI != null) waveUI.SetWave(waveIndex);
        if (GameManager.I != null) GameManager.I.SetWave(waveIndex);

        Debug.Log($"Spawned wave {waveIndex} with {count} enemies." + (isFinalWave ? " (FINAL)" : ""));
    }

    private void OnFinalWaveEnemyDied()
    {
        aliveFinalWaveEnemies = Mathf.Max(0, aliveFinalWaveEnemies - 1);

        if (aliveFinalWaveEnemies == 0)
        {
            // Final wave temizlendi -> WIN
            if (GameManager.I != null) GameManager.I.Win();
            else
            {
                Debug.Log($"YOU WIN! Cleared final wave {waveIndex}.");
                Time.timeScale = 0f;
            }
        }
    }

    private void EnsureWaveUI()
    {
        if (waveUI == null)
            waveUI = FindFirstObjectByType<WaveUIController>();
    }
}