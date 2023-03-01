using UnityEngine;
using TMPro;

namespace CollegeTD
{
    public class GameUIManager : MonoBehaviour
    {
        [field: Space, Header("Text Reference")]
        [field: SerializeField]
        private TextMeshProUGUI HealthText { get; set; }
        [field: SerializeField]
        private TextMeshProUGUI MoneyText { get; set; }
        [field: SerializeField]
        private TextMeshProUGUI CoinText { get; set; }
        [field: SerializeField]
        private TextMeshProUGUI SummaryText { get; set; }

        [field: Space, Header("Text GameObject Reference")]
        [field: SerializeField]
        private GameObject ReloadTextGameObject { get; set; }

        [field: Space, Header("UI References")]
        [field: SerializeField]
        private GameObject GameUI { get; set; }
        [field: SerializeField]
        private GameObject SummaryUI { get; set; }
        [field: SerializeField]
        private GameObject TakeControlUI { get; set; }

        [field: Space, Header("CameraController References")]
        [field: SerializeField]
        private CameraController CameraControllerCurrent { get; set; }

        [field: Space, Header("Summary message")]
        [field: SerializeField]
        private string WinMessage { get; set; }
        [field: SerializeField]
        private string LoseMessage { get; set; }
        [field: SerializeField]
        private string CoinMessage { get; set; }

        public void ChangeHealthText (int value)
        {
            HealthText.text = value.ToString();
        }

        public void ChangeMoneyText (int value)
        {
            MoneyText.text = value.ToString();
        }

        public void SwitchUIPrefabs (bool isControllingTower, TowerController towerController)
        {
            if (isControllingTower == true)
            {
                GameUI.SetActive(false);
                TakeControlUI.SetActive(true);
                towerController.OnCanAttackValueChange.AddListener(ReloadTextDisplay);
            }
            else
            {
                GameUI.SetActive(true);
                TakeControlUI.SetActive(false);
                towerController.OnCanAttackValueChange.RemoveListener(ReloadTextDisplay);
            }
        }

        public void ExitButton()
        {
            LevelManager.Instance.LoadSceneAdditive(LevelManager.Instance.FirstSceneName);
        }

        protected virtual void Start ()
        {
            Initialize();
            InitializeTexts();
        }

        protected virtual void OnEnable ()
        {
            SubscribeEvent();
        }

        protected virtual void OnDisable ()
        {
            UnsubscribeEvent();
        }

        private void ReloadTextDisplay (bool canAttack)
        {
            if (canAttack == true)
            {
                ReloadTextGameObject.SetActive(false);
            }
            else
            {
                ReloadTextGameObject.SetActive(true);
            }
        }

        private void SubscribeEvent ()
        {
            GameManager.Instance.OnGameStart += HandleGameStart;
            GameManager.Instance.OnGameEnd += HandleGameEnd;
        }

        private void UnsubscribeEvent ()
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.OnGameStart -= HandleGameStart;
                GameManager.Instance.OnGameEnd -= HandleGameEnd;
            }
        }

        private void HandleGameStart ()
        {
            GameUI.SetActive(true);
        }

        private void HandleGameEnd ()
        {
            if (GameManager.Instance.IsGameWin == true)
            {
                SummaryText.text = WinMessage;
            }
            else
            {
                SummaryText.text = LoseMessage;
            }

            CoinText.text = CoinMessage + " " +GameManager.Instance.CurrentMoney.ToString();

            GameUI.SetActive(false);
            TakeControlUI.SetActive(false);
            SummaryUI.SetActive(true);
        }

        private void Initialize ()
        {
            GameManager.Instance.OnHealthChange.AddListener(ChangeHealthText);
            GameManager.Instance.OnMoneyChange.AddListener(ChangeMoneyText);
            CameraControllerCurrent.OnTakeControl.AddListener(SwitchUIPrefabs);
        }

        private void InitializeTexts ()
        {
            HealthText.text = GameManager.Instance.CurrentHealth.ToString();
            MoneyText.text = GameManager.Instance.CurrentMoney.ToString();
        }
    }
}