using UnityEngine;

namespace CollegeTD
{
    [CreateAssetMenu(menuName = "ScriptableObject/PlaceUltimateSettingsData")]
    public class PlaceUltimateSettingsData : ScriptableObject
    {

        [field: Space, Header("Raycast Settings")]
        [field: SerializeField]
        public LayerMask FloorLayerMask { get; private set; }
        [field: SerializeField]
        public float MaxDistance { get; private set; }

        [field: Space, Header("Place Input")]
        [field: SerializeField]
        public KeyCode PlaceInput { get; private set; }
    }
}
