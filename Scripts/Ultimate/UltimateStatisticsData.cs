using UnityEngine;
using MilkShake;

namespace CollegeTD
{
    [CreateAssetMenu (menuName = "ScriptableObject/UltimateStatisticsData")]
    public class UltimateStatisticsData : ScriptableObject
    {
        [field: Space, Header("Cost")]
        [field: SerializeField]
        public int Cost { get; private set; }

        [field: Space, Header("Prewarm Attack Settings")]
        [field: SerializeField]
        public float AttackPrewarmTime { get; private set; }

        [field: Space, Header("Attack Settings")]
        [field: SerializeField]
        public float AttackDuration { get; private set; }
        [field: SerializeField]
        public int MaxTargets { get; private set; }
        [field: SerializeField]
        public float Range { get; private set; }
        [field: SerializeField]
        public int Damage { get; private set; }
        [field: SerializeField]
        public LayerMask EnemyLayerMask { get; private set; }

        [field: Space, Header("ShakePresent References")]
        [field: SerializeField]
        public ShakePreset CurrentShakePreset { get; private set; }
    }

}