using System;
using UnityEngine.Events;

namespace CollegeTD
{
    [Serializable]
    public class OnEnemyDestroyEvent : UnityEvent<Enemy> { }

}