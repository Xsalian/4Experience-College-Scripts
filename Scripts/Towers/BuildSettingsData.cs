using UnityEngine;

namespace CollegeTD
{
    [CreateAssetMenu(menuName = "ScriptableObject/BuildSettingsData")]
    public class BuildSettingsData : ScriptableObject
    {
        [field: Space, Header("Raycast Input")]
        [field: SerializeField]
        public LayerMask FloorLayerMask { get; private set; }
        [field: SerializeField]
        public float MaxDistance { get; private set; }
        [field: SerializeField]
        public LayerMask BuildGroundLayerMask { get; private set; }

        [field: Space, Header("Build Input")]
        [field: SerializeField]
        public KeyCode BuildTower { get; private set; }

        [field: Space, Header("Color Settings")]
        [field: SerializeField]
        public Material CorrectPlacementMatierial { get; private set; }
        [field: SerializeField]
        public Material WrongPlacementMatierial { get; private set; }
    }
}

