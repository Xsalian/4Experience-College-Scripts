using System;
using UnityEngine.Events;

namespace CollegeTD
{
    [Serializable]
    public class OnTowerBuildEvent : UnityEvent<TowerController> { }
}