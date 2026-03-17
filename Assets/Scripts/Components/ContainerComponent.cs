using Assets.Scripts.Components.Items;
using System;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Components
{
    public abstract class ContainerComponent : MonoBehaviour
    {
        public abstract IEnumerator IAddItem(ItemComponent item, PlayerComponent player, Action<ItemComponent> callback);
    }
}
