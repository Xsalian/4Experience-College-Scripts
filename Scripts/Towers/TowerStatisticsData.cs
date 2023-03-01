using UnityEngine;
using MilkShake;

namespace CollegeTD 
{
    [CreateAssetMenu(menuName = "ScriptableObject/TowerAttackData")]
    public class TowerStatisticsData : ScriptableObject
    {
        [field: SerializeField]
        public float FireDelay { get; private set; }
        [field: SerializeField]
        public int Damage { get; private set; }
        [field: SerializeField]
        public float Speed { get; private set; }
        [field: SerializeField]
        public float LifeTime { get; private set; }
        [field: SerializeField]
        public float FireRange { get; private set; }
        [field: SerializeField]
        public int MaxTargets { get; private set; }
        [field: SerializeField]
        public LayerMask EnemyLayerMask { get; private set; }
        [field: SerializeField]
        public Projectile Projectile { get; private set; }
        [field: SerializeField]
        public int BuildCost { get; private set; }
        [field: SerializeField]
        public KeyCode ShootInput { get; private set; }
        [field: SerializeField]
        public ShakePreset CurrentShakePreset { get; private set; }
    }
}