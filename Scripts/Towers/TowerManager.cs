using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CollegeTD
{
    public class TowerManager : MonoBehaviour
    {
        [field: Space, Header("Event Reference")]
        [field: SerializeField]
        public OnTowerDestroyEvent OnTowerDestroy { get; set; }

        public static TowerManager Instance { get; private set; }
        private TowerController CurrentTower { get; set; }
        private List<TowerController> TowerControllerCollection { get; set; } = new List<TowerController>();

        public void TowerBuilt (TowerController tower)
        {
            CurrentTower = null;
            tower.OnTowerBuild.RemoveListener(TowerBuilt);
        }

        public void TrySpawnTowerPrefab (TowerController tower)
        {
            CheckIfCurrentTowerIsNull();
            CurrentTower = Instantiate(tower);
            TowerControllerCollection.Add(CurrentTower);
            CurrentTower.OnTowerBuild.AddListener(TowerBuilt);
        }

        protected virtual void Awake ()
        {
            Initialize();
        }

        protected virtual void OnEnable ()
        {
            SubscribeEvent();
        }

        protected virtual void OnDisable ()
        {
            UnsubscribeEvent();
        }

        private void SubscribeEvent ()
        {
            GameManager.Instance.OnGameEnd += HandleGameEnd;
        }

        private void UnsubscribeEvent ()
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.OnGameEnd -= HandleGameEnd;
            }
        }

        private void HandleGameEnd ()
        {
            DestroySpawnedTowers();
        }

        private void Initialize ()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }

        private void DestroySpawnedTowers ()
        {
            foreach (TowerController tower in TowerControllerCollection)
            {
                if (tower.IsControlled == true)
                {
                    OnTowerDestroy.Invoke(true);
                }

                Destroy(tower.gameObject);
            }

            TowerControllerCollection.Clear();
        }

        private void CheckIfCurrentTowerIsNull ()
        {
            if (CurrentTower != null)
            {
                Destroy(TowerControllerCollection.Last().gameObject);
                TowerControllerCollection.Remove(TowerControllerCollection.Last());
            }
        }
    }
}