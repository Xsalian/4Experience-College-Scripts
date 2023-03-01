using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CollegeTD
{
    public class UltimateManager : SingletonMonoBehaviour<UltimateManager>
    {
        private UltimateController CurrentUltimate { get; set; }

        public void TrySpawnUltimate (UltimateController ultimate)
        {
            CheckIfCurrentUltimateIsNull();
            CurrentUltimate = Instantiate(ultimate);
            CurrentUltimate.OnUltimateUse.AddListener(UltimateUsed);
        }

        public void CheckIfCurrentUltimateIsNull ()
        {
            if (CurrentUltimate != null)
            {
                Destroy(CurrentUltimate.gameObject);
            }
        }

        public void UltimateUsed (UltimateController ultimate)
        {
            CurrentUltimate = null;
            ultimate.OnUltimateUse.RemoveListener(UltimateUsed);
        }
    }
}
