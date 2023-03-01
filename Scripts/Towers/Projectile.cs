using UnityEngine;

namespace CollegeTD
{
    public class Projectile : MonoBehaviour, IPooledObject
    {
        private int Damage { get; set; }
        private float Speed { get; set; }
        private float LifeTime { get; set; }
        private float Timer { get; set; }

        public void ObjectPooled ()
        {
            Initialize();
        }

        public void SetBulletStatistics (int damage, float speed, float lifeTime)
        {
            Damage = damage;
            Speed = speed;
            LifeTime = lifeTime;         
        }

        protected virtual void Update ()
        {
            Move();
            CheckLifeTime();
        }

        protected virtual void OnTriggerEnter (Collider other)
        {
            TryDealDamage(other);
        }

        private void Initialize ()
        {
            Timer = 0;
        }

        private void Move ()
        {
            transform.position += transform.forward * Speed;
        }

        private void CheckLifeTime ()
        {
            Timer += Time.deltaTime;

            if (LifeTime <= Timer)
            {
                ObjectPooler.Instance.DespawnToPool(transform.tag, gameObject);
            }
        }

        private void TryDealDamage (Collider other)
        {
            IHitable enemy = other.GetComponent<IHitable>();

            if (enemy != null)
            {
                enemy.TakeDamage(Damage);
                ObjectPooler.Instance.DespawnToPool(transform.tag, gameObject);
            }
        }
    }
}