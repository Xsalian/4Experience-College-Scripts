using UnityEngine;
using UnityEngine.AI;

namespace CollegeTD
{
    public class SpawnPoint : MonoBehaviour
    {
        [field: SerializeField]
        public NavMeshModifier SpawnedNavMeshModifier { get; private set; }
    }
}