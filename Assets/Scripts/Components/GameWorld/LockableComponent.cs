using UnityEngine;

namespace Assets.Scripts.Components.GameWorld
{
    public abstract class LockableComponent : MonoBehaviour
    {
        public abstract void Unlock();
    }
}
