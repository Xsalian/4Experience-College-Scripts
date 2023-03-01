using UnityEngine;

namespace CollegeTD
{
    public class ParticleController : MonoBehaviour, IPooledObject
    {
        [field: Space, Header("Particle Reference")]
        [field: SerializeField]
        private ParticleSystem Particle { get; set; }

        private float Timer { get; set; }

        public void ObjectPooled ()
        {
            Initialize();
        }

        protected virtual void Update ()
        {
            DespawnTimer();
        }

        private void Initialize ()
        {
            Timer = 0;
        }

        private void DespawnTimer ()
        {
            Timer += Time.deltaTime;

            if (Timer >= Particle.main.duration)
            {
                ObjectPooler.Instance.DespawnToPool(transform.tag, gameObject);
            }
        }
    }
}
