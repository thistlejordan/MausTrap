using Assets.Scripts.Enums;
using UnityEngine;

namespace Assets.Scripts.Components.GameWorld
{
    public class LockComponent : MonoBehaviour
    {
        [SerializeField] private LevelEnum level;
        [SerializeField] private bool isLocked;
        [SerializeField] private bool requiresBigKey;

        public LevelEnum Level => this.level;
        public bool IsLocked => this.isLocked;
        public bool RequiresBigKey => this.requiresBigKey;

        public void Unlock(KeyChainComponent keyChain)
        {
            keyChain.UseKey(this);
            this.GetComponent<LockableComponent>().Unlock();
            this.isLocked = false;
        }
    }
}
