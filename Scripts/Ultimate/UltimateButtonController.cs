using UnityEngine;

namespace CollegeTD
{
    public class UltimateButtonController : MonoBehaviour
    {
        public void SpawnUltimateButton(UltimateController ultimate)
        {
            GameManager.Instance.TryBuyUltimate(ultimate);
        }
    }
}