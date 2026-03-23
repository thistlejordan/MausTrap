using Assets.Scripts.Components.GameWorld;
using Assets.Scripts.Components.Items;
using Assets.Scripts.Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Components
{
    public class KeyChainComponent : ContainerComponent
    {
        // Dictionary for keys by level
        // LevelEnum: (keyCount, hasMaster)
        private Dictionary<LevelEnum, (int, bool)> keyDictionary;

        // Event triggered when key count changes
        public event Action<LevelEnum, int> OnKeyCountChanged;

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
            if (!this.keyDictionary.ContainsKey(@lock.Level))
            {
                this.keyDictionary.Add(@lock.Level, (0, false));
            }

            return !@lock.RequiresBigKey ? this.keyDictionary[@lock.Level].Item1 > 0 : this.keyDictionary[@lock.Level].Item2;
        }

        public void UseKey(LockComponent @lock)
        {
            if (this.HasKey(@lock))
            {
                if (@lock.RequiresBigKey)
                {
                    return;
                }

                this.keyDictionary[@lock.Level] = (this.keyDictionary[@lock.Level].Item1 - 1, this.keyDictionary[@lock.Level].Item2);
                OnKeyCountChanged?.Invoke(@lock.Level, this.keyDictionary[@lock.Level].Item1);
                return;
            }

            Debug.LogWarning($"Tried using key for {@lock.Level}, but don't have one. Should use 'HasKey' before calling 'UseKey'");
        }

        public int GetKeyCount(LevelEnum level)
        {
            if (!this.keyDictionary.ContainsKey(level))
            {
                return 0;
            }

            return this.keyDictionary[level].Item1;
        }

        public bool HasBigKey(LevelEnum level)
        {
            if (!this.keyDictionary.ContainsKey(level))
            {
                return false;
            }

            return this.keyDictionary[level].Item2;
        }

        public override IEnumerator IAddItem(ItemComponent item, PlayerComponent player, Action<ItemComponent> callback)
        {
            var key = item.GetComponent<KeyComponent>();

            // If item is not a key or if it's a duplicate big key, return it
            if (key is null)
            {
                callback(item);
                yield break;
            }

            if (key.IsBigKey && this.keyDictionary.ContainsKey(key.Level) && this.keyDictionary[key.Level].Item2)
            {
                callback(item);
                yield break;
            }

            // Initialize the level if it doesn't exist
            if (!this.keyDictionary.ContainsKey(key.Level))
            {
                this.keyDictionary.Add(key.Level, (0, false));
            }

            // Add the key
            if (key.IsBigKey)
            {
                this.keyDictionary[key.Level] = (this.keyDictionary[key.Level].Item1, true);
            }
            else
            {
                this.keyDictionary[key.Level] = (this.keyDictionary[key.Level].Item1 + 1, this.keyDictionary[key.Level].Item2);
            }

            // Notify listeners of the key count change
            OnKeyCountChanged?.Invoke(key.Level, this.keyDictionary[key.Level].Item1);

            // Play animation and sound
            AudioManager.Instance.m_AudioSource.PlayOneShot(AudioManager.Instance.m_ItemGetSoundEffect, 0.1f);
            yield return player.IAwait(item.IItemGetAnimation(), InputType.Character);
            Destroy(item);
            callback(null);
        }
    }
}
