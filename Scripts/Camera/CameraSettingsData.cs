using UnityEngine;

namespace CollegeTD
{ 
    [CreateAssetMenu(menuName = "ScriptableObject/CameraSettingsData")]
    public class CameraSettingsData : ScriptableObject
    {
        [field: Space, Header("Speed Settings")]
        [field: SerializeField]
        public float Speed { get; private set; }

        [field: Header("Height Settings")]
        [field: SerializeField]
        public float Height { get; private set; }

        [field: Header("X Value Settings")]
        [field: SerializeField]
        public float MaxX { get; private set; }
        [field: SerializeField]
        public float MinX { get; private set; }

        [field: Header("Z Value Settings")]
        [field: SerializeField]
        public float MaxZ { get; private set; }
        [field: SerializeField]
        public float MinZ { get; private set; }
    }
}
