using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CollegeTD
{
    public class TakeControlOnTower : MonoBehaviour
    {
        [field: Space, Header("Camera References")]
        [field: SerializeField]
        private Camera MainCamera { get; set; }

        [field: Space, Header("CameraController References")]
        [field: SerializeField]
        private CameraController CameraControllerCurrent { get; set; }

        [field: Space, Header("Raycast Settings")]
        [field: SerializeField]
        private float MaxDistance { get; set; }
        [field: SerializeField]
        private LayerMask TowerLayerMask { get; set; }
        [field: SerializeField]
        private KeyCode CastInput { get; set; }

        protected virtual void Update ()
        {
            HandleInput();
        }

        private void HandleInput ()
        {
            if (Input.GetKeyDown(CastInput))
            {
                CastCheckingRay();
            }
        }

        private void CastCheckingRay ()
        {
            Ray vRay = MainCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(vRay, out RaycastHit xHit, MaxDistance, TowerLayerMask) == true)
            {
                TowerController towerController = xHit.transform.gameObject.GetComponent<TowerController>();

                if (towerController.WasBuild == true && CameraControllerCurrent.IsControllingTower == false)
                {
                    CameraControllerCurrent.CameraOnTower(towerController);
                }
            }
        }
    }
}
