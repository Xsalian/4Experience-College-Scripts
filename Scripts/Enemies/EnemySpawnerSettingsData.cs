using UnityEngine;

namespace CollegeTD
{
    [CreateAssetMenu (menuName = "ScriptableObject/EnemySpawnerSettingsData")]
    public class EnemySpawnerSettingsData : ScriptableObject
    {
        [field: Space, Header("Enemy prefabs")]
        [field: SerializeField]
        public Enemy[] EnemiesPrefabsCollection { get; private set; }

        [field: Space, Header("NumberOfEnemies  Setting")]
        [field: SerializeField]
        public int NumberOfEnemies { get; private set; }

        [field: Space, Header("Keyboard Input")]
        [field: SerializeField]
        public KeyCode CreateEnemy { get; private set; }
        [field: SerializeField]
        public KeyCode DestroyAllEnemies { get; private set; }

        [field: Space, Header("Spawn Settings")]
        [field: SerializeField]
        public float MaxSpawnDelay { get; private set; }
        [field: SerializeField]
        public float MinSpawnDelay { get; private set; }
        [field: SerializeField]
        public float SpawnDuration { get; private set; }
    }
}
