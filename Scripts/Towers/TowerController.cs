using System.Collections;
using UnityEngine;
using MilkShake;

namespace CollegeTD
{
    public class TowerController : MonoBehaviour
    {
        [field: Space, Header("Event Reference")]
        [field: SerializeField]
        public OnTowerBuildEvent OnTowerBuild { get; set; }
        [field: SerializeField]
        public OnCanAttackValueChangeEvent OnCanAttackValueChange { get; set; }

        [field: Space, Header("Particle Reference")]
        [field: SerializeField]
        public ParticleSystem RangeParticle { get; set; }

        [field: Space, Header("ProjectileParents References")]
        [field: SerializeField]
        public Transform ProjectileParentActive { get; set; }
        [field: SerializeField]
        public Transform ProjectileParentControlled { get; set; }

        [field: Space, Header("Tower Statistic Reference")]
        [field: SerializeField]
        public TowerStatisticsData TowerStatistics { get; set; }

        [field: Space, Header("Build Settings Reference")]
        [field: SerializeField]
        private BuildSettingsData BuildSettings { get; set; }

        [field: Space, Header("Tower Mode Reference")]
        [field: SerializeField]
        private GameObject TowerActiveMode { get; set; }
        [field: SerializeField]
        private GameObject TowerBuildMode { get; set; }
        [field: SerializeField]
        private GameObject TowerControlledMode { get; set; }

        [field: Space, Header("Render Reference")]
        [field: SerializeField]
        private Renderer[] RendererComponentCollection { get; set; }

        [field: Space, Header("Audio Reference")]
        [field: SerializeField]
        private AudioSource AudioSourceCurrent { get; set; }

        public bool IsControlled { get; set; }
        public bool WasBuild { get; set; }
        private Camera MainCamera { get; set; }
        private bool IsOnBuildGround { get; set; }
        private bool IsColliding { get; set; }
        private bool CouldBePlaced { get; set; }
        private bool CanBePlaced { get; set; }
        private bool CanAttack { get; set; }
        private ObjectPooler CurrentObjectPooler { get; set; }

        public void TakingControlOnTower ()
        {
            IsControlled = true;
            TowerActiveMode.SetActive(false);
            TowerControlledMode.SetActive(true);
        }

        public void ExitFromControlledTower ()
        {
            IsControlled = false;
            TowerActiveMode.SetActive(true);
            TowerControlledMode.SetActive(false);
        }

        protected virtual void Awake ()
        {
            Initialize();
        }

        protected virtual void Update ()
        {
            FollowCursor();
            HandleInput();
            TryBuildTower();
            AutoAttack();
        }

        protected virtual void OnTriggerStay (Collider other)
        {
            IsColliding = true;
        }

        protected virtual void OnTriggerExit (Collider other)
        {
            IsColliding = false;
        }

        private void Initialize ()
        {
            MainCamera = Camera.main;
            WasBuild = false;
            IsControlled = false;
            TowerActiveMode.SetActive(false);
            TowerBuildMode.SetActive(true);
            ParticleSystem.ShapeModule shape = RangeParticle.shape;
            shape.radius = TowerStatistics.FireRange;
            CurrentObjectPooler = ObjectPooler.Instance;
        }

        private void FollowCursor ()
        {
            if (WasBuild == false)
            {
                Ray vRay = MainCamera.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(vRay, out RaycastHit vHit, BuildSettings.MaxDistance, BuildSettings.FloorLayerMask) == true)
                {
                    transform.position = new Vector3(vHit.point.x, 0.0f, vHit.point.z);
                }

                IsOnBuildGround = Physics.Raycast(vRay, BuildSettings.MaxDistance, BuildSettings.BuildGroundLayerMask);
                CanBePlaced = CheckIfCanBePlaced();
                ChangeMaterials();
            }
        }

        private bool CheckIfCanBePlaced ()
        {
            return IsOnBuildGround == true && IsColliding == false;
        }

        private void ChangeMaterials ()
        {
            if (CanBePlaced != CouldBePlaced)
            {
                CouldBePlaced = CanBePlaced;

                foreach (Renderer renderer in RendererComponentCollection)
                {
                    renderer.material = (CanBePlaced == true) ? BuildSettings.CorrectPlacementMatierial : BuildSettings.WrongPlacementMatierial;
                }
            }
        }

        private void HandleInput ()
        {
            if (Input.GetKeyDown(TowerStatistics.ShootInput) == true && IsControlled == true)
            {
                Shoot();
            }
        }
        
        private void TryBuildTower ()
        {
            if (Input.GetKeyDown(BuildSettings.BuildTower) == true && CheckIfCanBePlaced() == true && WasBuild == false)
            {
                OnTowerBuild.Invoke(this);
                WasBuild = true;
                CanAttack = true;
                TowerActiveMode.SetActive(true);
                TowerBuildMode.SetActive(false);
            }
        }

        private void AutoAttack ()
        {
            if (IsAutoAttackAllowed() == false)
            {
                return;
            }

            StartCoroutine(WaitForNextAttack());
            Collider[] hitColliders = new Collider[TowerStatistics.MaxTargets];
            int numColliders = Physics.OverlapSphereNonAlloc(transform.position, TowerStatistics.FireRange, hitColliders, TowerStatistics.EnemyLayerMask);

            for (int i = 0; i < numColliders; i++)
            {
                Enemy enemy = hitColliders[i].GetComponent<Enemy>();
                ProjectileParentActive.LookAt(enemy.Target);
                InitializeProjectile(ProjectileParentActive);
            }
        }

        private void InitializeProjectile (Transform projectileParent)
        {
            GameObject projectileFromPool = CurrentObjectPooler.SpawnFromPool(TowerStatistics.Projectile.transform.tag, projectileParent.position, projectileParent.rotation);
            AudioSourceCurrent.Play();
            Projectile projectile = projectileFromPool.GetComponent<Projectile>();
            projectile.SetBulletStatistics(TowerStatistics.Damage, TowerStatistics.Speed, TowerStatistics.LifeTime);
        }

        private bool IsAutoAttackAllowed ()
        {
            return WasBuild == true && CanAttack == true && IsControlled == false;
        }

        private void Shoot ()
        {
            if (CanAttack == true)
            {
                StartCoroutine(WaitForNextAttack());
                Shaker.ShakeAll(TowerStatistics.CurrentShakePreset);
                InitializeProjectile(ProjectileParentControlled);
            }
        }

        private IEnumerator WaitForNextAttack()
        {
            CanAttack = false;
            OnCanAttackValueChange.Invoke(CanAttack);
            yield return new WaitForSeconds(TowerStatistics.FireDelay);
            CanAttack = true;
            OnCanAttackValueChange.Invoke(CanAttack);
        }
    }
}