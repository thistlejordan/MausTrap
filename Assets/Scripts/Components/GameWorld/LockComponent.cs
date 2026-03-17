using Assets.Scripts.Enums;
using UnityEngine;

namespace Assets.Scripts.Components.GameWorld
{
    public class LockComponent : MonoBehaviour
    {
        [SerializeField] private LevelEnum level;
        [SerializeField] private bool isLocked;
        [SerializeField] private bool requiresBigKey;

        public LevelEnum Level { get => this.level; }
        public bool IsLocked { get => this.isLocked; }
        public bool RequiresBigKey { get => this.requiresBigKey; }

        public void Unlock(KeyChainComponent keyChain)
        {
            keyChain.UseKey(this);
            this.GetComponent<Collider2D>().enabled = false;
            this.isLocked = false;
        }
    }
}
