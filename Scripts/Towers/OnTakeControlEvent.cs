using System;
using UnityEngine.Events;

namespace CollegeTD 
{
    [Serializable]
    public class OnTakeControlEvent : UnityEvent<bool, TowerController> { }
}
