using UnityEngine;

namespace Assets.Scripts.Components.Items
{
    public class ConsumableComponent : ItemComponent
    {
        [SerializeField] protected int _quantity;

        public int Quantity { get => _quantity; }
    }
}
