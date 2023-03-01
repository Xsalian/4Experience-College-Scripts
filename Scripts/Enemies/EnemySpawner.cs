using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CollegeTD
{
    public class EnemySpawner : MonoBehaviour
    {
        [field: Space, Header("Navigation Points References")]
        [field: SerializeField]
        private SpawnPoint[] SpawnPointsCollection { get; set; }
        [field: SerializeField]
        private Transform EndPoint { get; set; }

        [field: Space, Header("EnemySpawnerSettingsData References")]
        [field: SerializeField]
        private EnemySpawnerSettingsData EnemySpawnerSettings { get; set; }

        private float TimerSpawn { get; set; }
        private bool CanSpawn { get; set; }
        private List<Enemy> EnemiesSpawned { get; set; } = new List<Enemy>();
        private int CurrentNumberOfEnemies { get; set; }
        private int CurrentNumberOfEnemiesDestroyed { get; set; }

        protected virtual void OnEnable()
        {
            SubscribeEvent();
        }

        protected virtual void Update ()
        {
            HandleKeyboardInput();
            TrySpawnEnemy();
        }

        protected virtual void OnDisable ()
        {
            UnsubscribeEvent();
        }

        private void SubscribeEvent ()
        {
            GameManager.Instance.OnGameStart += HandleGameStart;
            GameManager.Instance.OnGameEnd += HandleGameEnd;
        }

        private void UnsubscribeEvent ()
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.OnGameStart -= HandleGameStart;
                GameManager.Instance.OnGameEnd -= HandleGameEnd;
            }
        }

        private void HandleGameStart ()
        {
            Initialize();
        }

        private void HandleGameEnd ()
        {
            CanSpawn = false;
            DestroySpawnedEnemies();
        }

        private void Initialize ()
        {
            CanSpawn = true;
            CurrentNumberOfEnemies = 0;
            CurrentNumberOfEnemiesDestroyed = 0;
        }

        private void HandleKeyboardInput ()
        {
            if (Input.GetKeyDown(EnemySpawnerSettings.CreateEnemy) == true)
            {
                SpawnEnemy();
            }

            if (Input.GetKeyDown(EnemySpawnerSettings.DestroyAllEnemies) == true)
            {
                DestroySpawnedEnemies();
            }
        }

        private void TrySpawnEnemy ()
        {
            if (TimerSpawn <= EnemySpawnerSettings.SpawnDuration)
            {
                TimerSpawn += Time.deltaTime;
            }
            
            if (CanSpawn == true && CurrentNumberOfEnemies < EnemySpawnerSettings.NumberOfEnemies)
            {
                SpawnEnemy();
                float timeToWait = Mathf.Lerp(EnemySpawnerSettings.MaxSpawnDelay, EnemySpawnerSettings.MinSpawnDelay, TimerSpawn / EnemySpawnerSettings.SpawnDuration);
                StartCoroutine(WaitForNextSpawn(timeToWait));
            }
        }
        private void SpawnEnemy ()
        {
            SpawnPoint spawnPoint = GetRandomSpawnPoint();
            int spawnedPointArea = spawnPoint.SpawnedNavMeshModifier.area;

            Enemy enemy = Instantiate(EnemySpawnerSettings.EnemiesPrefabsCollection[GetRandomEnemy()], spawnPoint.transform);
            enemy.InitializeNavigation(spawnedPointArea, EndPoint.position);
            enemy.OnEnemyDestroy.AddListener(UnregisterEnemy);

            EnemiesSpawned.Add(enemy);
            CurrentNumberOfEnemies++;
        }

        private int GetRandomEnemy ()
        {
            int randomEnemy = UnityEngine.Random.Range(0, EnemySpawnerSettings.EnemiesPrefabsCollection.Length);
            return randomEnemy;
        }

        private SpawnPoint GetRandomSpawnPoint ()
        {
            int spawnPoint = UnityEngine.Random.Range(0, SpawnPointsCollection.Length);
            return SpawnPointsCollection[spawnPoint];
        }

        private void DestroySpawnedEnemies ()
        {
            foreach (Enemy enemy in EnemiesSpawned)
            {
                Destroy(enemy.gameObject);
            }

            EnemiesSpawned.Clear();
        }

        private void UnregisterEnemy (Enemy enemy)
        {
            EnemiesSpawned.Remove(enemy);
            CurrentNumberOfEnemiesDestroyed++;

            if (CurrentNumberOfEnemiesDestroyed == EnemySpawnerSettings.NumberOfEnemies)
            {
                GameManager.Instance.IsGameWin = true;
                GameManager.Instance.NotifyOnGameEnd();
            }

            enemy.OnEnemyDestroy.RemoveListener(UnregisterEnemy);
        }

        private IEnumerator WaitForNextSpawn (float time)
        {
            CanSpawn = false;
            yield return new WaitForSeconds(time);
            CanSpawn = true;
        }
    }
}
