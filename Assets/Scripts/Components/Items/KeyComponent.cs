using Assets.Scripts.Enums;
using UnityEngine;

namespace Assets.Scripts.Components.Items
{
    public class KeyComponent :ItemComponent
    {
        [SerializeField] private LevelEnum level;
        [SerializeField] private bool isBigKey;

        public LevelEnum Level { get => this.level; }
        public bool IsBigKey { get => this.isBigKey; }
    }
}
