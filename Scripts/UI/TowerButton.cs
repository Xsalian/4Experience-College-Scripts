using UnityEngine;
using TMPro;

namespace CollegeTD
{
    public class TowerButton : MonoBehaviour
    {
        [field: Space, Header("TowerController Reference")]
        [field: SerializeField]
        private TowerController TowerControllerCurrent { get; set; }

        [field: Space, Header("Text Reference")]
        [field: SerializeField]
        private TextMeshProUGUI ButtonText { get; set; }

        protected virtual void Awake ()
        {
            Initialize();
        }

        private void Initialize ()
        {
            ButtonText.text = "Cost: " + TowerControllerCurrent.TowerStatistics.BuildCost.ToString();
        }
    }
}
