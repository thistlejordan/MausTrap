using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Components
{
    public class HealthBarHUDComponent : MonoBehaviour
    {
        [SerializeField] private List<HeartHUDComponent> _hearts = new List<HeartHUDComponent>();

        public void SetMaximumHealth(int value) => throw new NotImplementedException();

        public void UpdateHealth(int value)
        {
            for (int i = 0; i < _hearts.Count; i++)
            {
                if (i < value)
                {
                    _hearts[i].SetToFull();
                }
                else
                {
                    _hearts[i].SetToEmpty();
                }
            }
        }
    }
}
