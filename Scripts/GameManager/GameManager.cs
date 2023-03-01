using System;
using UnityEngine;

namespace CollegeTD
{
    public class GameManager : SingletonMonoBehaviour<GameManager>, IHitable, ICollector
    {
        [field: Space, Header("Event Reference")]
        [field: SerializeField]
        public OnHealthChangeEvent OnHealthChange { get; set; }
        [field: SerializeField]
        public OnMoneyChangeEvent OnMoneyChange { get; set; }

        [field: Space, Header("Player Statistics")]
        [field: SerializeField]
        private int MaxHealth { get; set; }
        [field: SerializeField]
        private int StartMoney { get; set; }

        public bool IsGameWin { get; set; }
        public int CurrentHealth { get; set; }
        public int CurrentMoney { get; set; }

        public event Action OnGameStart = delegate { };
        public event Action OnGameEnd = delegate { };

        public void TakeDamage (int damage)
        {
            CurrentHealth -= damage;
            OnHealthChange.Invoke(CurrentHealth);

            if (CurrentHealth <= 0)
            {
                IsGameWin = false;
                NotifyOnGameEnd();
            }
        }

        public void CollectMoney (int amount)
        {
            CurrentMoney += amount;
            OnMoneyChange.Invoke(CurrentMoney);
        }

        public void TryBuyTower (TowerController towerController)
        {
            if (CurrentMoney >= towerController.TowerStatistics.BuildCost)
            {
                CurrentMoney -= towerController.TowerStatistics.BuildCost;
                OnMoneyChange.Invoke(CurrentMoney);
                TowerManager.Instance.TrySpawnTowerPrefab(towerController);
            }
        }

        public void TryBuyUltimate(UltimateController ultimateController)
        {
            if (CurrentMoney >= ultimateController.UltimateStatistics.Cost)
            {
                CurrentMoney -= ultimateController.UltimateStatistics.Cost;
                OnMoneyChange.Invoke(CurrentMoney);
                UltimateManager.Instance.TrySpawnUltimate(ultimateController);
            }
        }

        public void NotifyOnGameEnd()
        {
            OnGameEnd();
        }

        public void NotifyOnGameStart()
        {
            OnGameStart();
        }

        protected override void Awake ()
        {
            base.Awake();
            Initialize();
        }

        protected virtual void Start ()
        {
            NotifyOnGameStart();
        }

        private void Initialize ()
        {
            CurrentHealth = MaxHealth;
            OnHealthChange.Invoke(CurrentHealth);
            CurrentMoney = StartMoney;
            OnMoneyChange.Invoke(CurrentMoney);
        }
    }
}