using UnityEngine;

namespace CollegeTD
{
    public class CameraController : MonoBehaviour
    {
        [field: Space, Header("Event Reference")]
        [field: SerializeField]
        public OnTakeControlEvent OnTakeControl { get; set; }

        [field: Space, Header("Camera References")]
        [field: SerializeField]
        private Transform CameraSlot { get; set; }
        
        [field: Space, Header("Camera Settings References")]
        [field: SerializeField]
        private CameraSettingsData CameraSettings { get; set; }

        [field: Space, Header("Axis References")]
        [field: SerializeField]
        private string HorizontalAxis { get; } = "Horizontal";
        [field: SerializeField]
        private string VerticalAxis { get; } = "Vertical";
        [field: SerializeField]
        private string MouseXAxis { get; } = "Mouse X";
        [field: SerializeField]
        private string MouseYAxis { get; } = "Mouse Y";

        [field: Space, Header("ExitInput References")]
        [field: SerializeField]
        private KeyCode ExitInput { get; set; }

        [field: Space, Header("MouseSensitivity Setting")]
        [field: SerializeField]
        private float MouseSensitivity { get; set; }

        public bool IsControllingTower { get; set; }
        private float PositionX { get; set; }
        private float PositionZ { get; set; }
        private TowerController CurrentTowerController { get; set; }
        private Quaternion StartRotation { get; set; }
        private float RotationX { get; set; }
        private float RotationY { get; set; }

        public void CameraOnTower (TowerController towerController)
        {
            CurrentTowerController = towerController;
            CurrentTowerController.TakingControlOnTower();
            IsControllingTower = true;
            transform.SetParent(CurrentTowerController.ProjectileParentControlled);
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.Euler(Vector3.zero);
            OnTakeControl.Invoke(true, CurrentTowerController);
            Cursor.lockState = CursorLockMode.Locked;
        }

        protected virtual void Start ()
        {
            Initialize();
        }

        protected virtual void Update ()
        {
            MoveCamera();
            HandleInput();
        }

        private void Initialize()
        {
            PositionX = CameraSlot.position.x;
            PositionZ = CameraSlot.position.z;
            IsControllingTower = false;
            StartRotation = transform.rotation;
            TowerManager.Instance.OnTowerDestroy.AddListener(ExitControlledTower);
        }

        private void MoveCamera()
        {
            if (IsControllingTower == false)
            {
                HandleDefaultCameraMovement();
            }
            else
            {
                HandleTowerControlledCameraMovement();
            }
        }

        private void HandleInput ()
        {
            if (Input.GetKeyDown(ExitInput) == true && IsControllingTower == true)
            {
                ExitControlledTower(false);
            }
        }

        private void HandleDefaultCameraMovement ()
        {
            PositionX += Input.GetAxis(HorizontalAxis) * Time.deltaTime * CameraSettings.Speed;
            PositionX = Mathf.Clamp(PositionX, CameraSettings.MinX, CameraSettings.MaxX);
            PositionZ += Input.GetAxis(VerticalAxis) * Time.deltaTime * CameraSettings.Speed;
            PositionZ = Mathf.Clamp(PositionZ, CameraSettings.MinZ, CameraSettings.MaxZ);

            Debug.Log($"X:{PositionX} Y:{PositionZ}");
            CameraSlot.position = new Vector3(PositionX, CameraSettings.Height, PositionZ);
            Debug.Log($"Camera slot: {CameraSlot.position}");
        }

        private void HandleTowerControlledCameraMovement ()
        {
            float mouseX = Input.GetAxis(MouseXAxis) * MouseSensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis(MouseYAxis) * MouseSensitivity * Time.deltaTime;
            RotationX -= mouseY;
            RotationX = Mathf.Clamp(RotationX, -90.0f, 90.0f);
            RotationY += mouseX;
            CurrentTowerController.ProjectileParentControlled.localRotation = Quaternion.Euler(RotationX, RotationY, 0);
        }

        private void ExitControlledTower (bool isDestoryed)
        {
            transform.SetParent(null);
            transform.localRotation = StartRotation;

            if ( isDestoryed == false)
            {
                CurrentTowerController.ExitFromControlledTower();
            }

            IsControllingTower = false;
            Cursor.lockState = CursorLockMode.None;
            OnTakeControl.Invoke(false, CurrentTowerController);
        }
    }
}
