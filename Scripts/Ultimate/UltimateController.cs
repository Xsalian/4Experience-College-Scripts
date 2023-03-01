using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MilkShake;

namespace CollegeTD
{
    public class UltimateController : MonoBehaviour
    {
        [field: Space, Header("UltimateStatistics Reference")]
        [field: SerializeField]
        public UltimateStatisticsData UltimateStatistics { get; set; }

        [field: Space, Header("Event Reference")]
        [field: SerializeField]
        public OnUltimateUseEvent OnUltimateUse { get; set; }

        [field: Space, Header("PlaceUltimateSettings Reference")]
        [field: SerializeField]
        private PlaceUltimateSettingsData PlaceUltimateSettings { get; set; }

        [field: Space, Header("AudioSource Reference")]
        [field: SerializeField]
        private AudioSource CurrentAudioSource { get; set; }

        [field: Space, Header("Particle References")]
        [field: SerializeField]
        private GameObject PrewarmParticle { get; set; }
        [field: SerializeField]
        private GameObject AttackParticle { get; set; }
        [field: SerializeField]
        private ParticleSystem RangeParticle { get; set; }

        private Camera MainCamera { get; set; }
        private bool WasPlaced { get; set; }
        private bool CanAttack { get; set; }

        protected virtual void Awake ()
        {
            Initialize();
        }

        protected virtual void Update ()
        {
            FollowCursor();
            TryPlaced();
            DealDamage();
        }

        private void Initialize ()
        {
            MainCamera = Camera.main;
            WasPlaced = false;
            CanAttack = false;
            ParticleSystem.ShapeModule shape = RangeParticle.shape;
            shape.radius = UltimateStatistics.Range;
        }

        private void FollowCursor ()
        {
            if (WasPlaced == false)
            {
                Ray vRay = MainCamera.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(vRay, out RaycastHit vHit, PlaceUltimateSettings.MaxDistance, PlaceUltimateSettings.FloorLayerMask) == true)
                {
                    transform.position = new Vector3(vHit.point.x, 0.0f, vHit.point.z);
                }
            }
        }

        private void TryPlaced ()
        {
            if (Input.GetKeyDown(PlaceUltimateSettings.PlaceInput) == true && WasPlaced == false)
            {
                WasPlaced = true;
                UseUltimate();
            }
        }

        private void UseUltimate ()
        {
            OnUltimateUse.Invoke(this);
            CurrentAudioSource.Play();
            StartCoroutine(PrewarmAttack());
        }

        private void Attack ()
        {
            PrewarmParticle.SetActive(false);
            RangeParticle.gameObject.SetActive(false);
            AttackParticle.SetActive(true);
            StartCoroutine(DealDamageTimer());
        }

        private void DealDamage ()
        {
            if (CanAttack)
            {
                Collider[] hitColliders = new Collider[UltimateStatistics.MaxTargets];
                int numColliders = Physics.OverlapSphereNonAlloc(transform.position, UltimateStatistics.Range, hitColliders, UltimateStatistics.EnemyLayerMask);

                for (int i = 0; i < numColliders; i++)
                {
                    IHitable enemy = hitColliders[i].GetComponent<IHitable>();
                    enemy.TakeDamage(UltimateStatistics.Damage);
                }
            }
        }

        private IEnumerator PrewarmAttack ()
        {
            yield return new WaitForSeconds(UltimateStatistics.AttackPrewarmTime);
            Attack();
        }

        private IEnumerator DealDamageTimer ()
        {
            CanAttack = true;
            Shaker.ShakeAll(UltimateStatistics.CurrentShakePreset);
            yield return new WaitForSeconds(UltimateStatistics.AttackDuration);
            CanAttack = false;
            Destroy(gameObject);
        }
    }
}