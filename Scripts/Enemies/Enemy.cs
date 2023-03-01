using UnityEngine;
using UnityEngine.AI;

namespace CollegeTD
{
    public class Enemy : MonoBehaviour, IHitable
    {
        [field: Space, Header("Target Reference")]
        [field: SerializeField]
        public Transform Target { get; set; }

        [field: Space, Header("Event Reference")]
        [field: SerializeField]
        public OnEnemyDestroyEvent OnEnemyDestroy { get; set; }

        [field: Space, Header("Navigation Agent Reference")]
        [field: SerializeField]
        private NavMeshAgent Agent{ get; set; }

        [field: Space, Header("LayerMask Reference")]
        [field: SerializeField]
        private LayerMask CastleLayerMask { get; set; }

        [field: Space, Header("Enemy Statistics")]
        [field: SerializeField]
        private EnemyStatisticsData EnemyStatistics { get; set; }

        private int CurrentHealth { get; set; }

        public void InitializeNavigation (int spawnPoint, Vector3 endPointVector)
        {
            Agent.areaMask |= 1 << spawnPoint;
            Agent.enabled = true;
            Agent.SetDestination(endPointVector);
        }

        public void TakeDamage (int damage)
        {
            CurrentHealth -= damage;
            
            if (CurrentHealth <= 0)
            {
                GameManager.Instance.CollectMoney(EnemyStatistics.CoinAmount);
                ObjectPooler.Instance.SpawnFromPool(EnemyStatistics.HitEffect.transform.tag, Target.position, Target.rotation);
                Destroy(gameObject);   
            }
        }

        protected virtual void Awake ()
        {
            Initialize();
        }

        protected virtual void OnTriggerEnter (Collider other)
        {
            if (CastleLayerMask.CheckIfContainsLayer(other.gameObject.layer) == true)
            {
                KillEnemy();
            }
        }

        protected virtual void OnDestroy ()
        {
            OnEnemyDestroy.Invoke(this);
        }

        private void Initialize ()
        {
            Agent.speed = EnemyStatistics.Speed;
            CurrentHealth = EnemyStatistics.HealthPoint;
        }

        private void KillEnemy ()
        {
            GameManager.Instance.TakeDamage(EnemyStatistics.Damage);
            Destroy(gameObject);
        }
    }
}

