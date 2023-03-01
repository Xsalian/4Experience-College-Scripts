using UnityEngine;
using TMPro;

namespace CollegeTD
{
    public class UltimateButton : MonoBehaviour
    {
        [field: Space, Header("TowerController Reference")]
        [field: SerializeField]
        private UltimateController UltimateControllerCurrent { get; set; }

        [field: Space, Header("Text Reference")]
        [field: SerializeField]
        private TextMeshProUGUI ButtonText { get; set; }

        protected virtual void Awake()
        {
            Initialize();
        }

        private void Initialize()
        {
            ButtonText.text = "Cost: " + UltimateControllerCurrent.UltimateStatistics.Cost.ToString();
        }
    }
}