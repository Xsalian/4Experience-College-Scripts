using UnityEngine;

namespace CollegeTD
{
    public class TowerButtonController : MonoBehaviour
    {
        public void SpawnTowerButton (TowerController towerController)
        {
            GameManager.Instance.TryBuyTower(towerController);
        }
    }
}
