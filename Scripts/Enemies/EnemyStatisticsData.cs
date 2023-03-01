using UnityEngine;

namespace CollegeTD
{
    [CreateAssetMenu (menuName = "ScriptableObject/EnemyStatisticData")]
    public class EnemyStatisticsData : ScriptableObject
    {
        [field: SerializeField]
        public int HealthPoint { get; private set; }
        [field: SerializeField]
        public float Speed { get; private set; }
        [field: SerializeField]
        public int Damage { get; private set; }
        [field: SerializeField]
        public int CoinAmount { get; private set; }
        [field: SerializeField]
        public GameObject HitEffect { get; private set; }
    }
}