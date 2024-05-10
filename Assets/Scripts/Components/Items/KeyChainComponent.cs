using Assets.Scripts.Components.GameWorld;
using Assets.Scripts.Enums;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Components.Items
{
    public class KeyChainComponent : MonoBehaviour
    {
        // Dictionary for keys by level
        // LevelEnum: (keyCount, hasMaster)
        private Dictionary<LevelEnum, (int, bool)> keyDictionary;

        private void Awake()
        {
            this.keyDictionary = new Dictionary<LevelEnum, (int, bool)>();
        }

        public void AddKey(KeyComponent key)
        {
            if (!this.keyDictionary.ContainsKey(key.Level))
            {
                this.keyDictionary.Add(key.Level, (key.IsBigKey ? 0 : 1, key.IsBigKey));
                return;
            }

            if (key.IsBigKey || this.keyDictionary[key.Level].Item2)
            {
                this.keyDictionary[key.Level] = (this.keyDictionary[key.Level].Item1, true);
            }
            else
            {
                this.keyDictionary[key.Level] = (this.keyDictionary[key.Level].Item1 + 1, this.keyDictionary[key.Level].Item2);
            }
        }

        public bool HasKey(LockComponent @lock)
        {
            if (!this.keyDictionary.ContainsKey(@lock.Level)) this.keyDictionary.Add(@lock.Level, (0, false));
            return !@lock.RequiresBigKey ? this.keyDictionary[@lock.Level].Item1 > 0 : this.keyDictionary[@lock.Level].Item2;
        }

        public void UseKey(LockComponent @lock)
        {
            if (HasKey(@lock))
            {
                if (@lock.RequiresBigKey) return;
                else this.keyDictionary[@lock.Level] = (this.keyDictionary[@lock.Level].Item1 - 1, this.keyDictionary[@lock.Level].Item2);
            }
            else
            {
                Debug.LogWarning($"Tried using key for {@lock.Level}, but don't have one. Should use 'HasKey' before calling 'UseKey'");
            }
        }
    }
}
